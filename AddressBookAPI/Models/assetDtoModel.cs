using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Models
{
    public class assetDtoModel
    {
        public Guid Id { get; set; }

        public string fileName { get; set; }

        public byte[] file { get; set; }

        public Guid userId { get; set; }
        
    }
}
