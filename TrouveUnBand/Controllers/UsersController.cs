﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrouveUnBand.Models;
using System.Data.SqlClient;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace TrouveUnBand.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            bool RC = false;
            if (ModelState.IsValid)
            {
                if (user.Password == user.ConfirmPassword)
                {
                    RC = Insertcontact(user);
                    if (RC == true)
                    {
                        TempData["notice"] = "Registration Confirmed";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["sqlerror"] = "Database Error";
                        return View();
                    }
                }
                else
                {
                    TempData["PasswordNotEqual"] = "Both password fields must be identical";
                    return View();
                }
            }
            return View();
        }

        private bool Insertcontact(User user)
        {
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=tempdb;Integrated Security=True";
            try
            {
                myConnection.Open();
                String query = String.Format("INSERT INTO Users(FirstName, LastName, BirthDate, Nickname, Email, Password, City) " +
                "Values ('{0}','{1}',convert(datetime,'{2}'),'{3}','{4}','{5}','{6}')", 
                user.FirstName, user.LastName, user.BirthDate, user.Nickname, user.Email, user.Password, user.City);
                SqlCommand insert = new SqlCommand(query, myConnection);
                insert.ExecuteNonQuery();

                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                return false;
            }
            finally
            {
                myConnection.Close();
            }
        }

        //TODO: Utiliser Une fonction de Hashage pour ne pas écrire le mots de passe en clair
        private string EncryptPassword(string password)
        {
            byte[] pass = Encoding.UTF8.GetBytes(password);
            MD5 encpwrd = new MD5CryptoServiceProvider();
            return Encoding.UTF8.GetString(encpwrd.ComputeHash(pass));
        }
    }
}
