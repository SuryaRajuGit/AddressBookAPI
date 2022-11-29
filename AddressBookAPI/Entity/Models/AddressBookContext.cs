
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
                string[] row = item.Split(",");
                refTerm refObj = new refTerm { Id = Guid.Parse(row[0]), key = row[1].ToString(), description = row[2].ToString() };
                list.Add(refObj);
            }
            modelBuilder.Entity<refTerm>()
            .HasData(list);

            string path1 = @"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entity\Migrations\TextFile.csv";
            string ReadCsv = File.ReadAllText(path1);
            string[] data1 = ReadCsv.Split('\r');
            Guid id = Guid.NewGuid();
       
            modelBuilder.Entity<phone>()
                  .HasOne(e => e.user)
                  .WithMany(s => s.phone)
                  .HasForeignKey(s => s.userId);
            int count = 0;
            foreach (var item in data1)
            {
                string[] row = item.Split(",");
                id = Guid.NewGuid();
                user user1 = new user()
                {
                    Id = id,
                    firstName = row[0].ToString(),
                    lastName = row[1].ToString()
                };

                phone phone1 = new phone()
                {
                    Id = count + 1,
                    phoneNumber = row[11].ToString(),
                    userId = id,
                    refTermId = Guid.Parse(row[12])
                };
                email email1 = new email()
                {
                    Id = count + 1,
                    emailAddress = row[9].ToString(),
                    refTermId = Guid.Parse(row[10]),
                    userId = id,

                };
                address address1 = new address()
                {
                    Id = count + 1,
                    line1 = row[2].ToString(),
                    line2 = row[3].ToString(),
                    city = row[4].ToString(),
                    zipCode = row[5].ToString(),
                    stateName = row[6].ToString(),
                    country = Guid.Parse(row[7]),
                    refTermId = Guid.Parse(row[8]),
                    userId = id,

                };
                modelBuilder.Entity<user>().HasData(user1);
                modelBuilder.Entity<phone>().HasData(phone1);
                modelBuilder.Entity<email>().HasData(email1);
                modelBuilder.Entity<address>().HasData(address1);
            }


            string path2 = @"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entity\Migrations\refSet.csv";
            string ReadCsvRefSet = File.ReadAllText(path2);
            string[] dataRefSet = ReadCsvRefSet.Split('\r');

            foreach (var item in dataRefSet)
            {
                string[] row = item.Split(",");

                refSet refSet1 = new refSet()
                {
                    Id = Guid.Parse(row[0]),
                    key = row[1],
                    description = row[2]
                };

                modelBuilder.Entity<refSet>().HasData(refSet1);
            }

            string path3 = @"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entity\Migrations\refSetTerm.csv";
            string ReadCsvRefSetTerm = File.ReadAllText(path3);
            string[] dataRefSetTerm = ReadCsvRefSetTerm.Split('\r');

            List<setRefTerm> ll = new List<setRefTerm>();
            foreach (var item in dataRefSetTerm)
            {
                string[] row = item.Split(",");

                setRefTerm setRefTerm1 = new setRefTerm()
                {
                    Id = Guid.NewGuid(),
                    refSetId = Guid.Parse(row[0]),
                    refTermId = Guid.Parse(row[1]),
                };
                ll.Add(setRefTerm1);

            }
            modelBuilder.Entity<setRefTerm>().HasData(ll);
        }


    }

}
