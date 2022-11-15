using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Models
{
    public class addressDTO
    {
        [Required]
        public string line1 { get; set; }

        [Required]
        public string line2 { get; set; }

        [Required]
        public string city { get; set; }

        [Required]
        public string zipCode { get; set; }

        [Required]
        public string stateName { get; set; }

        [Required]
        public typeDTO country { get; set; }

        [Required]
        public typeDTO type { get; set; }

    }
}
