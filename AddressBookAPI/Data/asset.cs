using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Data
{
    public class asset
    {
       public Guid Id { get; set; }
        
       public byte[] field { get; set; }
    }
}
