using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test
{
    public class AppDBContext: DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }



        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}
