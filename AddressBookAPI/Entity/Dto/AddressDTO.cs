using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Dto
{
    public class AddressDTO
    {
        [Required]
        public string line1 { get; set; }

     
        public string line2 { get; set; }

        [Required]
        public string city { get; set; }

        [Required]
        public string zipCode { get; set; }

        [Required]
        public string state_name { get; set; }

        [Required]
        public TypeDTO country { get; set; }

        [Required]
        public TypeDTO type { get; set; }

    }
}
