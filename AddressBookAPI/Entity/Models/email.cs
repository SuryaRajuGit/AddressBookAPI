using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Models
{   

    public class Email
    {
    
        public int Id { get; set; }

        ///<summary>
        /// Email address of the user  
        ///</summary>
        public string EmailAddress { get; set; }
       
        public Guid UserId { get; set; }
        public User user { get; set; }

        ///<summary>
        /// type of email address 
        ///</summary>
        public Guid RefTermId { get; set; }
        public RefTerm refTerm { get; set; }

    }

   
}
