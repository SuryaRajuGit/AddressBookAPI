using AddressBookAPI.Data;
using AddressBookAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBookAPI.Services
{
    public interface IAddressBookServices
    {
        public Task<logInResponseDTO> VerifyUser(logInDTO logInModel);

        public int GetAddressBookCount();

        public Task<Guid?> DeletAddressBook(Guid id);

        public Task<Guid?> UpdateAddressBook(Guid id, userDTO userModel);

        public Task<List<userDTO>> GetAllAddressBooks(int size, string pageNo, string sortBy, string sortOrder);

        public Task<ErrorDTO> AddNewAddressBook(userDTO UserModel);

        public Task<userDTO> GetAddressBook(Guid id);

        public Task<UploadResponseDTO> UploadFile(IFormFile file);

        public byte[] Download(Guid id);

        public  Task<int?> SignupAdmin(signupDTO signupModel);

        public Task<Guid?> saveToDatabase(userDTO userModel);

    }
}
