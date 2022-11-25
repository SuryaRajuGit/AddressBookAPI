using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Models
{
    public class address
    {
        public int Id { get; set; }

        public string line1 { get; set; }

        public string line2 { get; set; }

        public string city { get; set; }

        public string zipCode { get; set; }

        public string state_name { get; set; }

        public Guid country { get; set; }

        public Guid refTermId { get; set; }
        public refTerm refTerm { get; set; }

        public Guid userId { get; set; }
        public user user { get; set; }
    }
}
