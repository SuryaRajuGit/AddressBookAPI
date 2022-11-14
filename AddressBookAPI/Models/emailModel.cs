using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Models
{
    
    public  class emailModel
    {
        [Required]
        public string emailAddress { get; set; }

        [Required]
        public typeModel type { get; set; }
 
    }
}
