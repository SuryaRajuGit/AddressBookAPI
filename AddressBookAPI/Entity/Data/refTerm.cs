using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Data
{
    public class refTerm
    {
        [Key]
        public Guid Id { get; set; }

        public string key { get; set; }

        public string description { get; set; }


    }
}
