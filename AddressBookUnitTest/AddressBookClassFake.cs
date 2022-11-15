using AddressBookAPI.Data;
using AddressBookAPI.Models;
using AddressBookAPI.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookUnitTest
{
    class AddressBookClassFake //: IAddressBookServices
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


        public Task<Tuple<string, string>> AddNewAddressBook(userDTO UserModel)
        {
            throw new NotImplementedException();
        }

        public Task<Guid?> DeletAddressBook(Guid id)
        {
            throw new NotImplementedException();
        }

        public byte[] Download(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<userDTO> GetAddressBook(Guid id)
        {
            throw new NotImplementedException();
        }

        public int GetAddressBookCount()
        {
            return 2;

        }

        public Task<List<userDTO>> GetAllAddressBooks(int size, string pageNo, string sortBy, string sortOrder)
        {
            throw new NotImplementedException();
        }

        public int Seed()
        {
            throw new NotImplementedException();
        }

        public int SignupAdmin(signupDTO signupModel)
        {
            throw new NotImplementedException();
        }

        public Task<Guid?> UpdateAddressBook(Guid id, userDTO userModel)
        {
            throw new NotImplementedException();
        }

        public Task<UploadResponseDTO> UploadFile(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public Task<logInResponseDTO> VerifyUser(logInDTO logInModel)
        {
            throw new NotImplementedException();
        }
    }
}
