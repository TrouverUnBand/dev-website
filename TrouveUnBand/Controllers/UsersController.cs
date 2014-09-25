﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TrouveUnBand.Models;
using WebMatrix.WebData;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace TrouveUnBand.Controllers
{
    public class UsersController : Controller
    {
        private TrouveUnBandEntities db = new TrouveUnBandEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            string RC = "";
            if (ModelState.IsValid)
            {
                if (user.Password == user.ConfirmPassword)
                {
                    user.Photo = StockPhoto();
                    RC = Insertcontact(user);
                    if (RC == "")
                    {
                        TempData["notice"] = "Registration Confirmed";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    RC = "Both password fields must be identical";
                }
            }
            TempData["TempDataError"] = RC;
            return View();
        }

        private string Insertcontact(User user)
        {
            try
            {
                var ValidUserQuery = (from User in db.Users
                                      where
                                      User.Email.Equals(user.Email) ||
                                      User.Nickname.Equals(user.Email)
                                      select new SearchUserInfo
                                      {
                                          Nickname = User.Nickname,
                                          Email = User.Email
                                      }).FirstOrDefault();

                if (ValidUserQuery == null)
                {
                    db.Database.Connection.Open();
                    user.Password = Encrypt(user.Password);
                    db.Users.Add(user);
                    db.SaveChanges();
                    db.Database.Connection.Close();
                    return "";
                }
                else
                {
                    return "L'utilisateur existe déjà";
                }
            }
            catch
            {
                return "Une erreur interne s'est produite. Veuillez réessayer plus tard";
            }
        }

        private string Encrypt(string password)
        {
            var hash = System.Security.Cryptography.SHA1.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(password ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            string ReturnLoginValid = LoginValid(model.Nickname, model.Password);
            if (ReturnLoginValid != "")
            {
                model.Nickname = ReturnLoginValid;
                FormsAuthentication.SetAuthCookie(model.Nickname, model.RememberMe);
                return RedirectToAction("Index", "Home");
            }

            TempData["LoginFail"] = "Your nickname/email or password is incorrect. Please try again.";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private String LoginValid(string NicknameOrEmail,string Password)
        {
            try
            {
                string EncryptedPass = Encrypt(Password);
                var LoginQuery = (from User in db.Users
                                    where
                                    (User.Email.Equals(NicknameOrEmail) ||
                                    User.Nickname.Equals(NicknameOrEmail)) &&
                                    User.Password.Equals(EncryptedPass)
                                    select new Login
                                    {
                                        Nickname = User.Nickname,
                                        Email = User.Email,
                                        Password = User.Password
                                    }).FirstOrDefault();
                if (LoginQuery != null)
                {
                    return LoginQuery.Nickname;
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        private string Updatecontact(User user, ICollection<Instrument> InstrumentListe)
        {
            try
            {
                User LoggedOnUser = db.Users.FirstOrDefault(x => x.Nickname == user.Nickname);
                LoggedOnUser.LastName = user.LastName;
                LoggedOnUser.Location = user.Location;
                LoggedOnUser.BirthDate = user.BirthDate;
                LoggedOnUser.Email = user.Email;
                LoggedOnUser.FirstName = user.FirstName;
                LoggedOnUser.Photo = user.Photo;
                LoggedOnUser.Gender = user.Gender;
                db.SaveChanges();

                Musician MusicianQuery = db.Musicians.FirstOrDefault(x => x.UserId == LoggedOnUser.UserId);

                if (MusicianQuery == null)
                {
                    Musician Musicien = new Musician();
                    Instrument i = InstrumentListe.FirstOrDefault();
                    Musicien.Instruments.Add(i);
                    Musicien.UserId = LoggedOnUser.UserId;
                    db.Database.Connection.Open();
                    db.Musicians.Add(Musicien);
                    db.Database.Connection.Close();
                }
                else
                {
                    Instrument i = InstrumentListe.FirstOrDefault();
                    MusicianQuery.Instruments.Add(i);
                    db.SaveChanges();
                }

                return "";
            }
            catch
            {
                return "Une erreur interne s'est produite. Veuillez réessayer plus tard";
            }
        }

        public ActionResult ProfileModification()
        {
            ViewBag.InstrumentList = new SelectList(db.Instruments, "InstrumentId", "Name");
            User LoggedOnUser = GetUserInfo(User.Identity.Name);
            if (LoggedOnUser.Photo != null)
            {
                LoggedOnUser.PhotoName = "data:image/jpeg;base64," + Convert.ToBase64String(LoggedOnUser.Photo);
            }
            ViewData["UserData"] = LoggedOnUser;
            return View();
        }

        [HttpPost]
        public ActionResult ProfileModification(Musician music)
        {

            User user = music.User;
            user.Nickname = User.Identity.Name;
            string RC = "";
            if (Request.Files[0].ContentLength == 0)
            {
                user.Photo = GetProfilePicByte(user.Nickname);
                RC = Updatecontact(user, music.Instruments);
            }
            else
            {
                HttpPostedFileBase PostedPhoto = Request.Files[0];
                try
                {
                    Image img = Image.FromStream(PostedPhoto.InputStream, true, true);
                    byte[] bytephoto = imageToByteArray(img);
                    user.PhotoName = PostedPhoto.FileName;
                    user.Photo = bytephoto;
                }
                catch
                {
                    user.Photo = StockPhoto();
                }
                RC = Updatecontact(user, music.Instruments);
            }

            if (RC == "")
            {
                TempData["notice"] = "Profil mis à jour";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["TempDataError"] = "Une erreur interne s'est produite";
                return View();
            }

        }

        private User GetUserInfo(string Nickname)
        {
            User LoggedOnUser = db.Users.FirstOrDefault(x => x.Nickname == Nickname);
            return LoggedOnUser;
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public byte[] StockPhoto()
        {
            string path = HttpContext.Server.MapPath("~/Images/stock_user.jpg");
            Image stock = Image.FromFile(path);
            return imageToByteArray(stock);
        }

        public byte[] GetProfilePicByte(string nickname)
        {
            var PicQuery = (from User in db.Users
                            where
                            User.Nickname.Equals(nickname)
                            select new Photo
                            {
                                ProfilePicture = User.Photo
                            }).FirstOrDefault();
            return PicQuery.ProfilePicture;
        }
    }
}

