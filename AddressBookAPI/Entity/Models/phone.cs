using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Models
{
    public class Phone
    {
        public int Id { get; set; }

        ///<summary>
        /// phone number of the user
        ///</summary>
        public string PhoneNumber { get; set; }
    
        public Guid UserId { get; set; }
        public User User { get; set; }

        ///<summary>
        /// type of phone number  
        ///</summary>
        public Guid RefTermId { get; set; }
        public RefTerm RefTerm { get; set; }
    }
}
