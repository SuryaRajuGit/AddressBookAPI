using AddressBookAPI.Data;

using AddressBookAPI.Models;
using Microsoft.AspNetCore.Http;
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

        public void UpdateAddressBook(Guid id, UserDTO userDTO);

        public List<UserDTO> GetAllAddressBooks(int size, int pageNo, string sortBy, string sortOrder);


        public UserDTO GetAddressBook(Guid id);

        public UploadResponseDTO UploadFile(IFormFile file );


        public byte[] Download(Guid id);

        public  int? SignupAdmin(SignupDTO signupDTO);

        public Guid? saveToDatabase(UserDTO userDTO);

        public string EmailExists(ICollection<EmailDTO> email);

        public string PhoneExists(ICollection<PhoneDTO> phone);

        public string AddressExists(ICollection<AddressDTO> address);


    }
}
