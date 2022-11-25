
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Dto
{
    
    public  class UserDTO 
    {
        public Guid Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string first_name { get; set; }

       
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string last_name { get; set; }

        [Required]
        public ICollection<AddressDTO> Address { get; set; }

        public ICollection<AssetDTO> AssetDTO { get; set; }
        
        [Required]
        public ICollection<EmailDTO> Email { get; set; }

        [Required]
        public ICollection<PhoneDTO> Phone { get; set; }

        

    }
}
