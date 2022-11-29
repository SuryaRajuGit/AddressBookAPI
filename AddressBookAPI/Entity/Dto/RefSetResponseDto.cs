using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Dto
{
    public class RefSetResponseDto
    {
        public Guid id { get; set; } 

        public string key { get; set; }

        public string description { get; set; }

    }
}
