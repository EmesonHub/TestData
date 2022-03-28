using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Data.DTO.Out
{
    public class Account_Users
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Website { get; set; }
        public object AssociatedUsers { get; set; }
    }
}
