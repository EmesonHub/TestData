using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Models
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
