using bundleSVC.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace bundleSVC.Data
{
    public class BundleContext : DbContext
    {

        public DbSet<Bundle> Bundles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=bundles.db;");
            base.OnConfiguring(optionsBuilder);
        }

        public BundleContext(DbContextOptions<BundleContext> opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Used only for creating the initial migrations and database file.

            //modelBuilder.Entity<Bundle>().ToTable("Bundles");
            //modelBuilder.Entity<Bundle>().HasData(
            //new Bundle() { B_code = 1, B_name = "test1", B_price = 7.5, B_expdate = new DateTime(2020, 12, 30), B_availdate = new DateTime(2020, 12, 1), B_active = true },
            //new Bundle() { B_code = 2, B_name = "test2", B_price = 12.0, B_expdate = new DateTime(2020, 12, 5), B_availdate = new DateTime(2020, 11, 1), B_active = true },
            //new Bundle() { B_code = 3, B_name = "test3", B_price = 11.0, B_expdate = new DateTime(2021, 1, 8), B_availdate = new DateTime(2020, 12, 20), B_active = true },
            //new Bundle() { B_code = 4, B_name = "test4", B_price = 13.0, B_expdate = new DateTime(2021, 1, 15), B_availdate = new DateTime(2020, 12, 25), B_active = true },
            //new Bundle() { B_code = 5, B_name = "test5", B_price = 5.0, B_expdate = new DateTime(2021, 2, 14), B_availdate = new DateTime(2021, 2, 1), B_active = true },
            //new Bundle() { B_code = 6, B_name = "test6", B_price = 8.0, B_expdate = new DateTime(2021, 2, 1), B_availdate = new DateTime(2021, 3, 1), B_active = true }
            //);
            //base.OnModelCreating(modelBuilder);
        }
    }
}
