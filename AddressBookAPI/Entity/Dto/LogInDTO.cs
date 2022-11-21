using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Models
{
    public class LogInDTO
    {
        public int Id { get; set; }

        [Required]
        public string userName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
