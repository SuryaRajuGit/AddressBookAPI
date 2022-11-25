using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Models
{
    public class setRefTerm
    {
        [Key]
        public Guid Id { get; set; }

        public Guid refSetId { get; set; }
        public refSet refSet { get; set; }


        public Guid refTermId { get; set; }
        public refTerm refTerm { get; set; }
    }
}
