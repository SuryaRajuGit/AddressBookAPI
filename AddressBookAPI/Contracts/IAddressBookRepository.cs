
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
        public string GetType(Guid item);

        public bool IsCountryExixsted(string type);

        public string loginDetails(string userName );

        public User GetAccount(Guid id, UserDTO userDTO);

        public void UpdateToDataBase(User account);

        public int GetAddressBookCount();

        public string EmailList(ICollection<EmailDTO> email,Guid id);

        public List<RefTerm> GetRefSetData(string key);

        public string PhoneList(ICollection<PhoneDTO> phone, Guid id);

        public string AdderessList(ICollection<AddressDTO> address,Guid id);

        public void  RemoveAccount(User account);

        public User GetAddressbook(Guid id);

        public void SaveToDataBase(User account);

        public void SaveFileToDataBase(Asset fileObj);

        public byte[] GetFile(Guid id);

        public void SinupAdmin(Login sinupModel);

        public bool IsUserNameExists(string text);

        public List<User> GetPageinatedList(int size,int pageNo);


    }
}
