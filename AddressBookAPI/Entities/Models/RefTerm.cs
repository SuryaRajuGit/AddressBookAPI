using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Models
{
    public class RefTerm
    {
        [Key]
        ///<summary>
        /// Id of the Refterm 
        ///</summary>
        public Guid Id { get; set; }

        ///<summary>
        /// reterm key 
        ///</summary>
        public string Key { get; set; }

        ///<summary>
        /// description of the key 
        ///</summary>
        public string Description { get; set; }


    }
}
