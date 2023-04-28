﻿using FCBankBasicHelper.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class CustomerModel
    {
        public int Id { get; set; }
        [Property]
        public string FirstName { get; set; } = null!;
        [Property]

        public string LastName { get; set; } = null!;

        public DateTime Birthday { get; set; }
        [Property]

        public string Email { get; set; } = null!;
        [Property]

        public string Passport { get; set; } = null!;
        [Property]

        public string Address { get; set; } = null!;

        public string Phone { get; set; } = null!;
        [Timestamp]
        [ConcurrencyCheck]
        public byte[] Version { get; set; } = null!;
    }
}
