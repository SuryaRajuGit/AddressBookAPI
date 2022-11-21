using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Models
{
    
    public  class EmailDTO
    {
        [Required]
        [EmailAddress(ErrorMessage ="Enter Valid Email Address")]
        public string emailAddress { get; set; }

        [Required]
        public TypeDTO type { get; set; }
 
    }
}
