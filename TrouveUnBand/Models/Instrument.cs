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
    public partial class Instrument
    {
        public Instrument()
        {
            this.Join_Musician_Instrument = new HashSet<Join_Musician_Instrument>();
            this.Join_Users_Instrument = new HashSet<Join_Users_Instrument>();
        }
    
        public int InstrumentId { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Join_Musician_Instrument> Join_Musician_Instrument { get; set; }
        public virtual ICollection<Join_Users_Instrument> Join_Users_Instrument { get; set; }
    }
    
}
