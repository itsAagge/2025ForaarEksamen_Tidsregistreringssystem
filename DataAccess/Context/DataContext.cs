using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    internal class DataContext : DbContext
    {
        public DataContext()
        {
            bool created = Database.EnsureCreated();
            if (created)
            {
                Debug.WriteLine("Database created");
            }
        }

        public DbSet<Afdeling> Afdelinger { get; set; }
        public DbSet<Medarbejder> Medarbejdere { get; set; }
        public DbSet<Sag> Sager { get; set; }
        public DbSet<Tidsregistrering> Tidsregistreringer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("ConnectionString");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigureAfdelinger());
            modelBuilder.ApplyConfiguration(new ConfigureMedarbejdere());
            modelBuilder.ApplyConfiguration(new ConfigureSager());
            modelBuilder.ApplyConfiguration(new ConfigureTidsregistreringer());
            modelBuilder.ApplyConfiguration(new ConfigureAfdelingMedarbejder());
        }
    }
}
