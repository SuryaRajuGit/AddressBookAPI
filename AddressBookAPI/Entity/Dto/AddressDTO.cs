using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Models
{
    public class AddressDTO
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
        public TypeDTO country { get; set; }

        [Required]
        public TypeDTO type { get; set; }

    }
}
