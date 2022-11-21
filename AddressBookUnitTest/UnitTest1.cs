using AddressBookAPI;
using AddressBookAPI.Controllers;
using AddressBookAPI.Data;
using AddressBookAPI.Helpers;
using AddressBookAPI.Models;
using AddressBookAPI.Repository;
using AddressBookAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Xunit;

namespace AddressBookUnitTest
{
    public class UnitTest1 //: IClassFixture<InjectionFixture>
    {
        private readonly IMapper _mapper;
        private readonly AddressBookController _addresBookController;
        private readonly AddressBookRepository _repository;
        private readonly AddressBookServices _addressBookServices;
        private readonly IConfiguration _configuration;
        private readonly IServer _server;
        private readonly AddressBookContext _context;
        private readonly ILogger _logger;
        public UnitTest1()
        {
            var hostBuilder = Host.CreateDefaultBuilder().
            ConfigureLogging((builderContext, loggingBuilder) =>
            {
                loggingBuilder.AddConsole((options) =>
                {
                    options.IncludeScopes = true;
                });
            });
            var host = hostBuilder.Build();
            var _logger = host.Services.GetRequiredService<ILogger<AddressBookController>>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApplicationMapper());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;

            var options = new DbContextOptionsBuilder<AddressBookContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new AddressBookContext(options);
            addData();
            _context.Database.EnsureCreated();
            _repository = new AddressBookRepository(_context);
            _addressBookServices = new AddressBookServices(_configuration, _repository, _server, _mapper);
            _addresBookController = new AddressBookController(_addressBookServices, _logger);


        }
        public void addData()
        {
            List<user> list = new List<user>
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
                Id = Guid.Parse("E1E91040-8C5E-4748-A625-2E493C7818D9"),
                firstName = "def",
                lastName = "jkh",

                email = new List<email>() {
                new email
                   {
                       emailAddress = "xyz@gmail.com",
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

            var sourceImg = File.OpenRead(@"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entity\Migrations\SeedingDataFromCSV\Book1.csv");
            var memoryStream = new MemoryStream();
            sourceImg.CopyToAsync(memoryStream);
            var bytearr = memoryStream.ToArray();
            var assetdto = new asset { Id = Guid.Parse("D1E910408C5E4748A6252E493C7818D9"), field = bytearr };

            var valdLogin = new login { password = "UnVmvnspqYZtOlgWDkmkKAbuj7qrNOH9", userName = "surya123" };

            _context.Login.Add(valdLogin);
            _context.User.AddRange(list);
            _context.AssetDTO.Add(assetdto);
            _context.SaveChanges();
            _context.AddRange();
        }

        [Fact]
        public async void GetAddressBookCount_Test()
        {
            var response = await _addresBookController.GetAddressBookCount() as OkObjectResult;
            var items = Assert.IsType<int>(response.Value);
            Assert.Equal(2, items);
        }

        [Fact]
        public async void GetAllAddressBooksAccounts_Test()
        {

            var okResult = await _addresBookController.GetAllAddressBooksAccounts(4, 1, "firstName", "ASC");
            var notFoundResult = await _addresBookController.GetAllAddressBooksAccounts(10, 10, "lastName", "ASC");

            Assert.IsType<OkObjectResult>(okResult.Result);

            var list = okResult.Result as OkObjectResult;

            Assert.IsType<List<UserDTO>>(list.Value);

            var listBooks = list.Value as List<UserDTO>;

            Assert.Equal(2, listBooks.Count);
        }

        [Fact]
        public async void GetAddressBook_Test()
        {
            var validId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9");
            var invalidId = Guid.NewGuid();
            var notFoundResult =await _addresBookController.GetAddressBook(invalidId);
            var okResult =await _addresBookController.GetAddressBook(validId);


            var item = okResult.Result as OkObjectResult;
            var bookItem = item.Value as UserDTO;

            Assert.IsType<NotFoundResult>(notFoundResult.Result);
            Assert.IsType<OkObjectResult>(okResult.Result);
            Assert.IsType<UserDTO>(item.Value);
            Assert.Equal(validId, bookItem.Id);
            Assert.Equal("abc", bookItem.firstName);
        }

        [Fact]
        public async void VerifyUser_Test()
        {
            var inValidLogin = new LogInDTO { Password = "jna", userName = "ksnknc" };
            var valdLogin = new LogInDTO { Password = "Surya@123", userName = "surya123" };

            var unAuthorised = await _addresBookController.VerifyUser(inValidLogin);
            var Authorised = await _addresBookController.VerifyUser(valdLogin);

            Assert.IsType<UnauthorizedObjectResult>(unAuthorised);
            Assert.IsType<OkObjectResult>(Authorised);
        }

        [Fact]
        public async void UploadFile_Test()
        {
            var file = new Mock<IFormFile>();
            var sourceImg = File.OpenRead(@"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entity\files\cap2.PNG");
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(sourceImg);
            writer.Flush();
            stream.Position = 0;
            var fileName = "cap2.PNG";
            file.Setup(f => f.OpenReadStream()).Returns(stream);
            file.Setup(f => f.FileName).Returns(fileName);
            file.Setup(f => f.Length).Returns(stream.Length);
            var inputFile = file.Object;

            var okResult =await _addresBookController.UploadFile(inputFile);
            var result = okResult.Result as OkObjectResult;

            Assert.IsType<OkObjectResult>(okResult.Result); 
            Assert.IsType<UploadResponseDTO>(result.Value);
        //    var bookItem = result.Value as UploadResponseDTO;
         //   Assert.Equal(fileName, bookItem.fileName);
        }

        [Fact]
        public async void Download_Test()
        {
            var inValid = Guid.NewGuid();
            var valid = Guid.Parse("D1E910408C5E4748A6252E493C7818D9");

            var notFoundResult = await _addresBookController.Download(inValid);
            var FoundResult = await _addresBookController.Download(valid);

            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<FileContentResult>(FoundResult);
        }

        [Fact]
        public async void SignUpAdmin_Test()
        {
            var notexists = new SignupDTO {userName="suryaNew",password="sur22sgya@1Rs" };
            var exists = new SignupDTO { userName = "surya123", password = "Surya@123" };
         //   var sinup =await  _addresBookController.SignUpAdmin(dto);

            var conflictResponse =await  _addresBookController.SignUpAdmin(exists);
            var okResponse = await _addresBookController.SignUpAdmin(notexists);

            ObjectResult isAlreadyResponse = Assert.IsType<ObjectResult>(conflictResponse);
      
            Assert.Equal(409, isAlreadyResponse.StatusCode);
            Assert.IsType<OkObjectResult>(okResponse);
        }

        [Fact]
        public async void AddNewAddressBook_Test()
        {
            var email = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                firstName = "abc",
                lastName = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        emailAddress = "abc@gmail.com",
                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        line1 = "1st",
                        line2 = "line2",
                        country = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },

                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },
                        stateName = "stateName",
                        city = "city",
                        zipCode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                        type = new TypeDTO{key= Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },
                  
              
                        phoneNumber = "8152233879"
                    }, new PhoneDTO
                    {
                
                        type = new TypeDTO{key= Guid.Parse("F87B8232-F2D8-4286-AC13-422AA54194CE") },
                        phoneNumber = "8122233879"
                    } }
            };
            var phone = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                firstName = "abc",
                lastName = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        emailAddress = "abcabc@gmail.com",
                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        line1 = "1st",
                        line2 = "line2",
                        country = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },

                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },
                        stateName = "stateName",
                        city = "city",
                        zipCode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                        type = new TypeDTO{key= Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },


                        phoneNumber = "8152233879"
                    } }
            };
            var address = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                firstName = "abc",
                lastName = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        emailAddress = "abcabc@gmail.com",
                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        line1 = "1st",
                        line2 = "line2",
                        country = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },

                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },
                        stateName = "stateName",
                        city = "city",
                        zipCode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                        type = new TypeDTO{key= Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },
                        phoneNumber = "0152233879"
                    } }
            };
            var saveToDb = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                firstName = "abc",
                lastName = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        emailAddress = "abcabc@gmail.com",
                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        line1 = "1sqwdty",
                        line2 = "line2",
                        country = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },

                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },
                        stateName = "stateName",
                        city = "city",
                        zipCode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                        type = new TypeDTO{key= Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },
                        phoneNumber = "0152233879"
                    } }
            };
       
            var emailExists =await _addresBookController.AddNewAddressBook(email);
            var phoneExists = await _addresBookController.AddNewAddressBook(phone);
            var addressExists = await _addresBookController.AddNewAddressBook(address);
            var save = await _addresBookController.AddNewAddressBook(saveToDb);
         //   var savenull = await _addresBookController.AddNewAddressBook(saveToDbNull);


            ObjectResult EmailExists = Assert.IsType<ObjectResult>(emailExists);
            ObjectResult PhoneExists = Assert.IsType<ObjectResult>(phoneExists);
            ObjectResult AddressExists = Assert.IsType<ObjectResult>(addressExists);
            ObjectResult saveDb = Assert.IsType<ObjectResult>(save);
          //  ObjectResult saveDbNull = Assert.IsType<ObjectResult>(savenull);

            Assert.Equal(409, EmailExists.StatusCode);
            Assert.Equal(409, PhoneExists.StatusCode);
            Assert.Equal(409, AddressExists.StatusCode);
            Assert.Equal(201, saveDb.StatusCode);
          //  Assert.Equal(404, saveDbNull.StatusCode);
        }
        [Fact]
        public async void DeleteAddressBook_Test()
        {
            var id = Guid.NewGuid();
            var existId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9");

            var response =await _addresBookController.DeleteAddressBook(id);
            var okResponse = await _addresBookController.DeleteAddressBook(existId);

            Assert.IsType<NotFoundResult>(response);
            Assert.IsType<OkResult>(okResponse);

        }
        [Fact]
        public async void UpdateAddressBook_Test()
        {
            var account = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                firstName = "abc",
                lastName = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        emailAddress = "abc@gmail.com",
                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        line1 = "1st",
                        line2 = "line2",
                        country = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },

                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },
                        stateName = "stateName",
                        city = "city",
                        zipCode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                        type = new TypeDTO{key= Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },


                        phoneNumber = "8152233879"
                    }, new PhoneDTO
                    {

                        type = new TypeDTO{key= Guid.Parse("F87B8232-F2D8-4286-AC13-422AA54194CE") },
                        phoneNumber = "8122233879"
                    } }
            };
            var id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9");
            var inValid = Guid.NewGuid();

            var response =await _addresBookController.UpdateAddressBook(id,account);
            var notFound = await _addresBookController.UpdateAddressBook(inValid, account);

            Assert.IsType<NotFoundResult>(notFound);
            Assert.IsType<OkResult>(response);

        }
        public void Disposal()

        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
