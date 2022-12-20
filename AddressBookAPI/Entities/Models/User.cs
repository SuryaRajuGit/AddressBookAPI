using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Models
{
    public class User
    {
        ///<summary>
        /// Id of the User 
        ///</summary>
        public Guid Id { get; set; }

        ///<summary>
        /// first name of the user 
        ///</summary>
        public string FirstName { get; set; }

        ///<summary>
        /// last name of the user 
        ///</summary>
        public string LastName { get; set; }

        ///<summary>
        /// address of the user 
        ///</summary>
        public ICollection<Address> Address { get; set; }

        ///<summary>
        /// profile picture of the user 
        ///</summary>
        public ICollection<Asset> Assetdto { get; set; }

        ///<summary>
        /// email address of the user 
        ///</summary>
        public ICollection<Email> Email { get; set; }

        ///<summary>
        /// phone number of the user 
        ///</summary>
        public ICollection<Phone> Phone { get; set; }
    }
}
