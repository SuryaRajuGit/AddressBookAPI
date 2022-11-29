using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Models
{   

    public class email
    {
    
        public int Id { get; set; }

        public string emailAddress { get; set; }
       
        public Guid userId { get; set; }
        public user user { get; set; }
   
        public Guid refTermId { get; set; }
        public refTerm refTerm { get; set; }

    }

   
}
