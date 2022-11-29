
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


            if (password == logInDTO.password)
            {
                JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();
                byte[] tokenKey = Encoding.UTF8.GetBytes(Constants.secutityKey);

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

        //
        ///<summary>
        ///returns all AddressBooks from database
        ///</summary>
        ///<param name="size">size of file ></param>
        ///<param name="pageNo">pageno  of file ></param>
        ///<param name="sortBy">sort by fist name or lastname of file ></param>
        ///<param name="sortOrder">sort by ascending or descending  of file ></param>
        ///<returns>list of AddressBooks</returns>
        public List<UserDTO> GetAllAddressBooks(int size, int pageNo, string sortBy, string sortOrder)
        {
            // takes Args size Int,pageNo Int ,returns list of AddressBooks after applying pagenation
            List<user> paginatedList = _addressBookRepository.Pageination(size, pageNo);
            IEnumerable<user> sortList;
            if (paginatedList == null)
            {
                return null;
            }
            
            if (sortOrder == Constants.DSC)
            {
                sortList = sortBy == Constants.firstName ? paginatedList.OrderBy(s => s.firstName) : paginatedList.OrderBy(s => s.lastName);
                sortList = sortBy == Constants.firstName ? sortList.OrderByDescending(s => s.firstName) : sortList.OrderByDescending(s => s.lastName);
                  
            }
            else
            {
                sortList = sortBy == Constants.firstName ? paginatedList.OrderBy(s => s.firstName) : paginatedList.OrderBy(s => s.lastName);
            }

            List<UserDTO> accountsList = new List<UserDTO>();
            foreach (user i in sortList)
            {
                UserDTO user = _mapper.Map<UserDTO>(i);

               
                foreach (string item in user.Email.Select(s => s.type.key))
                {
                    Guid sample;
                    if (Guid.TryParse(item, out sample))
                    {
                        break;
                    }
                    string type = _addressBookRepository.GetType(sample);
                    user.Email.Where(s => s.type.key == item).Select(c => { c.type.key = type; return c; }).ToList();
                }
                foreach (string item in user.Phone.Select(s => s.type.key))
                {
                    Guid sample;
                    if (Guid.TryParse(item, out sample))
                    {
                        break;
                    }
                    string type = _addressBookRepository.GetType(sample);
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

        ///<summary>
        ///checks if the entered emails are same or not 
        ///</summary>
        ///<param name="emailDTOs"></param>
        ///<returns>bool value</returns>
        public bool isMatched(ICollection<EmailDTO> emailDTOs)
        {
            HashSet<Guid> emialTypeHashSet = new HashSet<Guid>();
            HashSet<string> emialHashSet = new HashSet<string>();
            foreach (EmailDTO item in emailDTOs.Select(s => s))
            {
                emialTypeHashSet.Add(Guid.Parse(item.type.key));
                emialHashSet.Add(item.emailAddress);
            }
            if (emialTypeHashSet.Count() != emailDTOs.Select(s => s.type.key).Count())
            {
                if (emialHashSet.Count() != emailDTOs.Select(s => s.emailAddress).Count())
                {
                    return true;//new ErrorDTO { type = "Conflict", description = "both entered Emails are same" };
                }

            }
            return false;
            
        }

        ///<summary>
        ///checks if the entered phone no are same or not 
        ///</summary>
        ///<param name="phoneDTOs"></param>
        ///<returns>bool value</returns>
        public bool isMatchedphone(ICollection<PhoneDTO> phoneDTOs)
        {
            HashSet<Guid> phoneTypeHashSet = new HashSet<Guid>();
            HashSet<string> phoneHashSet = new HashSet<string>();
            foreach (PhoneDTO item in phoneDTOs.Select(s => s))
            {
                phoneTypeHashSet.Add(Guid.Parse(item.type.key));
                phoneHashSet.Add(item.phoneNumber);
            }
            if (phoneTypeHashSet.Count() != phoneDTOs.Select(s => s.type.key).Count())
            {
                if (phoneHashSet.Count() != phoneDTOs.Select(s => s.phoneNumber).Count())
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
            user account = _addressBookRepository.GetAccount(id, userDTO);
            if(account == null)
            {
                return new ErrorDTO { type = "AddressBook", description = "AddressBook not found " };
            }
            try
            {
                account.firstName = userDTO.firstName;
            account.lastName = userDTO.lastName;
            account.email = userDTO.Email.Select(s => new email
            {
                emailAddress = s.emailAddress,
                refTermId = Guid.Parse(s.type.key),
            }).ToList();


            account.address = userDTO.Address.Select(s => new address
            {
                refTermId = Guid.Parse(s.type.key),
                line1 = s.line1,
                line2 = s.line2,
                stateName = s.stateName,
                city = s.city,
                zipCode = s.zipCode,
                country = Guid.Parse(s.country.key),
            }).ToList();
            account.phone = userDTO.Phone.Select(s =>
                new phone
                {
                    refTermId = Guid.Parse(s.type.key),
                    phoneNumber = s.phoneNumber
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
        public ErrorDTO modelStateInvaliLoginAPI(ModelStateDictionary ModelState)
        {
            return new ErrorDTO
            {
                type = ModelState.Keys.Select(s => s).FirstOrDefault(),
                description = ModelState.Values.Select(s => s.Errors[0].ErrorMessage).FirstOrDefault()
            };
        }
        // 
        ///<summary>
        ///validation attribute error 
        ///</summary>
        ///<param name="ModelState"></param>
        ///<returns>ErrorDTO</returns>
        public ErrorDTO modelStateInvalidSinupAPI(ModelStateDictionary ModelState)
        {
            return new ErrorDTO
            {
                type = ModelState.Keys.FirstOrDefault(),
                description =
                ModelState.Values.Select(s => s.Errors.Select(s => s.ErrorMessage).FirstOrDefault()).FirstOrDefault()
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
            try
            {
                bool isEmailDup = isMatched(email);
                if(isEmailDup)
                {
                    return  new ErrorDTO { type = "email", description = "both entered Emails are same" };
                }

            }
            catch
            {
                return new ErrorDTO { type = "meta-data", description = "meat-data not exists in database" };
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
            try
            {
                bool isPhoneDup = isMatchedphone(phone);
                if (isPhoneDup)
                {
                    return new ErrorDTO { type = "phone", description = "both entered Phone no are same" };
                }
            }
            catch
            {
                return  new ErrorDTO{type="meta-data", description = "meta-data do not exist in database"};
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
        public List<RefSetResponseDto> getRefSetData(string key)
        {

            List<refTerm> refTermList = _addressBookRepository.getRefSetData(key);
            if(refTermList == null)
            {
                return null;
            }
            List<RefSetResponseDto> responseList = new List<RefSetResponseDto>();
            foreach (refTerm item in refTermList)
            {
                RefSetResponseDto response = new RefSetResponseDto
                {
                    id = item.Id,
                    key = item.key,
                    description = item.description
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
        public ErrorDTO modelStateInvalid(ModelStateDictionary modelState)
        {
            return new ErrorDTO
            {
                type = modelState.Select(s => s).Where(s => s.Value.ValidationState.ToString() == Constants.invalid).Select(s => s.Key).FirstOrDefault(),
                
                description = modelState.Values.Where(w => w.ValidationState.ToString() == Constants.invalid).
                Select(s => s.Errors[0].ErrorMessage).FirstOrDefault()
            
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
        public Guid? saveToDatabase(UserDTO userDTO)
        {
            Guid newId = Guid.NewGuid();
            try
            {
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
                    stateName = s.stateName,
                    city = s.city,
                    zipCode = s.zipCode
                }).ToList(),

                phone = userDTO.Phone.Select(s =>
                new phone
                {
                    refTermId = Guid.Parse(s.type.key),
                    userId = newId,
                    phoneNumber = s.phoneNumber
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
            user account = _addressBookRepository.GetAddressbook(id);
            if (account == null)
            {
                return null;
            }
            //takes account Dto,removes the addressBook from database
            _addressBookRepository.RemoveAccount(account);
            return "";
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
            user account =  _addressBookRepository.GetAddressbook(id);
            if (account == null)
            {
                return null;
            }
            
            UserDTO addressBook = _mapper.Map<UserDTO>(account);
            UserDTO addressBookObj = addressBook;
            foreach (string item in addressBook.Email.Select(s => s.type.key))
            {
                Guid sample;
                if (Guid.TryParse(item, out sample))
                {
                    break;
                }
                string type = _addressBookRepository.GetType(sample);
                addressBookObj.Email.Where(s => s.type.key == item).Select(c => { c.type.key = type; return c; }).ToList();
            }
            foreach (string item in addressBook.Phone.Select(s => s.type.key))
            {
                Guid sample;
                if (Guid.TryParse(item, out sample))
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

        // 
        ///<summary>
        /// saves new Admin logins into database 
        ///</summary>
        ///<param name="signupDTO"></param>
        ///<returns>string</returns>
        public string SignupAdmin(SignupDTO signupDTO)
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
            _addressBookRepository.SinupAdmin(loginObj);
            return "";

        }
    }

}
