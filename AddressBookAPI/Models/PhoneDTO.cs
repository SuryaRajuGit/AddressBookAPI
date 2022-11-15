using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Models
{
    public class phoneDTO
    {
        [Required]
        public string phoneNumber { get; set; }

        [Required]
        public typeDTO type { get; set; }
    }
}
