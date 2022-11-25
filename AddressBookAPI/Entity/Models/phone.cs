using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Models
{
    public class phone
    {
        public int Id { get; set; }

        public string phone_number { get; set; }

        public Guid userId { get; set; }
        public user user { get; set; }

        public Guid refTermId { get; set; }
        public refTerm refTerm { get; set; }
    }
}
