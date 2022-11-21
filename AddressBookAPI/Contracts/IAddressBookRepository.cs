using AddressBookAPI.Data;
using AddressBookAPI.Models;
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
        public string loginDetails(string userName );

        public IEnumerable<user> ListOfAccounts(string sortBy);

        public user GetAccountCount(Guid id);

        public void UpdateToDataBase(user account);

        public int GetAddressBookCount();

        public string EmailList(ICollection<EmailDTO> email);


        public string PhoneList(ICollection<PhoneDTO> phone);

        public string AdderessList(ICollection<AddressDTO> address);

        public void  RemoveAccount(user account);

        public void SaveToDataBase(user account);

        public void SaveFileToDataBase(asset fileObj);

        public byte[] GetFile(Guid id);

        public user GetAddressBook(Guid id);

        public int SinupAdmin(login sinupModel);

        public bool isUserNameExists(string text);

        public List<user> Pagenation(int size,int pageNo);


    }
}
