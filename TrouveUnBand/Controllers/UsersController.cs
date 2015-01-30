using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using TrouveUnBand.Classes;
using TrouveUnBand.Models;
using TrouveUnBand.POCO;
using TrouveUnBand.Services;
using TrouveUnBand.ViewModels;

namespace TrouveUnBand.Controllers
{
    public class UsersController : BaseController
    {
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
        public ActionResult Register(User userModel)
        {
            var returnCode = "";
            if (ModelState.IsValid)
            {
                if (userModel.Password == userModel.ConfirmPassword)
                {
                    returnCode = Insertcontact(userModel);
                    if (returnCode == "")
                    {
                        Success(Messages.REGISTRATION_CONFIRMED, true);
                        FormsAuthentication.SetAuthCookie(userModel.Nickname, false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    returnCode = Messages.PASSWORD_NOT_MATCHING;
                }
            }

            Danger(returnCode, true);
            return View();
        }

        private string Insertcontact(User userbd)
        {
            try
            {
                var validUserQuery = (from user in db.Users
                                      where
                                      user.Email.Equals(userbd.Email) ||
                                      user.Nickname.Equals(userbd.Nickname)
                                      select new SearchUserInfo
                                      {
                                          Nickname = user.Nickname,
                                          Email = user.Email
                                      }).FirstOrDefault();

                if (validUserQuery == null)
                {
                    userbd.Photo = Photo.USER_STOCK_PHOTO_MALE;
                    if (userbd.Gender == "Femme")
                    {
                        userbd.Photo = Photo.USER_STOCK_PHOTO_FEMALE;
                    }

                    userbd.Password = Encrypt(userbd.Password);
                    userbd = Geolocalisation.SetUserLocation(userbd);
                    db.Users.Add(userbd);
                    db.SaveChanges();

                    return "";
                }

                return Messages.EXISTING_USER(userbd);
            }
            catch (Exception)
            {
                return Messages.INTERNAL_ERROR;
            }
        }

        private string Encrypt(string password)
        {
            var hash = SHA1.Create();
            var encoder = new ASCIIEncoding();
            var combined = encoder.GetBytes(password ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string returnLoginValid = LoginValid(model.Nickname, model.Password);
                if (returnLoginValid != "")
                {
                    model.Nickname = returnLoginValid;
                    FormsAuthentication.SetAuthCookie(model.Nickname, model.RememberMe);
                    return RedirectToAction("Index", "Home");
                }
                Danger(Messages.INVALID_LOGIN, true);
                return View();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private String LoginValid(string nicknameOrEmail, string password)
        {
            try
            {
                string encryptedPass = Encrypt(password);
                var loginQuery = (from user in db.Users
                                  where
                                  (user.Email.Equals(nicknameOrEmail) ||
                                  user.Nickname.Equals(nicknameOrEmail)) &&
                                  user.Password.Equals(encryptedPass)
                                  select new LoginModel
                                  {
                                      Nickname = user.Nickname,
                                      Email = user.Email,
                                      Password = user.Password
                                  }).FirstOrDefault();
                if (loginQuery != null)
                {
                    return loginQuery.Nickname;
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        private string UpdateProfil(ProfileModificationViewModel.UserInfo userInfo)
        {
            try
            {
                var loggedOnUser = db.Users.FirstOrDefault(u => u.User_ID == userInfo.UserId);
                SetUserInfo(loggedOnUser, userInfo);
                db.SaveChanges();

                return "";
            }
            catch
            {
                return Messages.INVALID_LOGIN;
            }
        }

        public ActionResult ProfileModification()
        {
            User loggedOnUser = UserDao.GetUserByNickname(User.Identity.Name);

            ProfileModificationViewModel profileModificationView =
                new ProfileModificationViewModel(loggedOnUser)
                {
                    MusicianInfos = {InstrumentList = new List<Instrument>(db.Instruments)}
                };


            return View(profileModificationView);
        }

        [HttpPost]
        public ActionResult UserProfileModification(ProfileModificationViewModel.UserInfo userModel)
        {
            var returnCode = UpdateProfil(userModel);

            if (returnCode == "")
            {
                Success(Messages.PROFILE_UPDATED, true);
                return RedirectToAction("Index", "Home");
            }

            Danger(Messages.INTERNAL_ERROR, true);
            return RedirectToAction("ProfileModification", "Users");
        }

        [HttpPost]
        public ActionResult MusicianProfileModification(ProfileModificationViewModel.MusicianInfo user2)
        {
            string instrumentList = Request["InstrumentList"];
            string[] instrumentArray = instrumentList.Split(',');

            if (AllUnique(instrumentArray))
            {
                User user = db.Users.FirstOrDefault(x => x.Nickname == User.Identity.Name);
                string skillList = Request["SkillsList"];
                string[] skillArray = skillList.Split(',');
                string descriptionMusician = Request["TextArea"];

                user.Description = descriptionMusician;
                user.Users_Instruments.Clear();

                for (int i = 0; i < instrumentArray.Length; i++)
                {
                    var userInstruments = new Users_Instruments();
                    int currentInstrumentId = Convert.ToInt32(instrumentArray[i]);
                    var instrument = db.Instruments.FirstOrDefault(x => x.Instrument_ID == currentInstrumentId);

                    userInstruments.Instrument_ID = instrument.Instrument_ID;
                    userInstruments.Skills = Convert.ToInt32(skillArray[i]);
                    userInstruments.User_ID = user.User_ID;

                    user.Users_Instruments.Add(userInstruments);
                }

                if (SaveUpdatedUser())
                {
                    Success(Messages.MUSICIAN_PROFILE_UPDATED, true);
                    return RedirectToAction("Index", "Home");
                }

                Danger(Messages.INTERNAL_ERROR);
                return RedirectToAction("ProfileModification", "Users");
            }

            Warning(Messages.INSTRUMENT_ALREADY_SELECTED, true);
            return RedirectToAction("ProfileModification", "Users");
        }

        private bool AllUnique(string[] array)
        {
            bool allUnique = array.Distinct().Count() == array.Length;
            return allUnique;
        }

        private User GetUserInfo(string nickname)
        {
            User loggedOnUser = db.Users.FirstOrDefault(x => x.Nickname == nickname);
            loggedOnUser.Photo = loggedOnUser.Photo;
            return loggedOnUser;
        }

        public string GetUserFullName()
        {
            var loggedOnUser = GetUserInfo(User.Identity.Name);
            return loggedOnUser.FirstName + " " + loggedOnUser.LastName;
        }

        public string GetPhotoSrc()
        {
            var loggedOnUser = GetUserInfo(User.Identity.Name);
            return loggedOnUser.Photo;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult SendImageForCrop(User userWithPhoto)
        {
            Image image;
            string photoSrc = userWithPhoto.ProfilePicture.PhotoSrc;

            try
            {
                if (!string.IsNullOrEmpty(photoSrc))
                {
                    if (!Photo.IsPhoto(photoSrc))
                    {
                        Danger(Messages.FILE_TYPE_INVALID, true);
                        return RedirectToAction("ProfileModification");
                    }

                    var webClient = new WebClient();
                    image = Image.FromStream(webClient.OpenRead(userWithPhoto.ProfilePicture.PhotoSrc));
                }
                else
                {
                    var postedPhoto = Request.Files[0];

                    if (postedPhoto.ContentLength == 0 || !Photo.IsPhoto(postedPhoto))
                    {
                        Danger(Messages.POSTED_FILES_ERROR, true);
                        return RedirectToAction("ProfileModification");
                    } 

                    image = Image.FromStream(postedPhoto.InputStream, true, true);
                }

                CropPhoto(image, userWithPhoto.ProfilePicture.CropRect);

                Success(Messages.PICTURE_CHANGED, true);
                return RedirectToAction("ProfileModification", "Users");
            }
            catch
            {
                Danger(Messages.INTERNAL_ERROR, true);
                return RedirectToAction("ProfileModification");
            }
        }

        private void CropPhoto(Image photoToCrop, Rectangle cropRectangle)
        {
            User loggedOnUser = db.Users.FirstOrDefault(x => x.Nickname == User.Identity.Name);

            if (photoToCrop.Width < 250 || photoToCrop.Height < 172 || photoToCrop.Width > 800 || photoToCrop.Height > 600)
            {
                photoToCrop = PhotoResizer.ResizeImage(photoToCrop, 250, 172, 800, 600);
            }

            var croppedPhoto = PhotoCropper.CropImage(photoToCrop, cropRectangle);
            var savedPhotoPath = FileHelper.SavePhoto(croppedPhoto, loggedOnUser.Nickname, FileHelper.Category.USER_PROFILE_PHOTO);
            loggedOnUser.Photo = savedPhotoPath;
            db.SaveChanges();
        }

        private void SetUserInfo(User currentUser, ProfileModificationViewModel.UserInfo newUser)
        {
            if ((currentUser.Latitude == 0.0 || currentUser.Longitude == 0.0) || currentUser.Location != newUser.Location)
            {
                currentUser.Location = newUser.Location;
                currentUser = Geolocalisation.SetUserLocation(currentUser);
            }

            currentUser.FirstName = newUser.FirstName;
            currentUser.LastName = newUser.LastName;
            currentUser.BirthDate = newUser.BirthDate;
            currentUser.Email = newUser.Email;

            if (currentUser.Gender != newUser.Gender &&
                currentUser.Photo.Contains("_stock_user_"))
            {
                currentUser.Photo = Photo.USER_STOCK_PHOTO_MALE;
                if (newUser.Gender == "Femme")
                {
                    currentUser.Photo = Photo.USER_STOCK_PHOTO_FEMALE;
                }
            }
            currentUser.Gender = newUser.Gender;
        }

        private bool SaveUpdatedUser()
        {
            try
            {
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
