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
            this.Bands = new HashSet<Band>();
            this.Musicians = new HashSet<Musician>();
        }
    
        public int GenreId { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Band> Bands { get; set; }
        public virtual ICollection<Musician> Musicians { get; set; }
    }
    
}
