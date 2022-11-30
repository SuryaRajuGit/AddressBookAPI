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
        //[DataType(DataType.PhoneNumber)]
     //   [RegularExpression(@"^(?:(?:\+|0{0,2})91(\s*[\-]\s*)?|[0]?)?[789]\d{9}$", ErrorMessage = "The PhoneNumber field is not a valid phone number")]
        [Phone(ErrorMessage = "The PhoneNumber field is not a valid phone number")]
        [Required]
        [MinLength(10)]
        [JsonProperty(PropertyName = "phone_number")]
        public string phoneNumber { get; set; }

        [Required]
        public TypeDTO type { get; set; }
    }
}
