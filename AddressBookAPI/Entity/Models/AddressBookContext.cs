
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

        public DbSet<Address> Address { get; set; }

        public DbSet<Email> Email { get; set; }

        public DbSet<Phone> Phone { get; set; }

        public DbSet<RefSet> RefSet { get; set; }

        public DbSet<RefTerm> RefTerm { get; set; }

        public DbSet<SetRefTerm> SetRefTerm { get; set; }

        public DbSet<Asset> AssetDTO { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Login> Login { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string path = @"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entity\Migrations\Book1.csv";
            string ReadCSV = File.ReadAllText(path);
            var data = ReadCSV.Split('\r');
            var list = new List<RefTerm>();
            foreach (var item in data)
            {
                string[] row = item.Split(",");
                RefTerm refObj = new RefTerm { Id = Guid.Parse(row[0]), Key = row[1].ToString(), Description = row[2].ToString() };
                list.Add(refObj);
            }
            modelBuilder.Entity<RefTerm>()
            .HasData(list);

            string path1 = @"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entity\Migrations\TextFile.csv";
            string ReadCsv = File.ReadAllText(path1);
            string[] data1 = ReadCsv.Split('\r');
            Guid id = Guid.NewGuid();
       
            modelBuilder.Entity<Phone>()
                  .HasOne(e => e.User)
                  .WithMany(s => s.Phone)
                  .HasForeignKey(s => s.UserId);
            int count = 0;
            foreach (var item in data1)
            {
                string[] row = item.Split(",");
                id = Guid.NewGuid();
                User user1 = new User()
                {
                    Id = id,
                    FirstName = row[0].ToString(),
                    LastName = row[1].ToString()
                };

                Phone phone1 = new Phone()
                {
                    Id = count + 1,
                    PhoneNumber = row[11].ToString(),
                    UserId = id,
                    RefTermId = Guid.Parse(row[12])
                };
                Email email1 = new Email()
                {
                    Id = count + 1,
                    EmailAddress = row[9].ToString(),
                    RefTermId = Guid.Parse(row[10]),
                    UserId = id,

                };
                Address address1 = new Address()
                {
                    Id = count + 1,
                    Line1 = row[2].ToString(),
                    Line2 = row[3].ToString(),
                    City = row[4].ToString(),
                    Zipcode = row[5].ToString(),
                    StateName = row[6].ToString(),
                    Country = Guid.Parse(row[7]),
                    RefTermId = Guid.Parse(row[8]),
                    UserId = id,

                };
                modelBuilder.Entity<User>().HasData(user1);
                modelBuilder.Entity<Phone>().HasData(phone1);
                modelBuilder.Entity<Email>().HasData(email1);
                modelBuilder.Entity<Address>().HasData(address1);
            }


            string path2 = @"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entity\Migrations\refSet.csv";
            string ReadCsvRefSet = File.ReadAllText(path2);
            string[] dataRefSet = ReadCsvRefSet.Split('\r');

            foreach (var item in dataRefSet)
            {
                string[] row = item.Split(",");

                RefSet refSet1 = new RefSet()
                {
                    Id = Guid.Parse(row[0]),
                    Key = row[1],
                    Description = row[2]
                };

                modelBuilder.Entity<RefSet>().HasData(refSet1);
            }

            string path3 = @"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entity\Migrations\refSetTerm.csv";
            string ReadCsvRefSetTerm = File.ReadAllText(path3);
            string[] dataRefSetTerm = ReadCsvRefSetTerm.Split('\r');

            List<SetRefTerm> ll = new List<SetRefTerm>();
            foreach (var item in dataRefSetTerm)
            {
                string[] row = item.Split(",");

                SetRefTerm setRefTerm1 = new SetRefTerm()
                {
                    Id = Guid.NewGuid(),
                    RefSetId = Guid.Parse(row[0]),
                    RefTermId = Guid.Parse(row[1]),
                };
                ll.Add(setRefTerm1);

            }
            modelBuilder.Entity<SetRefTerm>().HasData(ll);
        }


    }

}
