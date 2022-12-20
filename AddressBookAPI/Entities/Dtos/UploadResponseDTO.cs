using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Dto
{
    public class UploadResponseDTO
    {
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "file_name")]
        public string fileName {get;set;}

        public string downloadURL { get; set; }

        public string fileType { get; set; }

        public long size { get; set; }

        public string fileContent { get; set; }
    }
}
