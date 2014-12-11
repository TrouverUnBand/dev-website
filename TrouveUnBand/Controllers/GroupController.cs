﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using DotNetOpenAuth.Messaging;
using Microsoft.Ajax.Utilities;
using TrouveUnBand.Models;
using TrouveUnBand.POCO;
using TrouveUnBand.Services;
using TrouveUnBand.Classes;

namespace TrouveUnBand.Controllers
{
    public class GroupController : Controller
    {
        private TrouveUnBandEntities db = new TrouveUnBandEntities();

        public ActionResult Index()
        {
            var myBands = new List<Band>();
            if (Request.IsAuthenticated)
            {
                User currentUser = GetAuthenticatedUser();
                if (currentUser.IsBandMember())
                {
                    myBands = currentUser.Bands.ToList();
                }
            }
            return View(myBands);
        }

        public ActionResult Details(int id = 0)
        {
            var band = db.Bands.Find(id);
            if (band == null)
            {
                return HttpNotFound();
            }
            return View(band);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var subgenres = GenreDao.GetAllSubgenresByGenres();
            var user = GetAuthenticatedUser();
            var bandMember = GetAuthenticatedBandMember();

            ViewBag.AuthenticatedUser = new JavaScriptSerializer().Serialize(bandMember);
            ViewBag.Subgenres = subgenres;

            var band = new Band();
            band.Users.Add(user);

            return View(band);
        }

        [HttpPost]
        public ActionResult Create(string bandJSON)
        {
            var myBand = JsonToModel.ToBand(bandJSON);
            ViewBag.sUsers = myBand.Users.Aggregate("", (current, item) => current + (item.User_ID.ToString() + ";")).TrimEnd(';');
            ViewBag.sGenres = myBand.Genres.Aggregate("", (current, item) => current + (item.Genre_ID.ToString() + ";")).TrimEnd(';');

            return PartialView("_CreateConfirm", myBand);
        }

        public ActionResult Confirm(string bandName, string bandDesc, string bandLocation, string sGenres, string sUsers, string bandPhoto)
        {
            var myBand = new Band
            {
                Name = bandName, Description = bandDesc, Location = bandLocation, Photo = bandPhoto
            };
            myBand.Genres = GenreDao.GetGenresById(sGenres.Split(';').Select(n => Convert.ToInt32(n)).ToArray(), db);
            myBand.Users = UserDao.GetUsersById(sUsers.Split(';').Select(n => Convert.ToInt32(n)).ToArray(), db);

            var coord = Geolocalisation.GetCoordinatesByLocation(myBand.Location);
            myBand.Latitude = coord.latitude;
            myBand.Longitude = coord.longitude;

            var queryExistingBand = from Q in db.Bands
                                    where myBand.Name != null && Q.Name == myBand.Name
                                    select Q;
            var currentuser = GetAuthenticatedUser();
            if (queryExistingBand.Any())
            {
                var existingBand = queryExistingBand.ToList()[0];
                if (existingBand != null)
                {
                    TempData["warning"] = AlertMessages.EXISTING_BAND(existingBand, myBand);
                    db.Entry(existingBand).State = EntityState.Unchanged;
                }
            }

            try
            {
                db.Database.Connection.Open();
                db.Bands.Add(myBand);
                db.SaveChanges();

                var savedPhotoPath = CropAndSavePhoto(myBand);
                myBand.Photo = savedPhotoPath;
                db.SaveChanges();

                db.Database.Connection.Close();

                TempData["Success"] = AlertMessages.BAND_CREATION_SUCCESS(myBand);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["TempDataError"] = AlertMessages.INTERNAL_ERROR;
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult UpdateModal(Band model)
        {
            if (IsValidBand(model))
            {
                TempData["TempDataError"] = AlertMessages.EMPTY_INPUT;
                return View("Create");
            }

            return View("index");
        }


        public ActionResult Edit(int id = 0)
        {
            var band = db.Bands.Find(id);
            if (band == null)
            {
                return HttpNotFound();
            }
            return View(band);
        }

        [HttpPost]
        public ActionResult Edit(Band band)
        {
            if (ModelState.IsValid)
            {
                db.Entry(band).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(band);
        }

        public ActionResult Delete(int id = 0)
        {
            var band = db.Bands.Find(id);
            if (band == null)
            {
                return HttpNotFound();
            }
            return View(band);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var band = db.Bands.Find(id);
            band.Genres.Clear();
            band.Users.Clear();
            db.SaveChanges();
            db.Bands.Remove(band);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private bool CurrentUserIsAuthenticated()
        {
            return System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
        }

        private User GetAuthenticatedUser()
        {
            if (!CurrentUserIsAuthenticated())
            {
                throw new Exception("User is not authenticated");
            }
            var userName = System.Web.HttpContext.Current.User.Identity.Name;
            var authenticatedUser = db.Users.FirstOrDefault(x => x.Nickname == userName);

            return authenticatedUser;      
        }

        private BandMemberModel GetAuthenticatedBandMember()
        {
            if (!CurrentUserIsAuthenticated())
            {
                throw new Exception("User is not authenticated");
            }
            var userName = System.Web.HttpContext.Current.User.Identity.Name;
            var Query = from bandMember in db.Users
                        where bandMember.Nickname == userName
                        select new BandMemberModel()
                        {
                            User_ID = bandMember.User_ID,
                            FirstName = bandMember.FirstName,
                            LastName = bandMember.LastName,
                            Nickname = bandMember.Nickname,
                            Location = bandMember.Location
                        };

            return Query.ToList()[0];
        }

        public bool IsValidBand(Band myBand)
        {
            if (
                String.IsNullOrEmpty(myBand.Name)
                || String.IsNullOrEmpty(myBand.Location)
                || String.IsNullOrEmpty(myBand.Description)
                || !myBand.Genres.Any() 
                || myBand.Genres == null
                || !myBand.Users.Any() 
                || myBand.Users == null
            )
            {
                return true;
            }
            return false;
        }

        public bool ContainsMusician(List<User> myMusicians, int musicianId)
        {
            if (myMusicians.Any(x => x.User_ID == musicianId))
            {
                return false;
            }
            return true;
        }

        public ActionResult SearchMusician(string searchString)
        {
            var musicians = UserDao.SearchBandMembers(searchString);
            ViewBag.Results = musicians;
            ViewBag.ResultsCount = musicians.Count;

            return PartialView("_MusicianFinder");
        }

        private string CropAndSavePhoto(Band myBand)
        {
            var postedPhoto = Request.Files[0];

            if (postedPhoto.ContentLength == 0)
            {
                return Photo.BAND_STOCK_PHOTO;
            }

            Image image = Image.FromStream(postedPhoto.InputStream, true, true);

            if (image.Width < 250 || image.Height < 172 || image.Width > 800 || image.Height > 600)
            {
                image = PhotoResizer.ResizeImage(image, 250, 172, 800, 600);
            }

            var croppedPhoto = PhotoCropper.CropImage(image, myBand.PhotoCrop.CropRect);
            string bandPhotoName = myBand.Band_ID.ToString();
            var savedPhotoPath = FileHelper.SavePhoto(croppedPhoto, bandPhotoName, FileHelper.Category.BAND_PHOTO);

            if (savedPhotoPath == "")
            {
                savedPhotoPath = Photo.BAND_STOCK_PHOTO;
            }

            return savedPhotoPath;
        }
    }
}
