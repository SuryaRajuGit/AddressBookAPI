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
        //takes Args username string ,returns string ,checks username exists in database
        public string loginDetails(string userName)
        {
            return _context.Login.Where(s => s.userName == userName).Select(s => s.password).FirstOrDefault();
            
        }
        //takes Args username string ,returns bool ,checks username exists in database
        public bool isUserNameExists(string text)
        {
            List<string> listOfAdmins =  _context.Login.Select(s => s.userName).ToList();
            return listOfAdmins.Contains(text);
        }

        //takes Args size Int,pageNo Int ,returns List<user> ,Applys Pagenation 
        public List<user> Pagenation( int size, int pageNo)
        {
            List<user> listOfUsers  = _context.User.Include(e => e.email).Include(p => p.phone).Include(a => a.address)
              .Skip((pageNo - 1) * 5)
              .Take(size)
              .ToList();

            return listOfUsers;
        }

        //takes Args sortBy string,returns IEnumerable<user> ,gets all AddressBooks sorted by  firstname or lastname
        public IEnumerable<user> ListOfAccounts(string sortBy)
        {
            IEnumerable<user> accounts = sortBy == enumList.Constants.firstName.ToString() ?
               _context.User.Include(e => e.email).Include(a => a.address).Include(p => p.phone).OrderBy(s => s.firstName) :
               _context.User.Include(e => e.email).Include(a => a.address).Include(p => p.phone).OrderBy(s => s.lastName);
           
            return accounts;

        }
        // returns user Model  
        public user GetAccountCount(Guid id)
        {
            return  _context.User.Include(e => e.email).Include(p => p.phone).Include(a => a.address).FirstOrDefault(s => s.Id == id);
        }

        //saves user Dto into database 
        public void UpdateToDataBase(user account)
        {
            _context.Update(account);
            _context.SaveChanges();
        }

        // returns count of addressBook count
        public int GetAddressBookCount()
        {
            return _context.User.Include(s =>s.email).Include(a => a.address).Include(p => p.phone).Select(s => s.Id).Count();
        }

        //  takes Args as Email and checks in database ,returns string 
        public string EmailList(ICollection<EmailDTO> email)
        {
            foreach (string item in email.Select(s => s.emailAddress))
            {
                email userEmail = _context.Email.Where(w => w.emailAddress == item).FirstOrDefault();
                if(userEmail != null)
                {
                    return item;
                }    
            }
            return null;
        }

        //  takes Args as Phone and checks in database ,returns string
        public string PhoneList(ICollection<PhoneDTO> phone)
        {
            foreach (string item in phone.Select(s => s.phoneNumber))
            {
                phone userPhone = _context.Phone.Where(w => w.phoneNumber == item).FirstOrDefault();
                if (userPhone != null)
                {
                    return item;
                }
            }
            return null;
        }

        //  takes Args as Address and checks in database ,returns string
        public string AdderessList(ICollection<AddressDTO> addresses)
        {
            foreach (AddressDTO item in addresses)
            {
                string josnObjj = JsonConvert.SerializeObject(item);
                foreach (address i in _context.Address)
                {
                    AddressDTO addressObj = new AddressDTO
                    {
                        line1 = i.line1,
                        line2 = i.line2,
                        city = i.city,
                        zipCode = i.zipCode,
                        stateName = i.stateName,
                        type = new TypeDTO { key = i.refTermId },
                        country = new TypeDTO { key = i.refTermId },
                    };
                    string josnObj = JsonConvert.SerializeObject(addressObj);
                    if (josnObj == josnObjj)
                    {
                        return josnObj;
                    }
                }
            }
            return null;
        }
        
        //Delets addressBook from database
        public void RemoveAccount(user account)
        {
             _context.Remove(account);
             _context.SaveChangesAsync();
        }

        //saves AdderssBoook to database
        public void  SaveToDataBase(user account)
        {
            _context.Add(account);
            _context.SaveChanges();
        }
        //saves file to database
        public void SaveFileToDataBase(asset fileObj)
        {      
            _context.Add(fileObj);
            _context.SaveChanges();       
        }

        //returns file converitng into byte[] 
        public byte[] GetFile(Guid id)
        {
            return _context.AssetDTO.Where(f => f.Id == id).Select(s => s.field).FirstOrDefault();
        }

        // returns user Model ,using Id.
        public user GetAddressBook(Guid id)
        {
            return  _context.User.Include(e => e.email).Include(a => a.address).Include(p => p.phone).Where(f => f.Id == id).FirstOrDefault();
            
        }

        //saves new Admin login details into database
        public int SinupAdmin(login sinupModel)
        {
             _context.Add(sinupModel);
            _context.SaveChanges();
            return  _context.Login.Where(w => w.userName == sinupModel.userName).Select(s => s.Id).FirstOrDefault();
        }
    }
}
