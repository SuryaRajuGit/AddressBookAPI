using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Dto
{
    public class logInResponseDTO
    { 
        public string jwt { get; set; }
        public string type { get; set; }
      
    }
}
