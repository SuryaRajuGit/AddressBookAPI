
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

        // 
        ///<summary>
        ///verifies user login details and returns logInResponseDTO Dto
        ///</summary>
        ///<param name="logInDTO">Guid of file ></param>
        ///<returns>logInResponseDTO</returns>
        public LogInResponseDTO VerifyUser(LogInDTO logInDTO)
        {
            //takes Args login userName string,returns string ,checks the userName exists in the database
            string userPassword = _addressBookRepository.loginDetails(logInDTO.UserName);
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
                byte[] tokenKey = Encoding.UTF8.GetBytes(Constants.secutityKey);

                SecurityTokenDescriptor tokenDeprictor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name, logInDTO.UserName) ,
                         //   new Claim("role","user1"),
                        }
                        ),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };

                SecurityToken token = tokenhandler.CreateToken(tokenDeprictor);
                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                LogInResponseDTO response = new LogInResponseDTO()
                {
                    AccessToken = tokenString,
                    TokenType = Constants.Bearer
                };
                return response;
            }
            return null;

        }

        //
        ///<summary>
        ///returns all AddressBooks from database
        ///</summary>
        ///<param name="size">size of file ></param>
        ///<param name="pageNo">pageno  of file ></param>
        ///<param name="sortBy">sort by fist name or lastname of file ></param>
        ///<param name="sortOrder">sort by ascending or descending  of file ></param>
        ///<returns>list of AddressBooks</returns>
        public List<UserDTO> GetAllAddressBooks(int size , int pageNo , string sortBy , string sortOrder )
        {
            // takes Args size Int,pageNo Int ,returns list of AddressBooks after applying pagenation
            List<User> paginatedList = _addressBookRepository.GetPageinatedList(size, pageNo);
            IEnumerable<User> sortList;
            if (paginatedList == null)
            {
                return null;
            }
            
            if (sortOrder == Constants.DSC)
            {
                sortList = sortBy == Constants.firstName ? paginatedList.OrderBy(item => item.FirstName) : paginatedList.OrderBy(item => item.FirstName);
                sortList = sortBy == Constants.firstName ? sortList.OrderByDescending(item => item.FirstName) : sortList.OrderByDescending(item => item.LastName);
                  
            }
            else
            {
                sortList = sortBy == Constants.firstName ? paginatedList.OrderBy(item => item.FirstName) : paginatedList.OrderBy(item => item.LastName);
            }

            List<UserDTO> accountsList = new List<UserDTO>();
            foreach (User i in sortList)
            {
                UserDTO user = _mapper.Map<UserDTO>(i);

               
                foreach (string item in user.Email.Select(s => s.Type.Key))
                {
                    Guid sample;
                    if (!Guid.TryParse(item, out sample))
                    {
                        break;
                    }
                    string type = _addressBookRepository.GetType(sample);
                    user.Email.Where(s => s.Type.Key == item).Select(src => { src.Type.Key = type; return src; }).ToList();
                }
                foreach (string item in user.Phone.Select(src => src.Type.Key))
                {
                    Guid sample;
                    if (!Guid.TryParse(item, out sample))
                    {
                        break;
                    }
                    string type = _addressBookRepository.GetType(sample);
                    user.Phone.Where(src => src.Type.Key == item).Select(crc => { crc.Type.Key = type; return crc; }).ToList();
                }
                Guid countryKey = Guid.Parse(user.Address.Select(src => src.Country.Key).FirstOrDefault());
                string getCountryType = _addressBookRepository.GetType(countryKey);
                user.Address.Select(crc => crc.Country).Select(src => { src.Key = getCountryType; return src; }).ToList();

                Guid addressKey = Guid.Parse(user.Address.Select(s => s.Type.Key).FirstOrDefault());
                string addressType = _addressBookRepository.GetType(addressKey);
                user.Address.Select(crc => crc.Type).Select(src => { src.Key = addressType; return src; }).ToList();
                accountsList.Add(user);
                
            }
            return accountsList;
        }

        ///<summary>
        ///checks if the entered emails are same or not 
        ///</summary>
        ///<param name="emailDTOs"></param>
        ///<returns>bool value</returns>
        public bool IsEmailMatched(ICollection<EmailDTO> emailDTOs)
        {
            HashSet<Guid> emialTypeHashSet = new HashSet<Guid>();
            HashSet<string> emialHashSet = new HashSet<string>();
            foreach (EmailDTO item in emailDTOs.Select(s => s))
            {
                emialTypeHashSet.Add(Guid.Parse(item.Type.Key));
                emialHashSet.Add(item.EmailAddress);
            }
            if (emialTypeHashSet.Count() != emailDTOs.Select(src => src.Type.Key).Count())
            {
                if (emialHashSet.Count() != emailDTOs.Select(src => src.EmailAddress).Count())
                {
                    return true;
                }

            }
            return false;
            
        }

        ///<summary>
        ///checks if the entered phone no are same or not 
        ///</summary>
        ///<param name="phoneDTOs"></param>
        ///<returns>bool value</returns>
        public bool IsPhoneMatched(ICollection<PhoneDTO> phoneDTOs)
        {
            HashSet<Guid> phoneTypeHashSet = new HashSet<Guid>();
            HashSet<string> phoneHashSet = new HashSet<string>();
            foreach (PhoneDTO item in phoneDTOs.Select(s => s))
            {
                phoneTypeHashSet.Add(Guid.Parse(item.Type.Key));
                phoneHashSet.Add(item.PhoneNumber);
            }
            if (phoneTypeHashSet.Count() != phoneDTOs.Select(src => src.Type.Key).Count())
            {
                if (phoneHashSet.Count() != phoneDTOs.Select(src => src.PhoneNumber).Count())
                {
                    return true;
                }

            }
            return false;

        }

        ///<summary>
        ///updates existing  AddressBook based on userId
        ///</summary>
        ///<param name="id"></param>
        /// ///<param name="userDTO"></param>
        ///<returns>ErrorDTO</returns>
        public ErrorDTO?  UpdateAddressBook(Guid id, UserDTO userDTO)
        {
            //takes Args Guid ,returns user Dto,gets addressbook from database using userId.
            User account = _addressBookRepository.GetAccount(id, userDTO);
            if (account == null)
            {
                return new ErrorDTO { type = "AddressBook", description = "AddressBook not found " };
            }
            bool isCountryExists = _addressBookRepository.IsCountryExixsted(userDTO.Address.Select(s => s.Country.Key).First());
            if (!isCountryExists)
            {
                return new ErrorDTO { type = "meta-data", description = "Meta-data not found" };
            }
            
            try
            {
                account.FirstName = userDTO.FirstName;
            account.LastName = userDTO.LastName;
            account.Email = userDTO.Email.Select(src => new Email
            {
                EmailAddress = src.EmailAddress,
                RefTermId = Guid.Parse(src.Type.Key),
            }).ToList();


            account.Address = userDTO.Address.Select(src => new Address
            {
                RefTermId = Guid.Parse(src.Type.Key),
                Line1 = src.Line1,
                Line2 = src.Line2,
                StateName = src.StateName,
                City = src.City,
                Zipcode = src.Zipcode,
                Country = Guid.Parse(src.Country.Key),
            }).ToList();
            account.Phone = userDTO.Phone.Select(src =>
                new Phone
                {
                    RefTermId = Guid.Parse(src.Type.Key),
                    PhoneNumber = src.PhoneNumber
                }).ToList().ToList();

            
                _addressBookRepository.UpdateToDataBase(account);
            }
            catch
            {
                return new ErrorDTO { type = "Meta-data", description = "Meta-data not found" }; ;
            }
            return null;
        }
         
        ///<summary>
        ///validation attribute error
        ///</summary>
        ///<param name="ModelState"></param>
        ///<returns>ErrorDTO</returns>
        public ErrorDTO ModelStateInvaliLoginAPI(ModelStateDictionary ModelState)
        {
            return new ErrorDTO
            {
                type = ModelState.Keys.Select(src => src).FirstOrDefault(),
                description = ModelState.Values.Select(src => src.Errors[0].ErrorMessage).FirstOrDefault()
            };
        }
        // 
        ///<summary>
        ///validation attribute error 
        ///</summary>
        ///<param name="ModelState"></param>
        ///<returns>ErrorDTO</returns>
        public ErrorDTO ModelStateInvalidSinupAPI(ModelStateDictionary ModelState)
        {
            return new ErrorDTO
            {
                type = ModelState.Keys.FirstOrDefault(),
                description =
                ModelState.Values.Select(src => src.Errors.Select(src => src.ErrorMessage).FirstOrDefault()).FirstOrDefault()
            };
        }
        // 
        ///<summary>
        ///Gets count of AddressBooks from database
        ///</summary>
        ///<returns>int</returns>
        public int GetAddressBookCount()
        {
            return _addressBookRepository.GetAddressBookCount();
        }

        // Checks email exists in the database and return string 
        public ErrorDTO EmailExists(ICollection<EmailDTO> email, Guid id)
        {
           
                bool isEmailDup = IsEmailMatched(email);
                if(isEmailDup)
                {
                    return  new ErrorDTO { type = "email", description = "both entered Emails are same" };
                }

            //takes Args email ICollection<EmailDTO> , returns string , checks emails exist in the database
        string isEmailExists = _addressBookRepository.EmailList(email,id);
        if(isEmailExists != null)
            {
                return new ErrorDTO
                {
                    type = "Email",
                    description = 
                    " Email already exists"
                };
            }
            return null;
        }

        // 
        ///<summary>
        ///Checks phone no exists in the database and return string
        ///</summary>
        ///<param name="phone"></param>
        /// ///<param name="id"></param>
        ///<returns>ErrorDTO</returns>
        public ErrorDTO PhoneExists(ICollection<PhoneDTO> phone,Guid id)
        {
            
                bool isPhoneDup = IsPhoneMatched(phone);
                if (isPhoneDup)
                {
                    return new ErrorDTO { type = "phone", description = "both entered Phone no are same" };
                }
            
           //  takes Args phone ICollection<PhoneDTO> ,returns string ,checks phone no exist in the database
            string isPhoneExists = _addressBookRepository.PhoneList(phone,id);
            if (isPhoneExists != null)
            {
                return new ErrorDTO
                {
                    type = "phone",
                    description = isPhoneExists +
                    " Phone number already exists"
                };
            }
            return null;
        }

        ///<summary>
        ///returns list of meta-data 
        ///</summary>
        ///<param name="key"></param>
        ///<returns>ErrorDTO</returns>
        public List<RefSetResponseDto> GetRefSetData(string key)
        {

            List<RefTerm> refTermList = _addressBookRepository.GetRefSetData(key);
            if(refTermList == null)
            {
                return null;
            }
            List<RefSetResponseDto> responseList = new List<RefSetResponseDto>();
            foreach (RefTerm item in refTermList)
            {
                RefSetResponseDto response = new RefSetResponseDto
                {
                    id = item.Id,
                    Key = item.Key,
                    Description = item.Description
                };
                responseList.Add(response);
            }
            return responseList;
        }

        ///<summary>
        ///returns list of meta-data 
        ///</summary>
        ///<param name="modelState"></param>
        ///<returns>ErrorDTO</returns>
        public ErrorDTO ModelStateInvalid(ModelStateDictionary modelState)
        {
            return new ErrorDTO
            {
                type = modelState.Select(src => src).Where(src => src.Value.ValidationState.ToString() == Constants.invalid).Select(src => src.Key).FirstOrDefault(),
                
                description = modelState.Values.Where(wre => wre.ValidationState.ToString() == Constants.invalid).
                Select(src => src.Errors[0].ErrorMessage).FirstOrDefault()
            
            };
        }

        // 
        ///<summary>
        /// Checks address exists in the database and return string
        ///</summary>
        ///<param name="addresses"></param>
        ///<param name="id"></param>
        ///<returns>ErrorDTO</returns>
        public ErrorDTO AddressExists(ICollection<AddressDTO> addresses, Guid id)
        {
            
            // takes Args Address ICollection<AddressDTO> ,returns string ,checks Address exist in the database
            string isAddressExists = _addressBookRepository.AdderessList(addresses,id);
            if (isAddressExists != null)
            {
                return  new ErrorDTO
                {
                    type = "Address",
                    description = isAddressExists +
                    "\n already exists"
                };
            }
            return null;
        }

        // 

        ///<summary>
        ///Saves new AddressBook into database 
        ///</summary>
        ///<param name="userDTO"></param>
        ///<returns>Guid</returns>
        public Guid? SaveToDatabase(UserDTO userDTO)
        {
            Guid newId = Guid.NewGuid();
            bool isCountryExists = _addressBookRepository.IsCountryExixsted(userDTO.Address.Select(src => src.Country.Key).First());
            if(!isCountryExists)
            {
                return null;
            }
            try
            {
                User user = new User()
                {
                Id = newId,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,

                Email = userDTO.Email.Select(src =>
                   new Email
                   {
                       EmailAddress = src.EmailAddress,
                       UserId = newId,
                       RefTermId = Guid.Parse(src.Type.Key),
                   }).ToList(),

                Address = userDTO.Address.Select(src =>
                new Address
                {
                    Line1 = src.Line1,
                    Line2 = src.Line2,
                    Country =  Guid.Parse(src.Country.Key),
                    UserId = newId,
                    RefTermId = Guid.Parse(src.Type.Key),
                    StateName = src.StateName,
                    City = src.City,
                    Zipcode = src.Zipcode
                }).ToList(),

                Phone = userDTO.Phone.Select(src =>
                new Phone
                {
                    RefTermId = Guid.Parse(src.Type.Key),
                    UserId = newId,
                    PhoneNumber = src.PhoneNumber
                }).ToList()
            };
            
                // saves new addressBook into database
                _addressBookRepository.SaveToDataBase(user);
            }
            catch
            {
                return null;
            }
            return newId;
            
        }

        //
        ///<summary>
        ///Deletes AddressBook from database
        ///</summary>
        ///<param name="id"></param>
        ///<returns>string</returns>
        public string  DeletAddressBook(Guid id)
        {
            //takes Args id Guid ,returns user Dto,checks the addressbook exists in the database
            User account = _addressBookRepository.GetAddressbook(id);
            if (account == null)
            {
                return null;
            }
            //takes account Dto,removes the addressBook from database
            _addressBookRepository.RemoveAccount(account);
            return string.Empty;
        }

        //
        ///<summary>
        ///uploads file into database in form of bytes
        ///</summary>
        ///<param name="file"></param>
        ///<returns>UploadResponseDTO</returns>
        public UploadResponseDTO UploadFile(IFormFile file)
        {
            MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);
            byte[] bytes = ms.ToArray();
            Asset fileObj = new Asset()
            {
                Id = Guid.NewGuid(),
                Field = bytes,
            };
            string addresses = _server.Features.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();
            UploadResponseDTO response = new UploadResponseDTO
            {
                Id = fileObj.Id,
                fileName = file.FileName,
                downloadURL = addresses + Constants.assetUrl+ fileObj.Id, 
                fileType = file.ContentType,
                
                size = file.Length,
                fileContent = null
            };
            // takes fileObj Model ,saves into database
            _addressBookRepository.SaveFileToDataBase(fileObj);

            return response;
        }

        
        ///<summary>
        /// Downloads file file from database
        ///</summary>
        ///<param name="id"></param>
        ///<returns>byte[]</returns>
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

        ///<summary>
        ///  returns AddressBook from database using id
        ///</summary>
        ///<param name="id"></param>
        ///<returns>UserDTO</returns>
        public UserDTO GetAddressBook(Guid id)
        {
            // takes Args id Guid ,returns user Model ,gets user model from database using id.
            User account =  _addressBookRepository.GetAddressbook(id);
            if (account == null)
            {
                return null;
            }
            
            UserDTO addressBook = _mapper.Map<UserDTO>(account);
            UserDTO addressBookObj = addressBook;
            foreach (string item in addressBook.Email.Select(s => s.Type.Key))
            {
                Guid sample;
                if (!Guid.TryParse(item, out sample))
                {
                    break;
                }
                string type = _addressBookRepository.GetType(sample);
                addressBookObj.Email.Where(src => src.Type.Key == item).Select(crt => { crt.Type.Key = type; return crt; }).ToList();
            }
            foreach (string item in addressBook.Phone.Select(s => s.Type.Key))
            {
                Guid sample;
                if (!Guid.TryParse(item, out sample))
                {
                    break;
                }
                string type = _addressBookRepository.GetType(Guid.Parse(item));
                addressBookObj.Phone.Where(src => src.Type.Key == item).Select(crt => { crt.Type.Key = type; return crt; }).ToList();
            }
            Guid countryKey = Guid.Parse(addressBook.Address.Select(s => s.Country.Key).FirstOrDefault());
            string getCountryType = _addressBookRepository.GetType(countryKey);
            addressBookObj.Address.Select(crt => crt.Country).Select(src => { src.Key = getCountryType; return src; }).ToList();

            Guid addressKey = Guid.Parse(addressBook.Address.Select(src => src.Type.Key).FirstOrDefault());
            string addressType = _addressBookRepository.GetType(addressKey);
            addressBookObj.Address.Select(crt => crt.Type).Select(src => { src.Key = addressType; return src; }).ToList();
            return addressBookObj;
        }

        // 
        ///<summary>
        /// saves new Admin logins into database 
        ///</summary>
        ///<param name="signupDTO"></param>
        ///<returns>string</returns>
        public string SignupAdmin(SignupDTO signupDTO)
        {
            string text = signupDTO.Password;
            // takes Args new login userName string ,return bool ,checks username exists in database
            bool isExists = _addressBookRepository.IsUserNameExists(signupDTO.UserName);
            if (isExists)
            {
                return null;
            }
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(this.key, this.iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            string password = Convert.ToBase64String(outputBuffer);
            Login loginObj = new Login { UserName = signupDTO.UserName, Password = password };
            _addressBookRepository.SinupAdmin(loginObj);
            return string.Empty;

        }
    }

}
