using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace AddressBookAPI.Entity.Dto
{
    public class PhoneDTO
    {
        
        [Phone(ErrorMessage = "The PhoneNumber field is not a valid phone number")]
        [Required]
        [MinLength(10)]
        [JsonProperty(PropertyName = "phone_number")]
        public string PhoneNumber { get; set; }

        [Required]
        [JsonProperty(PropertyName = "type")]
        public TypeDTO Type { get; set; }
    }
}
