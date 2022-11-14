using AddressBookAPI.Models;
using AddressBookAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AddressBookAPI.Data;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AddressBookAPI.Services;
using System.Net.Mail;

namespace AddressBookAPI.Controllers
{
    [Route("api")]
    [ApiController]
  //  [Authorize(AuthenticationSchemes ="Bearer")]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressBookServices _addressBookServices;
        private readonly AddressBookRepository addressBookRepository;

        public AddressBookController(IAddressBookServices addressBookServices)
        {
            _addressBookServices = addressBookServices;
           
        }

        [HttpPost]
        [Route("account")]
        public async Task<IActionResult> AddNewAddressBook([FromBody]userModel userModel)
        {
            foreach (var item in userModel.Email)
            {
                string invalidEmail = item.emailAddress;
                try
                {
                    var mail = new MailAddress(invalidEmail);
                    bool isValidEmail = mail.Host.Contains(".");
                    if (!isValidEmail)
                    {
                        return BadRequest(new validationErrorModel { type = "Email", description = "Invalid Email" });
                    }
                }
                catch (Exception)
                {
                    return BadRequest(new validationErrorModel { type = "Email", description = "Invalid Email : "+ invalidEmail });
                }
            }
            foreach (var item in userModel.Phone)
            {
                if(!(item.phoneNumber.Length == 10))
                {
                    return BadRequest(new validationErrorModel { type = "Phone", description = "Invalid Phone Number : "+item.phoneNumber  });
                }
            }

            var response = await _addressBookServices.AddNewAddressBook(userModel);
     
            switch (response.Item1)
            {
                case var value when value == enumList.Constants.email.ToString():
                    return StatusCode(409, "Email Already Exists " + response.Item2);
                    break;
                case var value when value == enumList.Constants.address.ToString():
                    return StatusCode(409, "Address Already Exists \n"+response.Item2);
                    break;
                case var value when value == enumList.Constants.phone.ToString():
                    return StatusCode(409, "phone number already Exists " + response.Item2);
                    break;
                case var value when value == string.Empty:
                    return NotFound();
                    break;
                default:
                    var id = Guid.Parse(response.Item1);
                    return StatusCode(201, id);
                    break;
            }
        }
        [HttpPost]
        [Route("seed_csv")]
        public async Task<ActionResult> Seed()
        {
            try
            {
                var count = _addressBookServices.Seed();
                return Ok(count);
            }
            catch
            {
                return BadRequest();
            }
                  
        }


        [HttpPost]
        [Route("sinup")]
        public async Task<ActionResult> SinUpAdmin([FromBody] signupModel sinupModel)
        {
            
            if (!sinupModel.password.Any(char.IsUpper) )
            {
                return BadRequest("One Uppercase Required");
            }
            if (sinupModel.password.Contains(" "))
            {
                return BadRequest("Do not Contain spaces");
            }
            if(sinupModel.password.Length < 6)
            {
                return BadRequest("Length of password shoud be minimum 6");
            }
            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] specialh = specialCh.ToCharArray();
            int i;
            for ( i = 0; i < sinupModel.password.Length; i++)
            {
                if( specialCh.Contains(sinupModel.password[i]) )
                {
                    break;
                }
            }
            if(i == sinupModel.password.Length)
            {
                return BadRequest("Contain one special character");
            }

            var response =  _addressBookServices.SignupAdmin(sinupModel);
            return Ok(response);
        }


        [HttpDelete]
        [Route("account/{id:Guid}")]
        public async Task<IActionResult>  DeleteAddressBook(Guid id)
        {
             var response = await _addressBookServices.DeletAddressBook(id);
            if(response == null)
            {
                return NotFound();
            }
            return Ok();
         
        }

        [HttpPut]
        [Route("account/{id:Guid}")]
        public async Task<IActionResult> UpdateAddressBook(Guid id, [FromBody] userModel userModel)
        {
            var response = await _addressBookServices.UpdateAddressBook(id, userModel);
            if(response == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("auth/signing")]
        public async Task<IActionResult> VerifyUser([FromBody]logInModel logInModel ) 
        {
            var response = await _addressBookServices.VerifyUser(logInModel);
            if (response == null)
            {
                return Unauthorized();
              
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("account")]
        public async Task<ActionResult<List<userModel>>> GetAllAddressBooks(int size, string pageNo, string sortBy, string sortOrder)
        {
             var response = await _addressBookServices.GetAllAddressBooks(size,pageNo,sortBy,sortOrder);           
             if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("account/count")]
        public async Task<IActionResult> GetAddressBookCount()
        {
            var count = await _addressBookServices.GetAddressBookCount();
            return Ok(count);
        }

        [HttpGet]
        [Route("get/account/{id:Guid}")]
        public async Task<ActionResult<userModel>> GetAddressBook(Guid id)
        {
            var response = await _addressBookServices.GetAddressBook(id);
            if(response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var response = await _addressBookServices.UploadFile(file);
            return Ok(response);
        }

        [HttpGet]
        [Route("asset/downloadFile/{id:Guid}")]
        public async Task<IActionResult> Download(Guid id)
        {
            var fileBytes = _addressBookServices.Download(id);
            if(fileBytes == null)
            {
                return NotFound();
            }
            return File(fileBytes, "image/jpeg");
        }

    }
}

