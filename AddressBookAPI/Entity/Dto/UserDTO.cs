using AddressBookAPI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Models
{
    
    public  class UserDTO 
    {
        public Guid Id { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        public ICollection<AddressDTO> Address { get; set; }

        public ICollection<AssetDTO> AssetDTO { get; set; }

        public ICollection<EmailDTO> Email { get; set; }

        public ICollection<PhoneDTO> Phone { get; set; }

        

    }
}
