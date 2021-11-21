using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlrAPI.Models.Database;
using PlrAPI.Models.Auth;
using PlrAPI.Systems;

namespace PlrAPI.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<SocialFormation> SocialFormations { get; set; }
        public DbSet<AdditionalFieldType> AdditionalFieldTypes { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharAdditionalValue> CharAdditionalValues { get; set; }
        public DbSet<User> Users { get; set; }

        private IConfiguration _config;

        public ApplicationContext(IConfiguration configuration)
        {
            _config = configuration;
            // Database.EnsureDeleted();
            // Database.EnsureCreated();
            // Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connString = _config.GetConnectionString("PGConnection");
            optionsBuilder.UseNpgsql(connString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdditionalFieldType>().HasIndex(af => af.Name).IsUnique();  // Имена дополнительных полей уникальны
            
            // Пара "доп. поле - пользователь" уникальна
            modelBuilder.Entity<CharAdditionalValue>().HasIndex(
                cav => new { cav.AdditionalFieldTypeId, cav.CharacterId }
                ).IsUnique();  

            byte[] adminSalt = PasswordsUtils.CreateSalt();
            string adminPassword = PasswordsUtils.CreateHashedPass("admin", adminSalt);
            modelBuilder.Entity<User>().HasData(
                new User[]
                {
                    new User { Id=1, Login="admin", Password=adminPassword, Salt=Convert.ToBase64String(adminSalt), Role=Roles.SuperAdmin }
                });
        }
    }
}
