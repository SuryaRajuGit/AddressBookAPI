
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
using AddressBookAPI.Entity.Dto;
using AddressBookAPI.Entity.Models;

namespace AddressBookAPI.Repository
{
    public class AddressBookRepository : IAddressBookRepository
    {
        private readonly AddressBookContext _context;

        public AddressBookRepository(AddressBookContext context)
        {
            _context = context;

            
            
        }
        //
        ///<summary>
        /// takes Args username string ,returns string ,checks username exists in database
        ///</summary>
        ///<param name="userName"></param>
        ///<returns>string</returns>
        public string loginDetails(string userName)
        {
            return _context.Login.Where(s => s.userName == userName).Select(s => s.password).FirstOrDefault();
            
        }
        //

        ///<summary>
        /// takes Args username string ,returns bool ,checks username exists in database
        ///</summary>
        ///<param name="text"></param>
        ///<returns>bool</returns>
        public bool isUserNameExists(string text)
        {
            List<string> listOfAdmins =  _context.Login.Select(s => s.userName).ToList();
            return listOfAdmins.Contains(text);
        }

        //
        /// takes Args size Int,pageNo Int ,returns List<user> ,Applys Pagenation 
        ///</summary>
        ///<param name="size"></param>
        ///<param name="pageNo"></param>
        ///<returns>List of addressBooks</returns>
        public List<user> Pageination( int size, int pageNo)
        {
            List<user> listOfUsers  = _context.User.Include(e => e.email).Include(p => p.phone).Include(a => a.address)
              .Skip((pageNo - 1) * 5)
              .Take(size)
              .ToList();

            return listOfUsers;
        }

        ///<summary>
        ///returns AddressBook
        ///</summary>
        ///<param name="id"></param>
        ///<param name="userDTO"></param>
        ///<returns>user</returns>
        public user GetAccount(Guid id, UserDTO userDTO)
        {

            return  _context.User.Include(e => e.email).Include(p => p.phone).Include(a => a.address).FirstOrDefault(s => s.Id == id);
        }

        // 
        ///<summary>
        ///saves user Dto into database
        ///</summary>
        ///<param name="account"></param>
        public void UpdateToDataBase(user account)
        {
            _context.Update(account);
            _context.SaveChanges();
        }

        ///<summary>
        ///returns AddressBook
        ///</summary>
        ///<param name="id"></param>
        ///<returns>user</returns>
        public user GetAddressbook(Guid id)
        {
            return _context.User.Include(s => s.email).Include(a => a.address).Include(p => p.phone).FirstOrDefault(s => s.Id == id);
        }

        ///<summary>
        ///returns list of refterm data 
        ///</summary>
        ///<param name="key"></param>
        ///<returns>List<refTerm></returns>
        public List<refTerm> getRefSetData(string key)
        {
            bool isExists = _context.RefSet.Any(cus => cus.key == key);
            if (!isExists)
            {
                return null;
            }
            List<refTerm> refTermList = _context.SetRefTerm.Include(s => s.refSet).Include(s => s.refTerm).Where(w => w.refSet.key == key).Select(s => s.refTerm).ToList();
            return refTermList;

        }


        // 
        ///<summary>
        ///returns count of addressBook count 
        ///</summary>
        ///<returns>int<refTerm></returns>
        public int GetAddressBookCount()
        {
           
            return _context.User.Include(s =>s.email).Include(a => a.address).Include(p => p.phone).Select(s => s.Id).Count();
        
        }

        //  
        ///<summary>
        /// takes Args as Email and checks in database ,returns string 
        ///</summary>
        ///<param name="email"></param>
        ///<param name="id"></param>
        ///<returns>string<refTerm></returns>
        public string EmailList(ICollection<EmailDTO> email,Guid id)
        {
            
            List<string> emailList = id != Guid.Empty ? _context.Email.Where(s => s.userId != id).Select(s => s.emailAddress).ToList() :
                    _context.Email.Select(s => s.emailAddress).ToList();
            foreach (string item in email.Select(s => s.emailAddress))
            {
                bool userEmail = emailList.Contains(item);   
                if (userEmail)
                {
                    return item;
                }
            }
            return null;
        }

