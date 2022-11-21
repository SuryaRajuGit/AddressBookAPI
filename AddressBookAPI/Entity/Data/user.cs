using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Data
{
    public class user
    {
        public Guid Id { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public ICollection<address> address { get; set; }

        public ICollection<asset> assetdto { get; set; }

        public ICollection<email> email { get; set; }

        public ICollection<phone> phone { get; set; }
    }
}
