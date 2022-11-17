using AddressBookAPI.Data;
using AddressBookAPI.Models;
using AddressBookAPI.Repository;
using AddressBookAPI.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookUnitTest
{
    class AddressBookClassFake : IAddressBookRepository
    {
        List<user> list = new List<user>()
        {
            new user()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                firstName = "abc",
                lastName = "xyz",

                email = new List<email>() {
                new email
                   {
                       emailAddress = "abc@gmail.com",
                       userId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                       refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                   }, },
                //new email
                //   {
                //       emailAddress = "abc@gmail.com",
                //       userId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                //       refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                //   } },

                address = new List<address>() {
                new address
                {
                    line1 = "1st",
                    line2 = "line2",
                    country = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                    userId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                    refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                    stateName = "stateName",
                    city = "city",
                    zipCode ="zipCode"
                },},

                phone =new List<phone> {
                new phone
                {
                    refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                    userId =Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9") ,
                    phoneNumber = "8152233879"
                },new phone
                {
                    refTermId = Guid.Parse("F87B8232-F2D8-4286-AC13-422AA54194CE"),
                    userId =Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9") ,
                    phoneNumber = "8122233879"
                } }

        },new user()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                firstName = "def",
                lastName = "jkh",

                email = new List<email>() {
                new email
                   {
                       emailAddress = "abc@gmail.com",
                       userId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                       refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                   }, },
                //new email
                //   {
                //       emailAddress = "abc@gmail.com",
                //       userId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                //       refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                //   } },

                address = new List<address>() {
                new address
                {
                    line1 = "1st",
                    line2 = "line2",
                    country = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                    userId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                    refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                    stateName = "stateName",
                    city = "city",
                    zipCode ="zipCode"
                },},

                phone =new List<phone> {
                new phone
                {
                    refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                    userId =Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9") ,
                    phoneNumber = "9152233879"
                },new phone
                {
                    refTermId = Guid.Parse("F87B8232-F2D8-4286-AC13-422AA54194CE"),
                    userId =Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9") ,
                    phoneNumber = "0122233879"
                } }

        }, };

        public List<string> AdderessList()
        {
            throw new NotImplementedException();
        }

        public List<string> EmailList()
        {
            throw new NotImplementedException();
        }

        public user GetAccountCount(Guid id)
        {
            throw new NotImplementedException();
        }

        public async  Task<user> GetAddressBook(Guid id)
        {
            var r = list.Where(w => w.Id == id).FirstOrDefault();
            return r;
        }

        public int GetAddressBookCount()
        {
            var x = list.Count();
            return x;
        }

        public byte[] GetFile(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool isUserNameExists(string text)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<user> ListOfAccounts(string sortBy)
        {
            var x = list;
            return x;
        }

        public string loginDetails(string userName)
        {
            throw new NotImplementedException();
        }

        public List<string> PhoneList()
        {
            throw new NotImplementedException();
        }

        public void RemoveAccount(user account)
        {
            throw new NotImplementedException();
        }

        public void SaveFileToDataBase(asset fileObj)
        {
            throw new NotImplementedException();
        }

        public void SaveToDataBase(user account)
        {
            throw new NotImplementedException();
        }

        public int SinupAdmin(Login sinupModel)
        {
            throw new NotImplementedException();
        }

        public void UpdateToDataBase(user account)
        {
            throw new NotImplementedException();
        }
    }
}
