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
            this.Users_Instruments = new HashSet<Users_Instruments>();
        }
    
        public int Instrument_ID { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Users_Instruments> Users_Instruments { get; set; }
    }
    
}
