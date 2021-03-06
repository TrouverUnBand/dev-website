﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TrouveUnBand.Classes;

namespace TrouveUnBand.Models
{
    [Serializable]
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        [NotMapped]
        public string ConfirmPassword { get; set; }
        [NotMapped]
        public Photo ProfilePicture { get; set; }

        public sealed class UserMetadata
        {
            [Required(ErrorMessage = "Ce champ est requis")]
            [RegularExpression(@"^([-':a-zäáàâëéèêíìöóòôúùñçA-ZÂÄÀÊËÈÉÌÔÔÒÙÇ]){2,}$", ErrorMessage = "Doit avoir 2 caractères minimum et n'être composé que de lettres")]
            public string FirstName { get; set; }
            [Required(ErrorMessage = "Ce champ est requis")]
            [RegularExpression(@"^([-':a-zäáàâëéèêíìöóòôúùñçA-ZÂÄÀÊËÈÉÌÔÔÒÙÇ]){2,}$", ErrorMessage = "Doit avoir 2 caractères minimum et n'être composé que de lettres")]
            public string LastName { get; set; }
            [Required(ErrorMessage = "Ce champ est requis")]
            public DateTime BirthDate { get; set; }
            [Required(ErrorMessage = "Ce champ est requis")]
            [RegularExpression(@"^([0-9a-zäáàâëéèêíìöóòôúùñçA-ZÂÄÀÊËÈÉÌÔÔÒÙÇ]){3,}$", ErrorMessage = "Doit avoir 3 caractères minimum et n'être composé que de lettres")]
            public string Nickname { get; set; }
            [Required(ErrorMessage = "Ce champ est requis")]
            [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Le courriel doit être valide")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Ce champ est requis")]
            [RegularExpression(@"^([-':a-zäáàâëéèêíìöóòôúùñçA-ZÂÄÀÊËÈÉÌÔÔÒÙÇ]){2,}$", ErrorMessage = "Doit avoir 2 caractères minimum et n'être composé que de lettres")]
            public string Location { get; set; }
            [Required(ErrorMessage = "Ce champ est requis")]
            [RegularExpression(@"^[\S]{4,138}$", ErrorMessage = "Doit avoir 4 caractères minimum")]
            public string Password { get; set; }
            [Required(ErrorMessage = "Ce champ est requis")]
            public string Gender { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }        

        public bool isMusician()
        {
            if (Users_Instruments.Any())
            {
                return true;
            }

            return false;
        }

        public bool IsBandMember()
        {
            if (Bands.Any())
            {
                return true;
            }

            return false;
        }
    }
}
