using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Data
{
    public class setRefTerm
    {
        public Guid Id { get; set; }

        [ForeignKey("RefSetId")]
        public Guid RefSetId { get; set; }
        public refSet refSet;

        [ForeignKey("RefTermId")]
        public Guid RefTermId { get; set; }
        public refTerm refTerm;
    }
}