        //  
        ///<summary>
        /// takes Args as Phone and checks in database ,returns string
        ///</summary>
        ///<param name="phone"></param>
        ///<param name="id"></param>
        ///<returns>string<refTerm></returns>
        public string PhoneList(ICollection<PhoneDTO> phone,Guid id)
        {
            List<string> phoneList = id != Guid.Empty ? _context.Phone.Where(s => s.userId != id).Select(s => s.phoneNumber).ToList() :
                    _context.Phone.Select(s => s.phoneNumber).ToList();
            foreach (string item in phone.Select(s => s.phoneNumber))
            {
                bool userPhone = phoneList.Contains(item);     //_context.Phone.Where(w => w.phoneNumber == item).FirstOrDefault();
                if (userPhone)
                {
                    return item;
                }
            }
            return null;
        }

        //  
        ///<summary>
        /// takes Args as Address and checks in database ,returns string
        ///</summary>
        ///<param name="addresses"></param>
        ///<param name="id"></param>
        ///<returns>string<refTerm></returns>
        public string AdderessList(ICollection<AddressDTO> addresses,Guid id)
        {
            
            foreach (AddressDTO item in addresses)
            {
                string josnObjj = JsonConvert.SerializeObject(item);
                foreach (address each in _context.Address)
                {
                    AddressDTO addressObj = new AddressDTO
                    {
                        line1 = each.line1,
                        line2 = each.line2,
                        city = each.city,
                        zipCode = each.zipCode,
                        stateName = each.stateName,
                        type = new TypeDTO { key = (each.refTermId).ToString().ToUpper() },
                        country = new TypeDTO { key = (each.country).ToString().ToUpper() },
                    };
                    string josnObj = JsonConvert.SerializeObject(addressObj);
                    bool isNewAddress = id != Guid.Empty ? id != each.userId : true;
                    if (josnObj == josnObjj && isNewAddress)
                    {
                        return josnObj;
                    }
                }
            }
            return null;
        }

        //
        ///<summary>
        /// Delets addressBook from database
        ///</summary>
        ///<param name="account"></param>
        public void RemoveAccount(user account)
        {
          
             _context.Remove(account);
             _context.SaveChanges();
        }

        //
        ///<summary>
        /// saves AdderssBoook to database
        ///</summary>
        ///<param name="account"></param>
        public void  SaveToDataBase(user account)
        {
            _context.Add(account);
            _context.SaveChanges();
        }
        //
        ///<summary>
        /// saves file to database
        ///</summary>
        ///<param name="fileObj"></param>
        public void SaveFileToDataBase(asset fileObj)
        {      
            _context.Add(fileObj);
            _context.SaveChanges();       
        }

        ///<summary>
        /// returns type 
        ///</summary>
        ///<param name="id"></param>
        ///<returns>string<refTerm></returns>
        public string GetType(Guid id)
        {
            return _context.RefTerm.Where(w => w.Id ==id).Select(s => s.key).FirstOrDefault();
          
        }

        //
        ///<summary>
        /// returns bytes of file 
        ///</summary>
        ///<param name="id"></param>
        ///<returns>byte[]<refTerm></returns>
        public byte[] GetFile(Guid id)
        {
            return _context.AssetDTO.Where(f => f.Id == id).Select(s => s.field).FirstOrDefault();
        }

        //
        ///<summary>
        ///saves new Admin login details into database
        ///</summary>
        ///<param name="sinupModel"></param>
        public void SinupAdmin(login sinupModel)
        {
             _context.Add(sinupModel);
            _context.SaveChanges();   
        }
    }
}
