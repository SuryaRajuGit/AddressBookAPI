using AddressBookAPI.Data;
using AddressBookAPI.Helpers;
using AddressBookAPI.Models;
using AddressBookAPI.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AddressBookAPI.Services
{
    public class AddressBookServices : IAddressBookServices
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAddressBookRepository _addressBookRepository;
        private readonly IServer _server;
        byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        public AddressBookServices(IConfiguration configuration, IAddressBookRepository addressBookRepository, IServer server, IMapper mapper)
        { 
            _configuration = configuration;
            _addressBookRepository = addressBookRepository;
            _server = server;
             _mapper = mapper;
        }

        // verifies user login details and returns logInResponseDTO Dto
        public logInResponseDTO VerifyUser(LogInDTO logInDTO)
        {
            //takes Args login userName string,returns string ,checks the userName exists in the database
            string userPassword = _addressBookRepository.loginDetails(logInDTO.userName);
            if (userPassword == null)
            {
                return null;
            }

            string text = userPassword;
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(this.key, this.iv);
            byte[] inputbuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            string password = Encoding.Unicode.GetString(outputBuffer);


            if (password == logInDTO.Password)
            {
                JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();
                byte[] tokenKey = Encoding.UTF8.GetBytes("thisismySecureKey12345678");

                SecurityTokenDescriptor tokenDeprictor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name, logInDTO.userName) }
                        ),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken token = tokenhandler.CreateToken(tokenDeprictor);
                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                logInResponseDTO response = new logInResponseDTO()
                {
                    jwt = tokenString,
                    type = "Bearer"
                };
                return response;
            }
            return null;

        }

        //returns all AddressBooks from database
        public List<UserDTO> GetAllAddressBooks(int size, int pageNo, string sortBy, string sortOrder)
        {
            // takes Args size Int,pageNo Int ,returns list of AddressBooks after applying pagenation
            List<user> pagenatedList = _addressBookRepository.Pagenation(size, pageNo);
            if (pagenatedList == null)
            {
                return null;
            }
            IEnumerable<user> sortList = sortBy == "firstName" ? pagenatedList.OrderBy(s => s.firstName) : pagenatedList.OrderBy(s => s.lastName);
            if (sortOrder == "DSC")
            {
                sortList = sortBy == "firstName" ? sortList.OrderByDescending(s => s.firstName) : sortList.OrderByDescending(s => s.lastName);
            }
           
            List<UserDTO> accountsList = new List<UserDTO>();
            foreach (user item in sortList)
            {
                UserDTO user = _mapper.Map<UserDTO>(item); //AddressBookMaping(item);
                accountsList.Add(user);
               
            };
            return accountsList;
        }

        // updates existing  AddressBook based on userId
        public void  UpdateAddressBook(Guid id, UserDTO userDTO)
        {
            //takes Args Guid ,returns user Dto,gets addressbook from database using userId.
            user account = _addressBookRepository.GetAccountCount(id);
            account.firstName = userDTO.firstName;
            account.lastName = userDTO.lastName;
            account.email = userDTO.Email.Select(s => new email
            {
                userId = id,
                emailAddress = s.emailAddress,
                refTermId = s.type.key,
            }).ToList();


            account.address = userDTO.Address.Select(s => new address
            {
                refTermId = s.type.key,
                line1 = s.line1,
                line2 = s.line2,
                stateName = s.stateName,
                city = s.city,
                zipCode = s.zipCode,
                country = s.type.key,
                userId = id,
            }).ToList();
            account.phone = userDTO.Phone.Select(s =>
                new phone
                {
                    refTermId = s.type.key,
                    phoneNumber = s.phoneNumber
                }).ToList().ToList();

            //Saves Updated AddressBook into database
            _addressBookRepository.UpdateToDataBase(account);
        }

        // Gets count of AddressBooks from database
        public int GetAddressBookCount()
        {
            return _addressBookRepository.GetAddressBookCount();
        }

        // Checks email exists in the database and return string 
        public string EmailExists(ICollection<EmailDTO> email)
        {
            // takes Args email ICollection<EmailDTO> ,returns string ,checks emails exist in the database
            string isEmailExists = _addressBookRepository.EmailList(email);
            if(isEmailExists != null)
            {
                return isEmailExists;
            }
            return null;
        }

        // Checks phone no exists in the database and return string
        public string PhoneExists(ICollection<PhoneDTO> phone)
        {
            // takes Args phone ICollection<PhoneDTO> ,returns string ,checks phone no exist in the database
            string isPhoneExists = _addressBookRepository.PhoneList(phone);
            if (isPhoneExists != null)
            {
                return isPhoneExists;
            }
            return null;
        }
        // Checks address exists in the database and return string
        public string AddressExists(ICollection<AddressDTO> addresses)
        {
            // takes Args Address ICollection<AddressDTO> ,returns string ,checks Address exist in the database
            string isPhoneExists = _addressBookRepository.AdderessList(addresses);
            if (isPhoneExists != null)
            {
                return isPhoneExists;
            }
            return null;
        }

        // Saves new AddressBook into database 
        public  Guid? saveToDatabase(UserDTO userDTO)
        {
            Guid newId = Guid.NewGuid();

            user user = new user()
            {
                Id = newId,
                firstName = userDTO.firstName,
                lastName = userDTO.lastName,

                email = userDTO.Email.Select(s =>
                   new email
                   {
                       emailAddress = s.emailAddress,
                       userId = newId,
                       refTermId = s.type.key,
                   }).ToList(),

                address = userDTO.Address.Select(s =>
                new address
                {
                    line1 = s.line1,
                    line2 = s.line2,
                    country = s.type.key,
                    userId = newId,
                    refTermId = s.type.key,
                    stateName = s.stateName,
                    city = s.city,
                    zipCode = s.zipCode
                }).ToList(),

                phone = userDTO.Phone.Select(s =>
                new phone
                {
                    refTermId = s.type.key,
                    userId = newId,
                    phoneNumber = s.phoneNumber
                }).ToList()
            };
            try
            {
                // saves new addressBook into database
                _addressBookRepository.SaveToDataBase(user);
            }
            catch
            {
                return null;
            }
            return newId;
            
        }

        //Deletes AddressBook from database
        public string  DeletAddressBook(Guid id)
        {
            //takes Args id Guid ,returns user Dto,checks the addressbook exists in the database
            user account = _addressBookRepository.GetAccountCount(id);
            if (account == null)
            {
                return null;
            }
            //takes account Dto,removes the addressBook from database
            _addressBookRepository.RemoveAccount(account);
            return "";
        }

        //uploads file into database in form of bytes
        public UploadResponseDTO UploadFile(IFormFile file)
        {
            MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);
            byte[] bytes = ms.ToArray();
            asset fileObj = new asset()
            {
                Id = Guid.NewGuid(),
                field = bytes,
            };
            string addresses = _server.Features.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();
            UploadResponseDTO response = new UploadResponseDTO
            {
                Id = fileObj.Id,
                fileName = file.FileName,
                downloadURL = addresses + "api/asset/downloadFile/" + fileObj.Id, 
                fileType = file.ContentType,
                size = file.Length,
                fileContent = null
            };
            // takes fileObj Model ,saves into database
            _addressBookRepository.SaveFileToDataBase(fileObj);

            return response;
        }

        // Downloads file file from database
        public byte[] Download(Guid id)
        {
            //takes Args id Guid,returns bytes[] ,gets file byte[] from database using Guid.
            byte[] bytes = _addressBookRepository.GetFile(id);
            if (bytes == null)
            {
                return null;
            }
            return bytes;
        }
        // returns AddressBook from database using id
        public UserDTO GetAddressBook(Guid id)
        {
            // takes Args id Guid ,returns user Model ,gets user model from database using id.
            user account =  _addressBookRepository.GetAddressBook(id);
            if (account == null)
            {
                return null;
            }
                UserDTO addressBook = _mapper.Map<UserDTO>(account)   ;
          
            return addressBook;
        }

        // saves new Admin logins into database 
        public int? SignupAdmin(SignupDTO signupDTO)
        {
            string text = signupDTO.password;
            // takes Args new login userName string ,return bool ,checks username exists in database
            bool isExists = _addressBookRepository.isUserNameExists(signupDTO.userName);
            if (isExists)
            {
                return null;
            }
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(this.key, this.iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            string password = Convert.ToBase64String(outputBuffer);
            login loginObj = new login { userName = signupDTO.userName, password = password };
            return _addressBookRepository.SinupAdmin(loginObj);

        }
    }

}
