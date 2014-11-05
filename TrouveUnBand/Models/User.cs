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
            this.Musicians = new HashSet<Musician>();
            this.Adverts = new HashSet<Advert>();
            this.Join_Users_Instrument = new HashSet<Join_Users_Instrument>();
            this.Bands = new HashSet<Band>();
            this.Genres = new HashSet<Genre>();
        }
    
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Location { get; set; }
        public byte[] Photo { get; set; }
        public string Gender { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<Musician> Musicians { get; set; }
        public virtual ICollection<Advert> Adverts { get; set; }
        public virtual ICollection<Join_Users_Instrument> Join_Users_Instrument { get; set; }
        public virtual ICollection<Band> Bands { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
    }
    
}
