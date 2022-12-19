
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
        ///<summary>
        /// takes Args username string ,returns string ,checks username exists in database
        ///</summary>
        ///<param name="userName"></param>
        ///<returns>string</returns>
        public string loginDetails(string userName)
        {
            return _context.Login.Where(src => src.UserName == userName).Select(src => src.Password).FirstOrDefault();
            
        }

        ///<summary>
        /// takes Args username string ,returns bool ,checks username exists in database
        ///</summary>
        ///<param name="text"></param>
        ///<returns>bool</returns>
        public bool IsUserNameExists(string text)
        {
            List<string> listOfAdmins =  _context.Login.Select(src => src.UserName).ToList();
            return listOfAdmins.Contains(text);
        }

      
        /// takes Args size Int,pageNo Int ,returns List<user> ,Applys Pagenation 
        ///</summary>
        ///<param name="size"></param>
        ///<param name="pageNo"></param>
        ///<returns>List of addressBooks</returns>
        public List<User> GetPageinatedList( int size, int pageNo)
        {
            List<User> listOfUsers  = _context.User.Include(eml => eml.Email).Include(phn => phn.Phone).Include(art => art.Address)
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
        public User GetAccount(Guid id, UserDTO userDTO)
        {

            return  _context.User.Include(eml => eml.Email).Include(phn => phn.Phone).Include(art => art.Address).FirstOrDefault(src => src.Id == id);
        }

        
        ///<summary>
        ///saves user Dto into database
        ///</summary>
        ///<param name="account"></param>
        public void UpdateToDataBase(User account)
        {
            _context.Update(account);
            _context.SaveChanges();
        }

        public bool IsCountryExixsted(string type)
        {
            return _context.RefTerm.Any(cus => cus.Id == Guid.Parse(type));
        }


        ///<summary>
        ///returns AddressBook
        ///</summary>
        ///<param name="id"></param>
        ///<returns>user</returns>
        public User GetAddressbook(Guid id)
        {
            return _context.User.Include(src => src.Email).Include(art => art.Address).Include(phn => phn.Phone).FirstOrDefault(src => src.Id == id);
        }

        ///<summary>
        ///returns list of refterm data 
        ///</summary>
        ///<param name="key"></param>
        ///<returns>List<refTerm></returns>
        public List<RefTerm> GetRefSetData(string key)
        {
            bool isExists = _context.RefSet.Any(cus => cus.Key == key);
            if (!isExists)
            {
                return null;
            }
            List<RefTerm> refTermList = _context.SetRefTerm.Include(src => src.RefSet).Include(src => src.RefTerm).Where(wrt => wrt.RefSet.Key == key).Select(src => src.RefTerm).ToList();
            return refTermList;

        }


        ///<summary>
        ///returns count of addressBook count 
        ///</summary>
        ///<returns>int<refTerm></returns>
        public int GetAddressBookCount()
        {
            return _context.User.Include(s => s.Email).Include(a => a.Address).Include(p => p.Phone).Select(s => s.Id).Count();
        }

        ///<summary>
        /// takes Args as Email and checks in database ,returns string 
        ///</summary>
        ///<param name="email"></param>
        ///<param name="id"></param>
        ///<returns>string<refTerm></returns>
        public string EmailList(ICollection<EmailDTO> email,Guid id)
        {
            
            List<string> emailList = id != Guid.Empty ? _context.Email.Where(src => src.UserId != id).Select(src=> src.EmailAddress).ToList() :
                    _context.Email.Select(src => src.EmailAddress).ToList();
            foreach (string item in email.Select(src => src.EmailAddress))
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
            List<string> phoneList = id != Guid.Empty ? _context.Phone.Where(src => src.UserId != id).Select(src => src.PhoneNumber).ToList() :
                    _context.Phone.Select(src => src.PhoneNumber).ToList();
            foreach (string item in phone.Select(src => src.PhoneNumber))
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
                foreach (Address each in _context.Address)
                {
                    AddressDTO addressObj = new AddressDTO
                    {
                        Line1 = each.Line1,
                        Line2 = each.Line2,
                        City = each.City,
                        Zipcode = each.Zipcode,
                        StateName = each.StateName,
                        Type = new TypeDTO { Key = (each.RefTermId).ToString().ToUpper() },
                        Country = new TypeDTO { Key = (each.Country).ToString().ToUpper() },
                    };
                    string josnObj = JsonConvert.SerializeObject(addressObj);
                    bool isNewAddress = id != Guid.Empty ? id != each.UserId : true;
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
        public void RemoveAccount(User account)
        {
             _context.Remove(account);
             _context.SaveChanges();
        }

        //
        ///<summary>
        /// saves AdderssBoook to database
        ///</summary>
        ///<param name="account"></param>
        public void  SaveToDataBase(User account)
        {
            _context.Add(account);
            _context.SaveChanges();
        }
        //
        ///<summary>
        /// saves file to database
        ///</summary>
        ///<param name="fileObj"></param>
        public void SaveFileToDataBase(Asset fileObj)
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
            return _context.RefTerm.Where(wrt => wrt.Id ==id).Select(src => src.Key).FirstOrDefault();
          
        }

        ///<summary>
        /// returns bytes of file 
        ///</summary>
        ///<param name="id"></param>
        ///<returns>byte[]<refTerm></returns>
        public byte[] GetFile(Guid id)
        {
            return _context.AssetDTO.Where(fnd => fnd.Id == id).Select(src => src.Field).FirstOrDefault();
        }

        ///<summary>
        ///saves new Admin login details into database
        ///</summary>
        ///<param name="sinupModel"></param>
        public void SinupAdmin(Login sinupModel)
        {
             _context.Add(sinupModel);
            _context.SaveChanges();   
        }
    }
}
