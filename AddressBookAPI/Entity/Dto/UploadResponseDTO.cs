using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Models
{
    public class UploadResponseDTO
    {
        public Guid Id { get; set; }

        public string fileName {get;set;}

        public string downloadURL { get; set; }

        public string fileType { get; set; }

        public long size { get; set; }

        public string fileContent { get; set; }
    }
}
