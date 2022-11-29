
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Dto
{

    public class EmailDTO
    {
        [Required]
        [JsonProperty(PropertyName = "email_address")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Enter Valid email address")]
        public string emailAddress { get; set; }

        [Required]
        public TypeDTO type { get; set; }
 
    }
}
