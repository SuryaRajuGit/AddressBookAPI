
using AddressBookAPI.Entity.Dto;
using AddressBookAPI.Helpers;

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
using AddressBookAPI.Entity.Dto;
using AddressBookAPI.Entity.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            string userPassword = _addressBookRepository.loginDetails(logInDTO.user_name);
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


            if (password == logInDTO.password)
            {
                JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();
                byte[] tokenKey = Encoding.UTF8.GetBytes("thisismySecureKey12345678");

                SecurityTokenDescriptor tokenDeprictor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name, logInDTO.user_name) }
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
            IEnumerable<user> sortList = sortBy == "firstName" ? pagenatedList.OrderBy(s => s.first_name) : pagenatedList.OrderBy(s => s.last_name);
            if (sortOrder == "DSC")
            {
                sortList = sortBy == "firstName" ? sortList.OrderByDescending(s => s.first_name) : sortList.OrderByDescending(s => s.last_name);
            }
           
            List<UserDTO> accountsList = new List<UserDTO>();
            foreach (user i in sortList)
            {
                UserDTO user = _mapper.Map<UserDTO>(i); //AddressBookMaping(item);

               
                foreach (string item in user.Email.Select(s => s.type.key))
                {
                    if (item.Length < 32)
                    {
                        break;
                    }
                    string type = _addressBookRepository.GetType(Guid.Parse(item));
                    user.Email.Where(s => s.type.key == item).Select(c => { c.type.key = type; return c; }).ToList();
                }
                foreach (string item in user.Phone.Select(s => s.type.key))
                {
                    if (item.Length < 32)
                    {
                        break;
                    }
                    string type = _addressBookRepository.GetType(Guid.Parse(item));
                    user.Phone.Where(s => s.type.key == item).Select(c => { c.type.key = type; return c; }).ToList();
                }
                Guid countryKey = Guid.Parse(user.Address.Select(s => s.country.key).FirstOrDefault());
                string getCountryType = _addressBookRepository.GetType(countryKey);
                user.Address.Select(c => c.country).Select(s => { s.key = getCountryType; return s; }).ToList();

                Guid addressKey = Guid.Parse(user.Address.Select(s => s.type.key).FirstOrDefault());
                string addressType = _addressBookRepository.GetType(addressKey);
                user.Address.Select(c => c.type).Select(s => { s.key = addressType; return s; }).ToList();
                accountsList.Add(user);
                
            }
            return accountsList;
        }
        public bool isMatched(ICollection<EmailDTO> emailDTOs)
        {
            HashSet<Guid> set = new HashSet<Guid>();
            HashSet<string> set1 = new HashSet<string>();
            foreach (EmailDTO item in emailDTOs.Select(s => s))
            {
                set.Add(Guid.Parse(item.type.key));
                set1.Add(item.email_address);
            }
            if (set.Count() != emailDTOs.Select(s => s.type.key).Count())
            {
                if (set1.Count() != emailDTOs.Select(s => s.email_address).Count())
                {
                    return true;//new ErrorDTO { type = "Conflict", description = "both entered Emails are same" };
                }

            }
            return false;
            
        }
        public bool isMatchedphone(ICollection<PhoneDTO> phoneDTOs)
        {
            HashSet<Guid> set = new HashSet<Guid>();
            HashSet<string> set1 = new HashSet<string>();
            foreach (PhoneDTO item in phoneDTOs.Select(s => s))
            {
                set.Add(Guid.Parse(item.type.key));
                set1.Add(item.phone_number);
            }
            if (set.Count() != phoneDTOs.Select(s => s.type.key).Count())
            {
                if (set1.Count() != phoneDTOs.Select(s => s.phone_number).Count())
                {
                    return true;//new ErrorDTO { type = "Conflict", description = "both entered Emails are same" };
                }

            }
            return false;

        }
        public ErrorDTO Duplicates(UserDTO userDTO)
        {
            
                if (userDTO.Email.Count() > 1)
                {
                    bool isMatchedEMail = isMatched(userDTO.Email);
                    if (isMatchedEMail)
                    {
                        return new ErrorDTO { type = "Conflict", description = "both entered Emails are same" };
                    }

                }
                if (userDTO.Phone.Count() > 1)
                {
                    bool isMatchedPhone = isMatchedphone(userDTO.Phone);
                    if (isMatchedPhone)
                    {
                        return new ErrorDTO { type = "Conflict", description = "both entered Phone no are same" };
                    }

                }
            
            return null;
        }

        // updates existing  AddressBook based on userId
        public ErrorDTO?  UpdateAddressBook(Guid id, UserDTO userDTO)
        {
            //takes Args Guid ,returns user Dto,gets addressbook from database using userId.
            user account = _addressBookRepository.GetAccount(id, userDTO);
            if(account == null)
            {
                return new ErrorDTO { type = "NotFound", description = "AddressBook not found " };
            }
            account.first_name = userDTO.first_name;
            account.last_name = userDTO.last_name;
            account.email = userDTO.Email.Select(s => new email
            {
                email_address = s.email_address,
                refTermId = Guid.Parse(s.type.key),
            }).ToList();


            account.address = userDTO.Address.Select(s => new address
            {
                refTermId = Guid.Parse(s.type.key),
                line1 = s.line1,
                line2 = s.line2,
                state_name = s.state_name,
                city = s.city,
                zipCode = s.zipCode,
                country = Guid.Parse(s.country.key),
            }).ToList();
            account.phone = userDTO.Phone.Select(s =>
                new phone
                {
                    refTermId = Guid.Parse(s.type.key),
                    phone_number = s.phone_number
                }).ToList().ToList();

            try
            {
                _addressBookRepository.UpdateToDataBase(account);
            }
            catch
            {
                return new ErrorDTO { type = "NotFound", description = "Meta-data not found" }; ;
            }
            return null;
        }
        // validation attribute error 
        public ErrorDTO modelStateInvaliLoginAPI(ModelStateDictionary ModelState)
        {
            return new ErrorDTO
            {
                type = ModelState.Keys.Select(s => s).FirstOrDefault(),
                description = ModelState.Values.Select(s => s.Errors[0].ErrorMessage).FirstOrDefault()
            };
        }
        // validation attribute error 
        public ErrorDTO modelStateInvalidSinupAPI(ModelStateDictionary ModelState)
        {
            return new ErrorDTO
            {
                type = ModelState.Keys.FirstOrDefault(),
                description =
                ModelState.Values.Select(s => s.Errors.Select(s => s.ErrorMessage).FirstOrDefault()).FirstOrDefault()
            };
        }
        // Gets count of AddressBooks from database
        public int GetAddressBookCount()
        {
            return _addressBookRepository.GetAddressBookCount();
        }

        // Checks email exists in the database and return string 
        public ErrorDTO EmailExists(ICollection<EmailDTO> email)
        {
            try
            {
                bool isEmailDup = isMatched(email);
                if(isEmailDup)
                {
                    return  new ErrorDTO { type = "Conflict", description = "both entered Emails are same" };
                }
                
            }
            catch
            {
                return new ErrorDTO { type = "Invaid dataType", description = "Wrong Key Value type" };
            }

            // takes Args email ICollection<EmailDTO> ,returns string ,checks emails exist in the database
            string isEmailExists = _addressBookRepository.EmailList(email);
            if(isEmailExists != null)
            {
                return new ErrorDTO
                {
                    type = "Conflict",
                    description = isEmailExists +
                    " Email already exists"
                };
            }
            return null;
        }

        // Checks phone no exists in the database and return string
        public ErrorDTO PhoneExists(ICollection<PhoneDTO> phone)
        {
            HashSet<Guid> set = new HashSet<Guid>();
            HashSet<string> set1 = new HashSet<string>();
            try
            {
                bool isEmailDup = isMatchedphone(phone);
                if (isEmailDup)
                {
                    return new ErrorDTO { type = "Conflict", description = "both entered Phone no are same" };
                }
            }
            catch
            {
                return  new ErrorDTO{type="Invaid dataType", description = "Wrong Key Value type"};
            }
            // takes Args phone ICollection<PhoneDTO> ,returns string ,checks phone no exist in the database
            string isPhoneExists = _addressBookRepository.PhoneList(phone);
            if (isPhoneExists != null)
            {
                return new ErrorDTO
                {
                    type = "Conflict",
                    description = isPhoneExists +
                    " Phone number already exists"
                };
            }
            return null;
        }

        public ErrorDTO modelStateInvalid(ModelStateDictionary modelState)
        {
            return new ErrorDTO
            {
                type = modelState.Select(s => s).Where(s => s.Value.ValidationState.ToString() == "Invalid").Select(s => s.Key).FirstOrDefault(),
                
                //Select(s => s.AttemptedValue).FirstOrDefault(),
                description = modelState.Values.Where(w => w.ValidationState.ToString() == "Invalid").
                Select(s => s.Errors[0].ErrorMessage).FirstOrDefault()
            
            };
        }

        // Checks address exists in the database and return string
        public ErrorDTO AddressExists(ICollection<AddressDTO> addresses)
        {
            
            // takes Args Address ICollection<AddressDTO> ,returns string ,checks Address exist in the database
            string isAddressExists = _addressBookRepository.AdderessList(addresses);
            if (isAddressExists != null)
            {
                return  new ErrorDTO
                {
                    type = "Conflict",
                    description = isAddressExists +
                    " already exists"
                };
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
                first_name = userDTO.first_name,
                last_name = userDTO.last_name,

                email = userDTO.Email.Select(s =>
                   new email
                   {
                       email_address = s.email_address,
                       userId = newId,
                       refTermId = Guid.Parse(s.type.key),
                   }).ToList(),

                address = userDTO.Address.Select(s =>
                new address
                {
                    line1 = s.line1,
                    line2 = s.line2,
                    country =  Guid.Parse(s.country.key),
                    userId = newId,
                    refTermId = Guid.Parse(s.type.key),
                    state_name = s.state_name,
                    city = s.city,
                    zipCode = s.zipCode
                }).ToList(),

                phone = userDTO.Phone.Select(s =>
                new phone
                {
                    refTermId = Guid.Parse(s.type.key),
                    userId = newId,
                    phone_number = s.phone_number
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
            user account = _addressBookRepository.GetAddressbook(id);
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
                file_name = file.FileName,
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
            user account =  _addressBookRepository.GetAddressbook(id);
            if (account == null)
            {
                return null;
            }
            
            UserDTO addressBook = _mapper.Map<UserDTO>(account);
            UserDTO addressBookObj = addressBook;
            foreach (string item in addressBook.Email.Select(s => s.type.key))
            {
                if (item.Length < 32)
                {
                    break;
                }
                string type = _addressBookRepository.GetType(Guid.Parse(item));
                addressBookObj.Email.Where(s => s.type.key == item).Select(c => { c.type.key = type; return c; }).ToList();
            }
            foreach (string item in addressBook.Phone.Select(s => s.type.key))
            {
                if (item.Length < 32)
                {
                    break;
                }
                string type = _addressBookRepository.GetType(Guid.Parse(item));
                addressBookObj.Phone.Where(s => s.type.key == item).Select(c => { c.type.key = type; return c; }).ToList();
            }
            Guid countryKey = Guid.Parse(addressBook.Address.Select(s => s.country.key).FirstOrDefault());
            string getCountryType = _addressBookRepository.GetType(countryKey);
            addressBookObj.Address.Select(c => c.country).Select(s => { s.key = getCountryType; return s; }).ToList();

            Guid addressKey = Guid.Parse(addressBook.Address.Select(s => s.type.key).FirstOrDefault());
            string addressType = _addressBookRepository.GetType(addressKey);
            addressBookObj.Address.Select(c => c.type).Select(s => { s.key = addressType; return s; }).ToList();
            return addressBookObj;
        }

        // saves new Admin logins into database 
        public string SignupAdmin(SignupDTO signupDTO)
        {
            string text = signupDTO.password;
            // takes Args new login userName string ,return bool ,checks username exists in database
            bool isExists = _addressBookRepository.isUserNameExists(signupDTO.user_name);
            if (isExists)
            {
                return null;
            }
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(this.key, this.iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            string password = Convert.ToBase64String(outputBuffer);
            login loginObj = new login { user_name = signupDTO.user_name, password = password };
            _addressBookRepository.SinupAdmin(loginObj);
            return "";

        }
    }

}
