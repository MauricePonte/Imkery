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

        public DbSet<TagDefinition> TagDefinitions { get; set; }
    }
}
