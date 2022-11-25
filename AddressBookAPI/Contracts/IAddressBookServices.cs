
using AddressBookAPI.Entity.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AddressBookAPI.Services
{
    public interface IAddressBookServices
    {
        public logInResponseDTO VerifyUser(LogInDTO logInDTO);

        

        public int GetAddressBookCount();

        public string DeletAddressBook(Guid id);

        public ErrorDTO UpdateAddressBook(Guid id, UserDTO userDTO);

        public ErrorDTO Duplicates(UserDTO userDTO);

        public List<UserDTO> GetAllAddressBooks(int size, int pageNo, string sortBy, string sortOrder);

        public ErrorDTO modelStateInvalid(ModelStateDictionary ModelState);

        public ErrorDTO modelStateInvalidSinupAPI(ModelStateDictionary ModelState);

        public ErrorDTO modelStateInvaliLoginAPI(ModelStateDictionary ModelState);

        public UserDTO GetAddressBook(Guid id);

        public UploadResponseDTO UploadFile(IFormFile file );

        public byte[] Download(Guid id);

        public  string SignupAdmin(SignupDTO signupDTO);

        public Guid? saveToDatabase(UserDTO userDTO);

        public ErrorDTO EmailExists(ICollection<EmailDTO> email);

        public ErrorDTO PhoneExists(ICollection<PhoneDTO> phone);

        public ErrorDTO AddressExists(ICollection<AddressDTO> address);
    }
}
