using Imkery.Entities;
using Microsoft.EntityFrameworkCore;
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

        public DbSet<Location> Locations { get; set; }
        public DbSet<Hive> Hives { get; set; }
        public DbSet<TagDefinition> TagDefinitions { get; set; }
        public DbSet<ActionDefinition> ActionDefinitions { get; set; }
    }
}