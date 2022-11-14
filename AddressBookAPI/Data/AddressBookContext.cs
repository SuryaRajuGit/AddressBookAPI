using AddressBookAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Data
{
    public class AddressBookContext : IdentityDbContext<ApplicationUser>
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

        public DbSet<Login> Login { get; set; }
    }
}
