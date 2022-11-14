using AddressBookAPI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Models
{
    
    public  class userModel 
    {
        public Guid Id { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        public ICollection<addressModel> Address { get; set; }

        public ICollection<assetDtoModel> AssetDTO { get; set; }

        public ICollection<emailModel> Email { get; set; }

        public ICollection<phoneModel> Phone { get; set; }

        

    }
}
