﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Models
{
    public class ErrorDTO
    {
        public string type { get; set; }

        public string description { get; set; }
    }
}