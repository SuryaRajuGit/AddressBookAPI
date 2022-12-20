using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Dto
{
    public  class TypeDTO
    {
        [Required]
        [MaxLength(36,ErrorMessage ="Enter Valid Guid")]
        [MinLength(36, ErrorMessage = "Enter Valid Guid")]
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
    }
}
