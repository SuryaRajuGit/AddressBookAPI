using AddressBookAPI.Data;
using AddressBookAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AddressBookAPI.Services;
using Newtonsoft.Json;
using System.Security.Cryptography;
using CsvHelper;

namespace AddressBookAPI.Repository
{
    public class AddressBookRepository : IAddressBookRepository
    {
        private readonly AddressBookContext _context;

        public AddressBookRepository(AddressBookContext context)
        {
            _context = context;
        }
        public string loginDetails(string userName)
        {
            return  _context.Login.Where(s => s.userName == userName).Select(s => s.password).FirstOrDefault();
        }

        public IEnumerable<user> ListOfAccounts(string sortBy)
        {
             var accounts  =   sortBy == enumList.Constants.firstName.ToString() ?
                _context.User.Include(e => e.email).Include(a => a.address).Include(p => p.phone).OrderBy(s => s.firstName) :
                _context.User.Include(e => e.email).Include(a => a.address).Include(p => p.phone).OrderBy(s => s.lastName);
            return accounts;

        }
        public user GetAccountCount(Guid id)
        {
            return  _context.User.Include(e => e.email).Include(p => p.phone).Include(a => a.address).FirstOrDefault(s => s.Id == id);
        }

        public void UpdateToDataBase(user account)
        {
            _context.Update(account);
            _context.SaveChanges();
        }

        public  int SeedData(List<refTerm> data)
        {
            foreach (var item in data)
            {
                _context.Add(item);
            }
            _context.SaveChanges();
            return data.Count();
        }

        public int GetAddressBookCount()
        {
            
            return _context.User.Select(s => s.Id).Count();
        }

        public List<string> EmailList()
        {
            return  _context.Email.Select(s => s.emailAddress).ToList(); 
        }

        public List<string> PhoneList()
        {
            return _context.Phone.Select(s => s.phoneNumber).ToList();
        }

        public List<string> AdderessList()
        {
            var listOfAddresses = new List<string>();
            foreach (var item in _context.Address)
            {
                var addressObj = new addressModel
                {
                    line1 = item.line1,
                    line2 = item.line2,
                    city = item.city,
                    zipCode = item.zipCode,
                    stateName = item.stateName,
                    type = new typeModel {key= item.refTermId },
                    country = new typeModel { key = item.refTermId },  
                };
                string josnObj = JsonConvert.SerializeObject(addressObj);
                listOfAddresses.Add(josnObj);
            }
            return listOfAddresses;
        }
        public   void RemoveAccount(user account)
        {
             _context.Remove(account);
             _context.SaveChangesAsync();
        }

        public void  saveToDataBase(user account)
        {
            _context.Add(account);
            _context.SaveChanges();
            
        }
        public void saveFileToDataBase(asset fileObj)
        {      
            _context.Add(fileObj);
            _context.SaveChanges();       
        }

        public byte[] GetFile(Guid id)
        {
            return _context.AssetDTO.Where(f => f.Id == id).Select(s => s.field).FirstOrDefault();
        }

        public Task<user> GetAddressBook(Guid id)
        {
            return _context.User.Include(e => e.email).Include(a => a.address).Include(p => p.phone).Where(f => f.Id == id).FirstOrDefaultAsync();
        }

        public int SinupAdmin(Login sinupModel)
        {
             _context.Add(sinupModel);
            _context.SaveChanges();
            return  _context.Login.Where(w => w.userName == sinupModel.userName).Select(s => s.Id).FirstOrDefault();
        }
    }
}
