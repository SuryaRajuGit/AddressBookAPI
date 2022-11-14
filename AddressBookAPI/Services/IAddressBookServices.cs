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
        public Task<logInResponse> VerifyUser(logInModel logInModel);

        public Task<int> GetAddressBookCount();

        public Task<Guid?> DeletAddressBook(Guid id);

        public Task<Guid?> UpdateAddressBook(Guid id, userModel userModel);

        public Task<List<userModel>> GetAllAddressBooks(int size, string pageNo, string sortBy, string sortOrder);

        public Task<Tuple<string, string>> AddNewAddressBook(userModel UserModel);

        public Task<userModel> GetAddressBook(Guid id);

        public Task<UploadResponse> UploadFile(IFormFile file);

        public byte[] Download(Guid id);

        public int SignupAdmin(signupModel signupModel);

        public int Seed();
    }
}
