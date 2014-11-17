//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace TrouveUnBand.Models
{
    public partial class User
    {
        public User()
        {
            this.Adverts = new HashSet<Advert>();
            this.Events = new HashSet<Event>();
            this.Users_Instruments = new HashSet<Users_Instruments>();
            this.Bands = new HashSet<Band>();
            this.Events1 = new HashSet<Event>();
            this.Genres = new HashSet<Genre>();
        }
    
        public int User_ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte[] Photo { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; }
        public System.DateTime CreationDate { get; set; }
    
        public virtual ICollection<Advert> Adverts { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Users_Instruments> Users_Instruments { get; set; }
        public virtual ICollection<Band> Bands { get; set; }
        public virtual ICollection<Event> Events1 { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
    }
    
}
