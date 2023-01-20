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
    public class UnitTest1 
    {
        private readonly IMapper _mapper;
        private readonly AddressBookController _addresBookController;
        private readonly AddressBookRepository _repository;
        private readonly AddressBookServices _addressBookServices;
        private readonly IConfiguration _configuration;
        private readonly IServer _server;
        private readonly AddressBookContext _context;
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

            string path = @"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entities\UnitTestFiles\data.csv";
            string ReadCSV = File.ReadAllText(path);
            string[] data = ReadCSV.Split('\r');
            List<User> list = new List<User>();
            foreach (string item in data)
            {
                string[] row = item.Split(",");
                User user = new User()
                {
                    Id = Guid.Parse(row[0]),
                    FirstName = row[1],
                    LastName = row[2],

                    Email = new List<Email>() {
                new Email
                   {
                       EmailAddress = row[3],
                       UserId = Guid.Parse(row[4]),
                       RefTermId = Guid.Parse(row[5]),
                   }, },
                    Address = new List<Address>() {
                new Address
                {
                    Line1 = row[6],
                    Line2 = row[7],
                    Country = Guid.Parse(row[8]),
                    UserId = Guid.Parse(row[9]),
                    RefTermId = Guid.Parse(row[10]),
                    StateName = row[11],
                    City = row[12],
                    Zipcode = row[13]
                },},

                    Phone = new List<Phone> {
                new Phone
                {
                    RefTermId = Guid.Parse(row[14]),
                    UserId =Guid.Parse(row[15]) ,
                    PhoneNumber = row[16]
                },new Phone
                {
                    RefTermId = Guid.Parse(row[17]),
                    UserId =Guid.Parse(row[18]) ,
                    PhoneNumber = row[19]
                } }

                };
                list.Add(user);

            }

            
            FileStream sourceImg = File.OpenRead(@"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entities\UnitTestFiles\files\cap2.PNG");
            MemoryStream memoryStream = new MemoryStream();
            sourceImg.CopyToAsync(memoryStream);
            byte[] bytearr = memoryStream.ToArray();
            Asset assetdto = new Asset { Id = Guid.Parse("D1E910408C5E4748A6252E493C7818D9"), Field = bytearr };

            Login valdLogin = new Login { Password = "UnVmvnspqYZtOlgWDkmkKAbuj7qrNOH9", UserName = "surya123" };

            string path1 = @"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entities\UnitTestFiles\RefData.csv";
            string ReadCSV1 = File.ReadAllText(path1);
            string[] data1 = ReadCSV1.Split('\r');



            foreach (string item in data1)
            {
                string[] row = item.Split(",");
                RefTerm termData = new RefTerm
                {
                    Id = Guid.Parse(row[0]),
                    Key = row[1],
                    Description = row[2]
                };
                RefSet setData = new RefSet
                {
                    Id = Guid.Parse(row[3]),
                    Key = row[4],
                    Description = row[5]
                };
                SetRefTerm setTerm = new SetRefTerm
                {
                    Id = Guid.NewGuid(),
                    RefSetId = Guid.Parse(row[6]),
                    RefTermId = Guid.Parse(row[7])
                };
                _context.Add(termData);
                _context.Add(setData);
                _context.Add(setTerm);
            }
            
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
            Assert.Equal("abc", bookItem.FirstName);
        }

        [Fact]
        public  void VerifyUser_Test()
        {
            LogInDTO inValidLogin = new LogInDTO { Password = "jna", UserName = "ksnknc" };
            LogInDTO valdLogin = new LogInDTO { Password = "Surya@123", UserName = "surya123" };

            IActionResult unAuthorised =  _addresBookController.VerifyUser(inValidLogin);
            IActionResult Authorised =  _addresBookController.VerifyUser(valdLogin);

            Assert.IsType<UnauthorizedObjectResult>(unAuthorised);
            Assert.IsType<OkObjectResult>(Authorised);
        }

        [Fact]
        public  void UploadFile_Test()
        {
            Mock<IFormFile> file = new Mock<IFormFile>();
            FileStream sourceImg = File.OpenRead(@"C:\Users\Hp\source\repos\AddressBookAPI\AddressBookAPI\Entities\UnitTestFiles\files\cap2.PNG");
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
            SignupDTO notexists = new SignupDTO {UserName="suryaNew",Password="sur22sgya@1Rs" };
            SignupDTO exists = new SignupDTO { UserName = "surya123", Password = "Surya@123" };
            SignupDTO badRequest = new SignupDTO { UserName = "suryaNew", Password = "gya1Rs" };

            IActionResult conflictResponse =  _addresBookController.SignUpAdmin(exists);
            IActionResult okResponse =  _addresBookController.SignUpAdmin(notexists);

            ObjectResult isAlreadyResponse = Assert.IsType<ObjectResult>(conflictResponse);

            Assert.Equal(409, isAlreadyResponse.StatusCode);
            Assert.IsType<OkObjectResult>(okResponse);
        }

        [Fact]
        public  void AddNewAddressBook_Test()
        {
            UserDTO email = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                FirstName = "abc",
                LastName = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        EmailAddress = "abc@gmail.com",
                        Type = new TypeDTO { Key = "12CF7780-9096-4855-A049-40476CEAD362" },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        Line1 = "1st",
                        Line2 = "line2",
                        Country = new TypeDTO { Key = "12CF7780-9096-4855-A049-40476CEAD362" },

                        Type = new TypeDTO { Key = "12CF7780-9096-4855-A049-40476CEAD362" },
                        StateName = "stateName",
                        City = "city",
                        Zipcode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                        Type = new TypeDTO{Key= "12CF7780-9096-4855-A049-40476CEAD362"},            
                        PhoneNumber = "8152233879"
                    }, new PhoneDTO
                    {
                
                        Type = new TypeDTO{Key= "F87B8232-F2D8-4286-AC13-422AA54194CE" },
                        PhoneNumber = "8122233879"
                    } }
            };
            UserDTO phone = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                FirstName = "abc",
                LastName = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        EmailAddress = "abcabc@gmail.com",
                        Type = new TypeDTO { Key ="12CF7780-9096-4855-A049-40476CEAD362".ToString() },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        Line1 = "1st",
                        Line2 = "line2",
                        Country = new TypeDTO { Key = "12CF7780-9096-4855-A049-40476CEAD362" },

                        Type = new TypeDTO { Key = "12CF7780-9096-4855-A049-40476CEAD362" },
                        StateName = "stateName",
                        City = "city",
                        Zipcode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                        Type = new TypeDTO{Key= "12CF7780-9096-4855-A049-40476CEAD362" },
                        PhoneNumber = "8152233879"
                    } }
            };
            UserDTO address = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                FirstName = "abc",
                LastName = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        EmailAddress = "abcabc@gmail.com",
                        Type = new TypeDTO { Key = "12CF7780-9096-4855-A049-40476CEAD362" },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        Line1 = "1st",
                        Line2 = "line2",
                        Country = new TypeDTO { Key = "12CF7780-9096-4855-A049-40476CEAD362" },

                        Type = new TypeDTO { Key = "12CF7780-9096-4855-A049-40476CEAD362" },
                        StateName = "stateName",
                        City = "city",
                        Zipcode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                       Type = new TypeDTO{Key= "12CF7780-9096-4855-A049-40476CEAD362" },
                        PhoneNumber = "0152233879"
                    } }
            };
            UserDTO saveToDb = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                FirstName = "abc",
                LastName = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        EmailAddress = "abcabc@gmail.com",
                        Type = new TypeDTO { Key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        Line1 = "1sqwdty",
                        Line2 = "line2",
                        Country = new TypeDTO { Key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },

                        Type = new TypeDTO { Key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },
                        StateName = "stateName",
                        City = "city",
                        Zipcode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                        Type = new TypeDTO{Key= Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },
                        PhoneNumber = "8142255769"
                    } }
            };
       
            IActionResult emailExists = _addresBookController.AddNewAddressBook(email);
            IActionResult phoneExists =  _addresBookController.AddNewAddressBook(phone);
            IActionResult addressExists =  _addresBookController.AddNewAddressBook(address);
            IActionResult save =  _addresBookController.AddNewAddressBook(saveToDb);


            ObjectResult EmailExists = Assert.IsType<ObjectResult>(emailExists);
            ObjectResult PhoneExists = Assert.IsType<ObjectResult>(phoneExists);
            ObjectResult AddressExists = Assert.IsType<ObjectResult>(addressExists);
            ObjectResult saveDb = Assert.IsType<ObjectResult>(save);

            Assert.Equal(409, EmailExists.StatusCode);
            Assert.Equal(409, PhoneExists.StatusCode);
            Assert.Equal(409, AddressExists.StatusCode);
            Assert.Equal(201, saveDb.StatusCode);
        }

        [Fact]
        public  void DeleteAddressBook_Test()
        {
            Guid id = Guid.NewGuid();
            Guid existId = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9");

            IActionResult response = _addresBookController.DeleteAddressBook(id);
            IActionResult okResponse =  _addresBookController.DeleteAddressBook(existId);

            Assert.IsType<NotFoundObjectResult>(response);
            Assert.IsType<OkObjectResult>(okResponse);

        }

        [Fact]
        public  void UpdateAddressBook_Test()
        {
            UserDTO account = new UserDTO()
            {
                Id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9"),
                FirstName = "abc",
                LastName = "xyz",

                Email = new List<EmailDTO>() {
                    new EmailDTO
                    {
                        EmailAddress = "abcde@gmail.com",
                        Type = new TypeDTO { Key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },

                    }, },
                Address = new List<AddressDTO>() {
                    new AddressDTO
                    {
                        Line1 = "1st",
                        Line2 = "linne2",
                        Country = new TypeDTO { Key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },

                        Type = new TypeDTO { Key = Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },
                        StateName = "stateName",
                        City = "city",
                        Zipcode = "zipCode"
                    }, },

                Phone = new List<PhoneDTO> {
                    new PhoneDTO
                    {
                        Type = new TypeDTO{Key= Guid.Parse("12CF7780-9096-4855-A049-40476CEAD362").ToString() },
                        PhoneNumber = "8152233009"
                    }, new PhoneDTO
                    {

                        Type = new TypeDTO{Key= Guid.Parse("F87B8232-F2D8-4286-AC13-422AA54194CE").ToString() },
                        PhoneNumber = "8120233009"
                    } }
            };
            Guid id = Guid.Parse("D1E91040-8C5E-4748-A625-2E493C7818D9");
            Guid inValid = Guid.NewGuid();

             IActionResult notFound =  _addresBookController.UpdateAddressBook( account, inValid);
            Assert.IsType<NotFoundObjectResult>(notFound);


            IActionResult response = _addresBookController.UpdateAddressBook(account,id);        
            Assert.IsType<OkObjectResult>(response);

        }

        [Fact]
        public void RefsetData_Test()
        {
            string key = "PHONE_NUMBER_TYPE";

            ActionResult<List<RefSetResponseDto>> response = _addresBookController.RefsetData(key);

            Assert.IsType<OkObjectResult>(response.Result);

            OkObjectResult item = response.Result as OkObjectResult;

            Assert.IsType<List<RefSetResponseDto>>(item.Value);

            List<RefSetResponseDto> list = item.Value as List<RefSetResponseDto>;     

            Assert.Equal("WORK",list[0].Key);

        }


        public void Disposal()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
