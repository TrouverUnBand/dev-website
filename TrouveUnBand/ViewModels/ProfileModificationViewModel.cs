using System;
using System.Collections.Generic;
using TrouveUnBand.Classes;
using TrouveUnBand.Models;

namespace TrouveUnBand.ViewModels
{
    public class ProfileModificationViewModel
    {
        public UserInfo UserInfos { get; set; }
        public MusicianInfo MusicianInfos { get; set; }
        public IEnumerable<Band> ListOfBands { get; set; }

        public ProfileModificationViewModel()
        {
            UserInfos = new UserInfo();
            MusicianInfos = new MusicianInfo();
        }

        public ProfileModificationViewModel(User user)
        {
            UserInfos = new UserInfo(user);
            MusicianInfos = new MusicianInfo(user);
        }

        public class UserInfo
        {
            public int UserId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime BirthDate { get; set; }
            public string Gender { get; set; }
            public string Location { get; set; }
            public string Email { get; set; }
            public string Photo { get; set; }
            public Photo ProfilePicture { get; set; }

            public UserInfo()
            {
                
            }

            public UserInfo(User user)
            {
                UserId = user.User_ID;
                FirstName = user.FirstName;
                LastName = user.LastName;
                BirthDate = user.BirthDate;
                Gender = user.Gender;
                Location = user.Location;
                Email = user.Email;
                Photo = user.Photo;
            }
        }

        public class MusicianInfo
        {
            public string Description { get; set; }
            public ICollection<Users_Instruments> UsersInstruments { get; set; }
            public List<Instrument> InstrumentList { get; set; }

            public MusicianInfo()
            {
                
            }
                 
            public MusicianInfo(User user)
            {
                Description = user.Description;
                UsersInstruments = user.Users_Instruments;
            }
        }
    }
}