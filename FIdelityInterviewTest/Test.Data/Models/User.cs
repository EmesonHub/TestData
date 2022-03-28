using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string LastName { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string Email { get; set; }
    }
}
