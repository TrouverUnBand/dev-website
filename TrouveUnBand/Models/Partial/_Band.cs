﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using TrouveUnBand.Classes;

namespace TrouveUnBand.Models
{
    [MetadataType(typeof(BandMetadata))]
    public partial class Band
    {
        public sealed class BandMetadata
        {
            [Required(ErrorMessage = "Ce champ est requis")]
            [RegularExpression(@"^([a-zäáàëéèíìöóòúùñçA-ZÄÀËÈÉÌÔÒÙÇ]){2,}$", ErrorMessage = "Doit avoir 2 caractères minimum, et être composé que de lettres")]
            public string FirstName { get; set; }
            [Required(ErrorMessage = "Ce champ est requis")]
            [RegularExpression(@"^([a-zäáàëéèíìöóòúùñçA-ZÄÀËÈÉÌÔÒÙÇ]){2,}$", ErrorMessage = "Doit avoir 2 caractères minimum, et être composé que de lettres")]
            public string LastName { get; set; }
            [Required(ErrorMessage = "Ce champ est requis")]
            public DateTime BirthDate { get; set; }
            [Required(ErrorMessage = "Ce champ est requis")]
            [RegularExpression(@"^([0-9a-zäáàëéèíìöóòúùñçA-ZÄÀËÈÉÌÔÒÙÇ]){3,}$", ErrorMessage = "Doit avoir 3 caractères minimum")]
            public string Nickname { get; set; }
            [Required(ErrorMessage = "Ce champ est requis")]
            [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Le courriel doit être valide")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Ce champ est requis")]
            [RegularExpression(@"^[a-zäáàëéèíìöóòúùñçA-ZÄÀËÈÉÌÔÒÙÇ \-]{2,}$", ErrorMessage = "Doit avoir 2 caractères minimum, lettres seulement")]
            public string Location { get; set; }
            [Required(ErrorMessage = "Ce champ est requis")]
            [RegularExpression(@"^[\S]{4,138}$", ErrorMessage = "Doit avoir 4 caractères minimum")]
            public string Password { get; set; }
            [Required(ErrorMessage = "Ce champ est requis")]
            public string Gender { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public void SetLocation()
        {
            var coord = Geolocalisation.GetCoordinatesByLocation(this.Location);
            this.Location = coord.formattedAddress;
            this.Latitude = coord.latitude;
            this.Longitude = coord.longitude;
        }
    }
}