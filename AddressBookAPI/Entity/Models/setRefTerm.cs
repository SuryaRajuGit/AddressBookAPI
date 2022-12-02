using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Models
{
    public class SetRefTerm
    {
        [Key]
        public Guid Id { get; set; }

        ///<summary>
        /// refset Id 
        ///</summary>
        public Guid RefSetId { get; set; }
        public RefSet RefSet { get; set; }

        ///<summary>
        /// refterm id 
        ///</summary>
        public Guid RefTermId { get; set; }
        public RefTerm RefTerm { get; set; }
    }
}
