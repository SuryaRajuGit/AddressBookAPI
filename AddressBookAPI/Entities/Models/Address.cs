using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Models
{
    public class Address
    {
        ///<summary>
        /// Id of the address 
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// street name of the user  
        ///</summary>
        public string Line1 { get; set; }

        ///<summary>
        /// 2nd street  of the user 
        ///</summary>
        public string Line2 { get; set; }

        ///<summary>
        /// city  of the user 
        ///</summary>
        public string City { get; set; }

        ///<summary>
        /// first name of the user 
        ///</summary>
        public string Zipcode { get; set; }

        ///<summary>
        /// state name of the user 
        ///</summary>
        public string StateName { get; set; }

        ///<summary>
        /// country  of the user 
        ///</summary>
        public Guid Country { get; set; }

        ///<summary>
        /// address type  the user 
        ///</summary>
        public Guid RefTermId { get; set; }
        public RefTerm RefTerm { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
