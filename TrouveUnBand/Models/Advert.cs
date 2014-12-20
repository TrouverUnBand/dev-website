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
    public partial class Advert
    {
        public Advert()
        {
            this.Genres = new HashSet<Genre>();
        }
    
        public int Advert_ID { get; set; }
        public int Creator_ID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public System.DateTime CreationDate { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public string Location { get; set; }
        public string Photo { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
    
        public virtual User User { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
    }
    
}
