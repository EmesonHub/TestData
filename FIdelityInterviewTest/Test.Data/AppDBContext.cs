﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Test.Data.Models;

namespace Test.Data
{
    public class AppDBContext: DbContext
    {
        //public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
        //string connectionString = "";

        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            if (!opt.IsConfigured)
            {
                opt.UseMySql(AppDBContext_Setting.ConnectionString, new MySqlServerVersion(new Version()));
            }
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}
