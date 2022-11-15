using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Models
{
    
    public  class emailDTO
    {
        [Required]
        public string emailAddress { get; set; }

        [Required]
        public typeDTO type { get; set; }
 
    }
}
