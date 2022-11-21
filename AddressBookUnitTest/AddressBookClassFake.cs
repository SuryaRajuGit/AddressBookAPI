//using AddressBookAPI.Data;
//using AddressBookAPI.Helpers;
//using AddressBookAPI.Models;
//using AddressBookAPI.Repository;
//using AddressBookAPI.Services;
//using AutoMapper;
//using AutoMapper.Configuration;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;

//namespace AddressBookUnitTest
//{
//    public class AddressBookClassFake : IAddressBookRepository
//    {
//        private readonly InjectionFixture _injection;
//        private readonly IConfigurationProvider _configuration;
//        private readonly AddressBookContext _context;
//        private readonly IMapper _mapper;
//        public AddressBookClassFake( AddressBookContext context)
           
//        {
//           // _injection = injection;
//            _context = context;
//            adddata();
            
//        }
//        public void adddata() 
//        {
//            List<user> list = new List<user>
//                {
//            new user()
//            {
//                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
//                firstName = "abc",
//                lastName = "xyz",

//                email = new List<email>() {
//                new email
//                   {
//                       emailAddress = "abc@gmail.com",
//                       userId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
//                       refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
//                   }, },
//                address = new List<address>() {
//                new address
//                {
//                    line1 = "1st",
//                    line2 = "line2",
//                    country = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
//                    userId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
//                    refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
//                    stateName = "stateName",
//                    city = "city",
//                    zipCode ="zipCode"
//                },},

//                phone =new List<phone> {
//                new phone
//                {
//                    refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
//                    userId =Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9") ,
//                    phoneNumber = "8152233879"
//                },new phone
//                {
//                    refTermId = Guid.Parse("F87B8232-F2D8-4286-AC13-422AA54194CE"),
//                    userId =Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9") ,
//                    phoneNumber = "8122233879"
//                } }

//        },new user()
//            {
//                Id = Guid.Parse("E1E91040-8C5E-4748-A625-2E493C7818D9"),
//                firstName = "def",
//                lastName = "jkh",

//                email = new List<email>() {
//                new email
//                   {
//                       emailAddress = "xyz@gmail.com",
//                       userId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
//                       refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
//                   }, },
//                //new email
//                //   {
//                //       emailAddress = "abc@gmail.com",
//                //       userId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
//                //       refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
//                //   } },

//                address = new List<address>() {
//                new address
//                {
//                    line1 = "1st",
//                    line2 = "line2",
//                    country = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
//                    userId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
//                    refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
//                    stateName = "stateName",
//                    city = "city",
//                    zipCode ="zipCode"
//                },},

//                phone =new List<phone> {
//                new phone
//                {
//                    refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
//                    userId =Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9") ,
//                    phoneNumber = "9152233879"
//                },new phone
//                {
//                    refTermId = Guid.Parse("F87B8232-F2D8-4286-AC13-422AA54194CE"),
//                    userId =Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9") ,
//                    phoneNumber = "0122233879"
//                } }

//        }, };



         
//        List<asset> assetList = new List<asset>() {
//            new asset {Id=Guid.Parse("D1E910408C5E4748A6252E493C7818D9") } };

//            var y = _context.User;
//            _context.User.AddRange(list);
//            _context.AssetDTO.AddRange(assetList);
//            var x = _context.User;
//            _context.SaveChanges();

//        }
//        List<string> liss = new List<string>{
//            "surya","abcde"
//            };
//        public List<string> AdderessList()
//        {
//            throw new NotImplementedException();
//        }

//        public string AdderessList(ICollection<AddressDTO> address)
//        {
//            foreach (var item in address)
//            {
//                string josnObjj = JsonConvert.SerializeObject(item);
//                foreach (var itemm in _context.Address)
//                {
//                    var addressObj = new AddressDTO
//                    {
//                        line1 = itemm.line1,
//                        line2 = itemm.line2,
//                        city = itemm.city,
//                        zipCode = itemm.zipCode,
//                        stateName = itemm.stateName,
//                        type = new TypeDTO { key = itemm.refTermId },
//                        country = new TypeDTO { key = itemm.refTermId },
//                    };
//                    string josnObj = JsonConvert.SerializeObject(addressObj);
//                    if (josnObj == josnObjj)
//                    {
//                        return josnObj;
//                    }
//                }
//            }
//            return null;
//        }

//        public List<string> EmailList()
//        {
//            throw new NotImplementedException();
//        }

//        public string EmailList(ICollection<EmailDTO> email)
//        {
//            foreach (var item in email.Select(s => s.emailAddress))
//            {
//                var x = _context.Email.Where(w => w.emailAddress == item).FirstOrDefault();
//                if (x != null)
//                {
//                    return item;
//                }
//            }
//            return null;
//        }

//        public user GetAccountCount(Guid id)
//        {
//            throw new NotImplementedException();
//        }

//        public user GetAddressBook(Guid id)
//        {
//         //   adddata();
//              var r = _context.User.Where(w => w.Id == id).FirstOrDefault();
//            return r;
//        }

//        public int GetAddressBookCount()
//        {
          
//            var x =   _context.User.Count();
//            return x;

//            //var x = list.Count();
//            //return x;
//        }

//        public byte[] GetFile(Guid id)
//        {
            
//            var sourceImg = File.OpenRead(@"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entity\Migrations\SeedingDataFromCSV\Book1.csv");
//             var memoryStream = new MemoryStream();
//            sourceImg.CopyToAsync(memoryStream);
//            var bytearr = memoryStream.ToArray();
//            var isthere =  _context.AssetDTO.Where(w => w.Id == id).FirstOrDefault();
//            return isthere != null ? memoryStream.ToArray() : null;
//        }

//        public bool isUserNameExists(string text)
//        {
//            var t = liss.Contains(text);
//            return t;
//        }

//        public IEnumerable<user> ListOfAccounts(string sortBy)
//        {
//            var x = _context.User;
//            if (sortBy == "")
//            {
//                return null;
//            }
//            return x;
//        }

//        public string loginDetails(string userName)
//        {
//            byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
//            byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

//            SymmetricAlgorithm algorithm = DES.Create();
//            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
//            byte[] inputbuffer = Encoding.Unicode.GetBytes("surya123");
//            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
//            string password = Convert.ToBase64String(outputBuffer);

//            return userName == "surya1" ? password : null;
//        }

//        public List<user> Pagenation(int size, int pageNo)
//        {
//            if(size == 1)
//            {
//                return null;
//            }
//           var x =  _context.User.Skip((pageNo - 1) * 1)
//              .Take(size)
//              .ToList();
                
//            return x;
//        }

//        public List<string> PhoneList()
//        {
//            throw new NotImplementedException();
//        }

//        public string PhoneList(ICollection<PhoneDTO> phone)
//        {
//            foreach (var item in phone.Select(s => s.phoneNumber))
//            {
//                var x = _context.Phone.Where(w => w.phoneNumber == item).FirstOrDefault();
//                if (x != null)
//                {
//                    return item;
//                }
//            }
//            return null;
//        }

//        public void RemoveAccount(user account)
//        {
//            throw new NotImplementedException();
//        }

//        public void SaveFileToDataBase(asset fileObj)
//        {
            
//        }

//        public void SaveToDataBase(user account)
//        {
//            _context.User.Add(account);
//            _context.SaveChanges();
//        }

//        public int  SinupAdmin(login sinupModel)
//        {
//            return 2;
//        }

//        public void UpdateToDataBase(user account)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
