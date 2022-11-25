using AddressBookAPI;
using AddressBookAPI.Controllers;

using AddressBookAPI.Entity.Dto;
using AddressBookAPI.Entity.Models;
using AddressBookAPI.Helpers;
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
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder().
            ConfigureLogging((builderContext, loggingBuilder) =>
            {
                loggingBuilder.AddConsole((options) =>
                {
                    options.IncludeScopes = true;
                });
            });
            IHost host = hostBuilder.Build();
            ILogger<AddressBookController> _logger = host.Services.GetRequiredService<ILogger<AddressBookController>>();

            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApplicationMapper());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;

            DbContextOptions<AddressBookContext> options = new DbContextOptionsBuilder<AddressBookContext>()
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
                first_name = "abc",
                last_name = "xyz",

                email = new List<email>() {
                new email
                   {
                       email_address = "abc@gmail.com",
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
                    state_name = "stateName",
                    city = "city",
                    zipCode ="zipCode"
                },},

                phone =new List<phone> {
                new phone
                {
                    refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                    userId =Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9") ,
                    phone_number = "8152233879"
                },new phone
                {
                    refTermId = Guid.Parse("F87B8232-F2D8-4286-AC13-422AA54194CE"),
                    userId =Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9") ,
                    phone_number = "8122233879"
                } }

        },new user()
            {
                Id = Guid.Parse("E1E91040-8C5E-4748-A625-2E493C7818D9"),
                first_name = "def",
                last_name = "jkh",

                email = new List<email>() {
                new email
                   {
                       email_address = "xyz@gmail.com",
                       userId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                       refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                   }, 
                new email
                   {
                       email_address = "abc@gmail.com",
                       userId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                       refTermId = Guid.Parse("04CD138D-6CE7-4389-919C-6687CF7F011F"),
                   } },

                address = new List<address>() {
                new address
                {
                    line1 = "1st",
                    line2 = "line2",
                    country = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                    userId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                    refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                    state_name = "stateName",
                    city = "city",
                    zipCode ="zipCode"
                },},

                phone =new List<phone> {
                new phone
                {
                    refTermId = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362"),
                    userId =Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9") ,
                    phone_number = "9152233879"
                },new phone
                {
                    refTermId = Guid.Parse("F87B8232-F2D8-4286-AC13-422AA54194CE"),
                    userId =Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9") ,
                    phone_number = "0122233879"
                } }

        }, };

            FileStream sourceImg = File.OpenRead(@"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entity\files\cap2.PNG");
            MemoryStream memoryStream = new MemoryStream();
            sourceImg.CopyToAsync(memoryStream);
            byte[] bytearr = memoryStream.ToArray();
            asset assetdto = new asset { Id = Guid.Parse("D1E910408C5E4748A6252E493C7818D9"), field = bytearr };

            login valdLogin = new login { password = "UnVmvnspqYZtOlgWDkmkKAbuj7qrNOH9", user_name = "surya123" };

            _context.Login.Add(valdLogin);
            _context.User.AddRange(list);
            _context.AssetDTO.Add(assetdto);
            _context.SaveChanges();
            _context.AddRange();
        }

        [Fact]
        public  void GetAddressBookCount_Test()
        {
            OkObjectResult response =  _addresBookController.GetAddressBookCount() as OkObjectResult;
            Assert.IsType<ResponseGetCountDTO>(response.Value);

            ResponseGetCountDTO listBooks = response.Value as ResponseGetCountDTO;

            Assert.Equal(2, listBooks.count);
        }

        [Fact]
        public  void GetAllAddressBooksAccounts_Test()
        {

            ActionResult<List<UserDTO>> okResult =  _addresBookController.GetAllAddressBooksAccounts(4, 1, "firstName", "DSC");
            ActionResult<List<UserDTO>> notFoundResult =  _addresBookController.GetAllAddressBooksAccounts(10, 10, "lastName", "ASC");

            Assert.IsType<OkObjectResult>(okResult.Result);

            OkObjectResult list = okResult.Result as OkObjectResult;

            Assert.IsType<List<UserDTO>>(list.Value);

            List<UserDTO> listBooks = list.Value as List<UserDTO>;

            Assert.Equal(2, listBooks.Count);
        }

        [Fact]
        public  void GetAddressBook_Test()
        {
            Guid validId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9");
            Guid invalidId = Guid.NewGuid();
            ActionResult<UserDTO> notFoundResult = _addresBookController.GetAddressBook(invalidId);
            ActionResult<UserDTO> okResult = _addresBookController.GetAddressBook(validId);


            OkObjectResult item = okResult.Result as OkObjectResult;
            UserDTO bookItem = item.Value as UserDTO;

            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
            Assert.IsType<OkObjectResult>(okResult.Result);
            Assert.IsType<UserDTO>(item.Value);
            Assert.Equal(validId, bookItem.Id);
            Assert.Equal("abc", bookItem.first_name);
        }

        [Fact]
        public  void VerifyUser_Test()
        {
            LogInDTO inValidLogin = new LogInDTO { password = "jna", user_name = "ksnknc" };
            LogInDTO valdLogin = new LogInDTO { password = "Surya@123", user_name = "surya123" };

            IActionResult unAuthorised =  _addresBookController.VerifyUser(inValidLogin);
            IActionResult Authorised =  _addresBookController.VerifyUser(valdLogin);

            Assert.IsType<UnauthorizedObjectResult>(unAuthorised);
            Assert.IsType<OkObjectResult>(Authorised);
        }

        [Fact]
        public  void UploadFile_Test()
        {
            Mock<IFormFile> file = new Mock<IFormFile>();
            FileStream sourceImg = File.OpenRead(@"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entity\files\cap2.PNG");
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(sourceImg);
            writer.Flush();
            stream.Position = 0;
            string fileName = "cap2.PNG";
            file.Setup(f => f.OpenReadStream()).Returns(stream);
            file.Setup(f => f.FileName).Returns(fileName);
            file.Setup(f => f.Length).Returns(stream.Length);
            IFormFile inputFile = file.Object;

            ActionResult<UploadResponseDTO> okResult = _addresBookController.UploadFile(inputFile);
            OkObjectResult result = okResult.Result as OkObjectResult;

            Assert.IsType<OkObjectResult>(okResult.Result); 
            Assert.IsType<UploadResponseDTO>(result.Value);
        //    var bookItem = result.Value as UploadResponseDTO;
         //   Assert.Equal(fileName, bookItem.fileName);
        }

        [Fact]
        public  void Download_Test()
        {
            Guid inValid = Guid.NewGuid();
            Guid valid = Guid.Parse("D1E910408C5E4748A6252E493C7818D9");

            IActionResult notFoundResult =  _addresBookController.Download(inValid);
            IActionResult FoundResult =  _addresBookController.Download(valid);

            Assert.IsType<NotFoundObjectResult>(notFoundResult);
            Assert.IsType<FileContentResult>(FoundResult);
        }

        [Fact]
        public  void SignUpAdmin_Test()
        {
            SignupDTO notexists = new SignupDTO {user_name="suryaNew",password="sur22sgya@1Rs" };
            SignupDTO exists = new SignupDTO { user_name = "surya123", password = "Surya@123" };
            SignupDTO badRequest = new SignupDTO { user_name = "suryaNew", password = "gya1Rs" };
            //   var sinup =await  _addresBookController.SignUpAdmin(dto);

            IActionResult conflictResponse =  _addresBookController.SignUpAdmin(exists);
            IActionResult okResponse =  _addresBookController.SignUpAdmin(notexists);
      //      IActionResult OkbadRequest = _addresBookController.SignUpAdmin(badRequest);

            ObjectResult isAlreadyResponse = Assert.IsType<ObjectResult>(conflictResponse);
      //      ObjectResult badResponse = Assert.IsType<ObjectResult>(OkbadRequest);

            Assert.Equal(409, isAlreadyResponse.StatusCode);
            Assert.IsType<OkResult>(okResponse);
       //     Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public  void AddNewAddressBook_Test()
        {
            UserDTO email = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                first_name = "abc",
                last_name = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        email_address = "abc@gmail.com",
                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        line1 = "1st",
                        line2 = "line2",
                        country = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },

                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },
                        state_name = "stateName",
                        city = "city",
                        zipCode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                        type = new TypeDTO{key= Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },
                  
              
                        phone_number = "8152233879"
                    }, new PhoneDTO
                    {
                
                        type = new TypeDTO{key= Guid.Parse("F87B8232-F2D8-4286-AC13-422AA54194CE").ToString() },
                        phone_number = "8122233879"
                    } }
            };
            UserDTO phone = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                first_name = "abc",
                last_name = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        email_address = "abcabc@gmail.com",
               //         type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        line1 = "1st",
                        line2 = "line2",
                //        country = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },

                 //       type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },
                        state_name = "stateName",
                        city = "city",
                        zipCode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                  //      type = new TypeDTO{key= Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },


                        phone_number = "8152233879"
                    } }
            };
            UserDTO address = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                first_name = "abc",
                last_name = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        email_address = "abcabc@gmail.com",
                //        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        line1 = "1st",
                        line2 = "line2",
                  //      country = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },

                  //      type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },
                        state_name = "stateName",
                        city = "city",
                        zipCode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                   //     type = new TypeDTO{key= Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362") },
                        phone_number = "0152233879"
                    } }
            };
            UserDTO saveToDb = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                first_name = "abc",
                last_name = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        email_address = "abcabc@gmail.com",
                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        line1 = "1sqwdty",
                        line2 = "line2",
                        country = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },

                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },
                        state_name = "stateName",
                        city = "city",
                        zipCode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                        type = new TypeDTO{key= Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },
                        phone_number = "8142255769"
                    } }
            };
       
            IActionResult emailExists = _addresBookController.AddNewAddressBook(email);
            IActionResult phoneExists =  _addresBookController.AddNewAddressBook(phone);
            IActionResult addressExists =  _addresBookController.AddNewAddressBook(address);
            IActionResult save =  _addresBookController.AddNewAddressBook(saveToDb);
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
        public  void DeleteAddressBook_Test()
        {
            Guid id = Guid.NewGuid();
            Guid existId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9");

            IActionResult response = _addresBookController.DeleteAddressBook(id);
            IActionResult okResponse =  _addresBookController.DeleteAddressBook(existId);

            Assert.IsType<NotFoundObjectResult>(response);
            Assert.IsType<OkResult>(okResponse);

        }

        [Fact]
        public  void UpdateAddressBook_Test()
        {
            UserDTO account = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                first_name = "abc",
                last_name = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        email_address = "abc@gmail.com",
                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        line1 = "1st",
                        line2 = "line2",
                        country = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },

                        type = new TypeDTO { key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },
                        state_name = "stateName",
                        city = "city",
                        zipCode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                        type = new TypeDTO{key= Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },


                        phone_number = "8152233879"
                    }, new PhoneDTO
                    {

                        type = new TypeDTO{key= Guid.Parse("F87B8232-F2D8-4286-AC13-422AA54194CE").ToString() },
                        phone_number = "8122233879"
                    } }
            };
            Guid id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9");
            Guid inValid = Guid.NewGuid();

            IActionResult response = _addresBookController.UpdateAddressBook(account,id);
            IActionResult notFound =  _addresBookController.UpdateAddressBook( account, inValid);

            Assert.IsType<NotFoundObjectResult>(notFound);
            Assert.IsType<OkResult>(response);

        }
        public void Disposal()

        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
