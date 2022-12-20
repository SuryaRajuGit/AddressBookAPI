using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Dto
{
    public class Type
    {
        [JsonProperty(PropertyName = "key")]
        public Guid Key { get; set; }
    }
}
