using System;
using System.Collections.Generic;
using System.Linq;
using TrouveUnBand.Classes;
using TrouveUnBand.Models;

namespace TrouveUnBand.Services
{
    public class UserDao
    {
        public static List<User> GetAllUsers()
        {
            var db = new TrouveUnBandEntities();
            var eventList = new List<User>();
            var user = db.Users.Where(x => x.Users_Instruments.Count > 0);
            eventList.AddRange(user);

            return eventList;
        }

        public static List<User> GetMusicians(string[] genres, string userName, string location, int radius)
        {
            var db = new TrouveUnBandEntities();
            var users = db.Users.ToList();
            var usersToRemove = new List<User>();

            
            if(!String.IsNullOrEmpty(userName))
            {
                users = users.Where(x => x.FirstName.Contains(userName) || 
                                   x.LastName.Contains(userName) || 
                                   x.Nickname.Contains(userName)).ToList();
            }

            foreach (var user in users)
            {
                if (!user.isMusician())
                {
                    usersToRemove.Add(user);
                }

                if (genres != null)
                {
                    foreach (var genre in genres)
                    {
                        if (user.Genres.All(x => x.Name != genre))
                        {
                            usersToRemove.Add(user);
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(location))
            {
                foreach (var user in users)
                {
                    if (Geolocalisation.CheckIfInRange(user.Location, location, radius))
                    {
                        usersToRemove.Add(user);
                    }
                }
            }

            foreach (var user in usersToRemove)
            {
                users.Remove(user);
            }

            return users;
        }

        public static List<User> GetMusicians(int? genreID, string userName, string location, int radius)
        {
            var db = new TrouveUnBandEntities();
            var users = db.Users.ToList();

            if (!String.IsNullOrEmpty(userName))
            {
                users = users.Where(user => user.FirstName.Contains(userName) ||
                                   user.LastName.Contains(userName) ||
                                   user.Nickname.Contains(userName)).ToList();
            }

            if (!String.IsNullOrEmpty(location))
            {
                var usersToRemove = new List<User>();

                foreach (var user in users)
                {
                    if (!Geolocalisation.CheckIfInRange(user.Location, location, radius))
                    {
                        usersToRemove.Add(user);
                    }
                }

                foreach (var user in usersToRemove)
                {
                    users.Remove(user);
                }
            }

            if (genreID != null)
            {
                users = users.Where(user => user.Genres.Any(genre => genre.Genre_ID == genreID)).ToList();
            }

            return users;
        }

        public static List<User> GetMusicians(string userName)
        {
            var db = new TrouveUnBandEntities();
            var users = db.Users.ToList();

            if (!String.IsNullOrEmpty(userName))
            {
                var userlist = new List<User>();
                foreach (var user in users)
                {
                    if (user.isMusician())
                    {
                        userlist.Add(user);
                    }
                }

                users = userlist.Where(user => user.FirstName.Contains(userName) ||
                                   user.LastName.Contains(userName) ||
                                   user.Nickname.Contains(userName)).ToList();
            }

            return users;
        }

        public static List<BandMemberModel> SearchBandMembers(string userName)
        {
            var db = new TrouveUnBandEntities();

            if (String.IsNullOrEmpty(userName))
            {
                var users = from bandMember in db.Users
                            select new BandMemberModel()
                            {
                                User_ID = bandMember.User_ID,
                                FirstName = bandMember.FirstName,
                                LastName = bandMember.LastName,
                                Nickname = bandMember.Nickname,
                                Location = bandMember.Location
                            };
                return users.ToList();
            }
                var Query = from bandMember in db.Users
                            where bandMember.FirstName.Contains(userName)
                                || bandMember.LastName.Contains(userName)
                                || bandMember.Nickname.Contains(userName)
                            select new BandMemberModel()
                            {
                                User_ID = bandMember.User_ID,
                                FirstName = bandMember.FirstName,
                                LastName = bandMember.LastName,
                                Nickname = bandMember.Nickname,
                                Location = bandMember.Location
                            };
                return Query.ToList();          
        }

        public static List<User> GetUsersById(int[] idsArray, TrouveUnBandEntities db)
        {
            return idsArray.Select(t => db.Users.FirstOrDefault(x => x.User_ID == t)).ToList();
        }

        public static List<User> GetUsersById(int[] idsArray)
        {
            var db = new TrouveUnBandEntities();
            return idsArray.Select(t => db.Users.FirstOrDefault(x => x.User_ID == t)).ToList();
        }

        public static User GetUserById(int userId)
        {
            var db = new TrouveUnBandEntities();
            User user = db.Users.FirstOrDefault(x => x.User_ID == userId);
            return user;
        }

        public static User GetUserByNickname(string nickname)
        {
            var db = new TrouveUnBandEntities();
            User user = db.Users.FirstOrDefault(x => x.Nickname == nickname);
            return user;
        }
    }
}
