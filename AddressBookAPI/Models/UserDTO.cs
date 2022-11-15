using AddressBookAPI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Models
{
    
    public  class userDTO 
    {
        public Guid Id { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        public ICollection<addressDTO> Address { get; set; }

        public ICollection<assetDTO> AssetDTO { get; set; }

        public ICollection<emailDTO> Email { get; set; }

        public ICollection<phoneDTO> Phone { get; set; }

        

    }
}
