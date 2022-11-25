
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Entity.Models
{
    public class AddressBookContext : DbContext
    {
        public AddressBookContext()
        {
        }

        public AddressBookContext(DbContextOptions<AddressBookContext> options) : base(options)
        {

        }

        public DbSet<address> Address { get; set; }

        public DbSet<email> Email { get; set; }

        public DbSet<phone> Phone { get; set; }

        public DbSet<refSet> RefSet { get; set; }

        public DbSet<refTerm> RefTerm { get; set; }

        public DbSet<setRefTerm> SetRefTerm { get; set; }

        public DbSet<asset> AssetDTO { get; set; }

        public DbSet<user> User { get; set; }

        public DbSet<login> Login { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string path = @"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entity\Migrations\Book1.csv";
            string ReadCSV = File.ReadAllText(path);
            var data = ReadCSV.Split('\r');
            var list = new List<refTerm>();
            foreach (var item in data)
            {
                var row = item.Split(",");
                var refObj = new refTerm { Id = Guid.Parse(row[0].ToString()), key = row[1].ToString(), description = row[2].ToString() };
                list.Add(refObj);
            }
            modelBuilder.Entity<refTerm>()
            .HasData(list);
        }



    }

}
