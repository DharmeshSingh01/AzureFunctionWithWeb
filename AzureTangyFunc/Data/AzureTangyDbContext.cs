﻿using AzureTangyFunc.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTangyFunc.Data
{
    public class AzureTangyDbContext : DbContext
    {
        public AzureTangyDbContext(DbContextOptions<AzureTangyDbContext> options) : base(options)
        {

        }
        public DbSet<SalesRequest> SalesRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SalesRequest>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
