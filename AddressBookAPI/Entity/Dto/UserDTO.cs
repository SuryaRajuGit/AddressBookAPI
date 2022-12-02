
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace AddressBookAPI.Entity.Dto
{
    
    public  class UserDTO 
    {
        public Guid Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

       
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [Required]
        public ICollection<AddressDTO> Address { get; set; }

        public ICollection<AssetDTO> AssetDTO { get; set; }
        
        [Required]
        public ICollection<EmailDTO> Email { get; set; }

        [Required]
        public ICollection<PhoneDTO> Phone { get; set; }

        

    }
}
