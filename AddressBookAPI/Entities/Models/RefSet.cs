using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Models
{
    public class RefSet
    {
        ///<summary>
        /// Id of the Refset 
        ///</summary>
        public Guid Id { get; set; }

        ///<summary>
        /// refset key
        ///</summary>
        public string Key { get; set; }

        ///<summary>
        /// description of the refset key 
        ///</summary>
        public string Description { get; set; }
    }
}
