using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AddressBookAPI.Entity.Dto
{
    public class PhoneDTO
    {
        //[DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "The PhoneNumber field is not a valid phone number")]
        [Required]
        public string phone_number { get; set; }

        [Required]
        public TypeDTO type { get; set; }
    }
}
