
using AddressBookAPI.Entity.Dto;
using AddressBookAPI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AddressBookAPI.Repository
{
    public  interface IAddressBookRepository
    {
        ///<summary>
        /// returns type 
        ///</summary>
        ///<param name="id"></param>
        ///<returns>string</returns>
        public string GetType(Guid item);

        ///<summary>
        /// returns bool value if the country exists
        ///</summary>
        ///<param name="type"></param>
        ///<returns>bool</returns>
        public bool IsCountryExisted(string type);

        ///<summary>
        /// takes Args username string ,returns string ,checks username exists in database
        ///</summary>
        ///<param name="userName"></param>
        ///<returns>string</returns>
        public string loginDetails(string userName );

        ///<summary>
        ///returns AddressBook
        ///</summary>
        ///<param name="id"></param>
        ///<param name="userDTO"></param>
        ///<returns>user</returns>
        public User GetAccount(Guid id, UserDTO userDTO);

        ///<summary>
        ///saves user Dto into database
        ///</summary>
        ///<param name="account"></param>
        public void UpdateToDataBase(User account);

        ///<summary>
        ///returns count of addressBook count 
        ///</summary>
        ///<returns>int</returns>
        public int GetAddressBookCount();

        ///<summary>
        /// takes Args as Email and checks in database ,returns string 
        ///</summary>
        ///<param name="email"></param>
        ///<param name="id"></param>
        ///<returns>string</returns>
        public string EmailList(ICollection<EmailDTO> email,Guid id);

        ///<summary>
        ///returns list of refterm data 
        ///</summary>
        ///<param name="key"></param>
        ///<returns>List<refTerm></returns>
        public List<RefTerm> GetRefSetData(string key);

        ///<summary>
        /// takes Args as Phone and checks in database ,returns string
        ///</summary>
        ///<param name="phone"></param>
        ///<param name="id"></param>
        ///<returns>string</returns>
        public string PhoneList(ICollection<PhoneDTO> phone, Guid id);

        ///<summary>
        /// takes Args as Address and checks in database ,returns string
        ///</summary>
        ///<param name="addresses"></param>
        ///<param name="id"></param>
        ///<returns>string</returns>
        public string AdderessList(ICollection<AddressDTO> address,Guid id);

        ///<summary>
        /// Delets addressBook from database
        ///</summary>
        ///<param name="account"></param>
        public void  RemoveAccount(User account);

        ///<summary>
        ///returns AddressBook
        ///</summary>
        ///<param name="id"></param>
        ///<returns>user</returns>
        public User GetAddressbook(Guid id);

        ///<summary>
        /// saves AdderssBoook to database
        ///</summary>
        ///<param name="account"></param>
        public void SaveToDataBase(User account);

        ///<summary>
        /// saves file to database
        ///</summary>
        ///<param name="fileObj"></param>
        public void SaveFileToDataBase(Asset fileObj);

        ///<summary>
        /// returns bytes of file 
        ///</summary>
        ///<param name="id"></param>
        ///<returns>byte[]<refTerm></returns>
        public byte[] GetFile(Guid id);

        ///<summary>
        ///saves new Admin login details into database
        ///</summary>
        ///<param name="sinupModel"></param>
        public void SinupAdmin(Login sinupModel);

        ///<summary>
        /// takes Args username string ,returns bool ,checks username exists in database
        ///</summary>
        ///<param name="text"></param>
        ///<returns>bool</returns>
        public bool IsUserNameExists(string text);

        /// takes Args size Int,pageNo Int ,returns List<user> ,Applys Pagenation 
        ///</summary>
        ///<param name="size"></param>
        ///<param name="pageNo"></param>
        ///<returns>List of addressBooks</returns>
        public List<User> GetPaginatedList(int size,int pageNo);


    }
}
