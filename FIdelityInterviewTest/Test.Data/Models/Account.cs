using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Data.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string CompanyName { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string Website { get; set; }
    }
}
