using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AddressBookAPI.Models
{
    public class phoneDTO
    {
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\\(?(\[0-9\]{3})\\)?\[-.●\]?(\[0-9\]{3})\[-.●\]?(\[0-9\]{4})$", ErrorMessage = "The PhoneNumber field is not a valid phone number")]
        [Display(Name = "Mobile Number:")]
        [Required]
        [MinLength(10)]
        [MaxLength(10)]
        public string phoneNumber { get; set; }

        [Required]
        public typeDTO type { get; set; }
    }
}
