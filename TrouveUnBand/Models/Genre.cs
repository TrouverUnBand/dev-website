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
    public partial class Genre
    {
        public Genre()
        {
            this.Genres1 = new HashSet<Genre>();
            this.Adverts = new HashSet<Advert>();
            this.Bands = new HashSet<Band>();
            this.Events = new HashSet<Event>();
            this.Users = new HashSet<User>();
        }
    
        public int Genre_ID { get; set; }
        public Nullable<int> Parent_ID { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Genre> Genres1 { get; set; }
        public virtual Genre Genre1 { get; set; }
        public virtual ICollection<Advert> Adverts { get; set; }
        public virtual ICollection<Band> Bands { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
    
}
