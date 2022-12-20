
using AddressBookAPI.Entity.Dto;
using AddressBookAPI.Entity.Models;
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
        ///<summary>
        ///verifies user login details and returns logInResponseDTO Dto
        ///</summary>
        ///<param name="logInDTO">Guid of file ></param>
        ///<returns>logInResponseDTO</returns>
        public LogInResponseDTO VerifyUser(LogInDTO logInDTO);

        ///<summary>
        ///Gets count of AddressBooks from database
        ///</summary>
        ///<returns>int</returns>
        public int GetAddressBookCount();

        ///<summary>
        ///Deletes AddressBook from database
        ///</summary>
        ///<param name="id"></param>
        ///<returns>string</returns>
        public string DeletAddressBook(Guid id);

        ///<summary>
        ///updates existing  AddressBook based on userId
        ///</summary>
        ///<param name="id"></param>
        /// ///<param name="userDTO"></param>
        ///<returns>ErrorDTO</returns>
        public ErrorDTO UpdateAddressBook(Guid id, UserDTO userDTO);

        ///<summary>
        ///returns all AddressBooks from database
        ///</summary>
        ///<param name="size">size of file ></param>
        ///<param name="pageNo">pageno  of file ></param>
        ///<param name="sortBy">sort by fist name or lastname of file ></param>
        ///<param name="sortOrder">sort by ascending or descending  of file ></param>
        ///<returns>list of AddressBooks</returns>
        public List<UserDTO> GetAllAddressBooks(int size, int pageNo, string sortBy, string sortOrder);

        ///<summary>
        ///returns list of meta-data 
        ///</summary>
        ///<param name="modelState"></param>
        ///<returns>ErrorDTO</returns>
        public ErrorDTO ModelStateInvalid(ModelStateDictionary ModelState);

        ///<summary>
        ///validation attribute error 
        ///</summary>
        ///<param name="ModelState"></param>
        ///<returns>ErrorDTO</returns>
        public ErrorDTO ModelStateInvalidSinupAPI(ModelStateDictionary ModelState);

        ///<summary>
        ///validation attribute error
        ///</summary>
        ///<param name="ModelState"></param>
        ///<returns>ErrorDTO</returns>
        public ErrorDTO ModelStateInvaliLoginAPI(ModelStateDictionary ModelState);

        ///<summary>
        ///  returns AddressBook from database using id
        ///</summary>
        ///<param name="id"></param>
        ///<returns>UserDTO</returns>
        public UserDTO GetAddressBook(Guid id);

        ///<summary>
        ///uploads file into database in form of bytes
        ///</summary>
        ///<param name="file"></param>
        ///<returns>UploadResponseDTO</returns>
        public UploadResponseDTO UploadFile(IFormFile file );

        ///<summary>
        /// Downloads file file from database
        ///</summary>
        ///<param name="id"></param>
        ///<returns>byte[]</returns>
        public byte[] Download(Guid id);

        ///<summary>
        /// saves new Admin logins into database 
        ///</summary>
        ///<param name="signupDTO"></param>
        ///<returns>string</returns>
        public string SignupAdmin(SignupDTO signupDTO);

        ///<summary>
        ///Saves new AddressBook into database 
        ///</summary>
        ///<param name="userDTO"></param>
        ///<returns>Guid</returns>
        public Guid? SaveToDatabase(UserDTO userDTO);

        ///<summary>
        ///returns list of meta-data 
        ///</summary>
        ///<param name="key"></param>
        ///<returns>ErrorDTO</returns>
        public List<RefSetResponseDto> GetRefSetData(string key);

        ///<summary>
        ///Checks email exists in the database and return string 
        ///</summary>
        ///<returns>ErrorDTO</returns>
        public ErrorDTO EmailExists(ICollection<EmailDTO> email,Guid id);

        ///<summary>
        ///Checks phone no exists in the database and return string
        ///</summary>
        ///<param name="phone"></param>
        /// ///<param name="id"></param>
        ///<returns>ErrorDTO</returns>
        public ErrorDTO PhoneExists(ICollection<PhoneDTO> phone,Guid id);

        ///<summary>
        /// Checks address exists in the database and return string
        ///</summary>
        ///<param name="addresses"></param>
        ///<param name="id"></param>
        ///<returns>ErrorDTO</returns>
        public ErrorDTO AddressExists(ICollection<AddressDTO> address,Guid id);
    }
}
