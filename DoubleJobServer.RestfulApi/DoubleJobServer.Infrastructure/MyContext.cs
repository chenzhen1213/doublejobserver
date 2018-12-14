using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using DoubleJobServer.Core.DomainModels;

namespace DoubleJobServer.Infrastructure
{
    public class MyContext : DbContext
    {

        public MyContext(DbContextOptions<MyContext> options)
         : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Country> Countries { get; set; }

        public DbSet<City> Cities { get; set; }
    }

}
