using Newtonsoft.Json;
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
        [JsonProperty(PropertyName = "user_name")]
        public string userName { get; set; }

        [Required]
        public string password { get; set; }
    }
}
