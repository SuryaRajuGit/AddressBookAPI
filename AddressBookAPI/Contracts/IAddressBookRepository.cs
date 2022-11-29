
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

        public string loginDetails(string userName );

        public user GetAccount(Guid id, UserDTO userDTO);

        public void UpdateToDataBase(user account);

        public int GetAddressBookCount();

        public string EmailList(ICollection<EmailDTO> email,Guid id);

        public List<refTerm> getRefSetData(string key);

        public string PhoneList(ICollection<PhoneDTO> phone, Guid id);

        public string AdderessList(ICollection<AddressDTO> address,Guid id);

        public void  RemoveAccount(user account);

        public user GetAddressbook(Guid id);

        public void SaveToDataBase(user account);

        public void SaveFileToDataBase(asset fileObj);

        public byte[] GetFile(Guid id);

        public void SinupAdmin(login sinupModel);

        public bool isUserNameExists(string text);

        public List<user> Pageination(int size,int pageNo);


    }
}
