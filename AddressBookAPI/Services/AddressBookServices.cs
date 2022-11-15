using AddressBookAPI.Data;
using AddressBookAPI.Models;
using AddressBookAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookAPI.Services
{
    public class AddressBookServices : IAddressBookServices
    {

        private readonly IConfiguration _configuration;
        private readonly IAddressBookRepository _addressBookRepository;

        byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

        public AddressBookServices(IConfiguration configuration, IAddressBookRepository addressBookRepository)
        {

            _configuration = configuration;
            _addressBookRepository = addressBookRepository;
        }

        public string? ValidateEmail(userDTO userModel)
        {
            foreach (emailDTO item in userModel.Email)
            {
                string validEmail = item.emailAddress;
                try
                {
                    MailAddress mail = new MailAddress(validEmail);
                    bool isTrue =  mail.Host.Contains(".");
                    return isTrue ? null : validEmail;
                }
                catch (Exception)
                {
                    return validEmail; 
                }
            }
            return null;
        }

        public string ValidatePhone(userDTO userModel)
        {
            foreach (phoneDTO item in userModel.Phone) 
            { 
                if (!(item.phoneNumber.Length == 10))
                {
                    return item.phoneNumber; //BadRequest(new validationErrorModel { type = "Phone", description = "Invalid Phone Number : " + item.phoneNumber });
                }
            }
            return null;
        }

        public string PasswordValidations(signupDTO sinupModel)
        {
            if (!sinupModel.password.Any(char.IsUpper))
            {
                return enumList.Constants.upperCase.ToString(); //BadRequest(new validationErrorModel { type = "Upper case Exception", description = "One Uppercase Required" });
            }
            if (sinupModel.password.Contains(" "))
            {
                return enumList.Constants.space.ToString();//BadRequest(new validationErrorModel { description = "Do not Contain spaces" });
            }
            if (sinupModel.password.Length < 6)
            {
                return enumList.Constants.length.ToString(); //BadRequest(new validationErrorModel { description = "Length of password shoud be minimum 6" });
            }
            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] specialh = specialCh.ToCharArray();
            int i;
            for (i = 0; i < sinupModel.password.Length; i++)
            {
                if (specialCh.Contains(sinupModel.password[i]))
                {
                    break;
                }
            }
            if (i == sinupModel.password.Length)
            {
                return enumList.Constants.specialCharacter.ToString();
            }
            return null;
}

public async Task<logInResponseDTO> VerifyUser(logInDTO logInModel)
        {

            string userPassword = _addressBookRepository.loginDetails(logInModel.userName);
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


            if (password == logInModel.Password)
            {
                var tokenhandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

                var tokenDeprictor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name, logInModel.userName) }
                        ),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenhandler.CreateToken(tokenDeprictor);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                var response = new logInResponseDTO()
                {
                    jwt = tokenString,
                    type = "Bearer"
                };
                return response;
            }
            return null;

        }
        public async Task<List<userDTO>> GetAllAddressBooks(int size, string pageNo, string sortBy, string sortOrder)
        {
            var accounts = _addressBookRepository.ListOfAccounts(sortBy);
            if (accounts == null)
            {
                return null;
            }
            int count = 0;
            var accountsList = new List<userDTO>();
            foreach (var item in accounts)
            {
                var user = AddressBookMaping(item);
                accountsList.Add(user);
                count = count + 1;
                if (size == count)
                {
                    break;
                }
            };
            return accountsList;
        }

        public async Task<Guid?> UpdateAddressBook(Guid id, userDTO userModel)
        {
            var account = _addressBookRepository.GetAccountCount(id);
            if (account == null)
            {
                return null;
            }

            account.firstName = userModel.firstName;
            account.lastName = userModel.lastName;
            var x = account.email.Count;
            account.email = userModel.Email.Select(s => new email
            {
                userId = id,
                emailAddress = s.emailAddress,
                refTermId = s.type.key,
            }).ToList();


            account.address = userModel.Address.Select(s => new address
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
            account.phone = userModel.Phone.Select(s =>
                new phone
                {
                    refTermId = s.type.key,
                    phoneNumber = s.phoneNumber
                }).ToList().ToList();
            _addressBookRepository.UpdateToDataBase(account);
            return id;

        }
        public int GetAddressBookCount()
        {
            return _addressBookRepository.GetAddressBookCount();
        }
        public async Task<Tuple<string, string>> AddNewAddressBook(userDTO UserModel)
        {

            var emailList = _addressBookRepository.EmailList();
            var phoneList = _addressBookRepository.PhoneList();

            foreach (var email in UserModel.Email)
            {
                if (emailList.Contains(email.emailAddress))
                {
                    return Tuple.Create(enumList.Constants.email.ToString(), email.emailAddress);
                }
            }
            foreach (var phone in UserModel.Phone)
            {
                if (phoneList.Contains(phone.phoneNumber))
                {
                    return Tuple.Create(enumList.Constants.phone.ToString(), phone.phoneNumber);

                }
            }
            var addressList = _addressBookRepository.AdderessList();
            foreach (var item in UserModel.Address)
            {
                var jsonobj = JsonConvert.SerializeObject(item);
                if (addressList.Contains(jsonobj))
                {
                    return Tuple.Create(enumList.Constants.address.ToString(), jsonobj);
                }
            }

            var newId = Guid.NewGuid();

            var userModel = new user()
            {
                Id = newId,
                firstName = UserModel.firstName,
                lastName = UserModel.lastName,

                email = UserModel.Email.Select(s =>
                   new email
                   {
                       emailAddress = s.emailAddress,
                       userId = newId,
                       refTermId = s.type.key,
                   }).ToList(),

                address = UserModel.Address.Select(s =>
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

                phone = UserModel.Phone.Select(s =>
                new phone
                {
                    refTermId = s.type.key,
                    userId = newId,
                    phoneNumber = s.phoneNumber
                }).ToList()
            };

            try
            {
                _addressBookRepository.SaveToDataBase(userModel);

                return Tuple.Create(newId.ToString(), string.Empty); ;
            }
            catch
            {
                return Tuple.Create(string.Empty, string.Empty);
            }
            return null;

        }

        public async Task<Guid?> DeletAddressBook(Guid id)
        {

            var account = _addressBookRepository.GetAccountCount(id);
            if (account == null)
            {
                return null;
            }
            _addressBookRepository.RemoveAccount(account);
            return id;

        }

        public async Task<UploadResponseDTO> UploadFile(IFormFile file)
        {

            MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);

            var bytes = ms.ToArray();
            var fileObj = new asset()
            {
                Id = Guid.NewGuid(),
                field = bytes,

            };

            var response = new UploadResponseDTO
            {
                Id = fileObj.Id,
                fileName = file.FileName,
                downloadURL = "http://localhost:6162/api/asset/downloadFile/" + fileObj.Id,
                fileType = file.ContentType,
                size = file.Length,
                fileContent = null
            };
            _addressBookRepository.SaveFileToDataBase(fileObj);

            return response;
        }
        public byte[] Download(Guid id)
        {
            var bytes = _addressBookRepository.GetFile(id);
            if (bytes == null)
            {
                return null;
            }
            return bytes;
        }

        public async Task<userDTO> GetAddressBook(Guid id)
        {
            var account = await _addressBookRepository.GetAddressBook(id);
            if (account == null)
            {
                return null;
            }
            var response = AddressBookMaping(account);
            return response;
        }

        public userDTO AddressBookMaping(user user)
        {
            var users =
                new userDTO()
                {
                    Id = user.Id,
                    firstName = user.firstName,
                    lastName = user.lastName,
                    Email = user.email.Select(s =>
                      new emailDTO
                      {
                          emailAddress = s.emailAddress,
                          type = new typeDTO { key = s.refTermId }
                      }).ToList(),

                    Address = user.address.Select(s =>
                      new addressDTO
                      {
                          line1 = s.line1,
                          line2 = s.line2,
                          type = new typeDTO { key = s.refTermId },
                          stateName = s.stateName,
                          city = s.city,
                          country = new typeDTO { key = s.refTermId },
                          zipCode = s.zipCode
                      }).ToList(),

                    Phone = user.phone.Select(s =>
                      new phoneDTO
                      {
                          type = new typeDTO { key = s.refTermId },
                          phoneNumber = s.phoneNumber
                      }).ToList()
                };
            return users;
        }

        public int? SignupAdmin(signupDTO signupModel)
        {

            string text = signupModel.password;
            bool isExists = _addressBookRepository.isUserNameExists(signupModel.userName);
            if (isExists)
            {
                return null;
            }
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(this.key, this.iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            var password = Convert.ToBase64String(outputBuffer);
            var loginObj = new Login { userName = signupModel.userName, password = password };
            return _addressBookRepository.SinupAdmin(loginObj);

        }
    }

}
