//------------------------------------------------------------------------------
// <auto-generated>
//    Ce code a été généré à partir d'un modèle.
//
//    Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//    Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrouveUnBand.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Band
    {
        public Band()
        {
            this.Genres = new HashSet<Genre>();
            this.Musicians = new HashSet<Musician>();
        }
    
        public int BandId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
    
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Musician> Musicians { get; set; }
    }
}
