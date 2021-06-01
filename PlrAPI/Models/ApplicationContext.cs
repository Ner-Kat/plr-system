using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlrAPI.Models.Database;
using PlrAPI.Models.Auth;

namespace PlrAPI.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<SocialFormation> SocialFormations { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }

        private IConfiguration _config;

        public ApplicationContext(IConfiguration configuration)
        {
            _config = configuration;
            // Database.EnsureCreated();
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connString = _config.GetConnectionString("PGConnection");
            optionsBuilder.UseNpgsql(connString);
        }
    }
}
