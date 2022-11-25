using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Dto
{
    public class TokensDTO
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
