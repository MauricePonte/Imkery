using Imkery.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imkery.Data.Storage.Core
{
    public class ImkeryDbContext : DbContext
    {
        public ImkeryDbContext()
        {

        }
        public ImkeryDbContext(DbContextOptions<ImkeryDbContext> options)
             : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Hive>().OwnsMany(b=> b.Tags);
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Hive> Hives { get; set; }
        public DbSet<TagDefinition> TagDefinitions { get; set; }
        public DbSet<ActionDefinition> ActionDefinitions { get; set; }
    }
}