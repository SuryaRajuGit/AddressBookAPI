using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Models
{
    public class Login
    {
        ///<summary>
        /// Id of the Login 
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// user name of the login user 
        ///</summary>
        public string UserName { get; set; }

        ///<summary>
        /// password of the user
        ///</summary>
        public string Password { get; set; }
    }
}





