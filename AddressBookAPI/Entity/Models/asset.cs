using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Models
{
    public class Asset
    {
       public Guid Id { get; set; }

        ///<summary>
        /// binary data of the profile pic of user
        ///</summary>
        public byte[] Field { get; set; }
    }
}
