using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Dto
{
    public class LogInDTO
    {
      

        [Required]
        public string user_name { get; set; }

        [Required]
        public string password { get; set; }
    }
}
