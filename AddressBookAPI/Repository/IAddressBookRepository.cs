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

        public List<string> EmailList();

        public List<string> PhoneList();

        public List<string> AdderessList();

        public void  RemoveAccount(user account);

        public void SaveToDataBase(user account);

        public void SaveFileToDataBase(asset fileObj);

        public byte[] GetFile(Guid id);

        public Task<user> GetAddressBook(Guid id);

        public int SinupAdmin(Login sinupModel);

        public bool isUserNameExists(string text);


    }
}
