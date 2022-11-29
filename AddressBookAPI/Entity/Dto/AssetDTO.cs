using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Dto
{
    public class AssetDTO
    {
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "file_name")]
        public string fileName { get; set; }

        public byte[] file { get; set; }

        public Guid userId { get; set; }
        
    }
}
