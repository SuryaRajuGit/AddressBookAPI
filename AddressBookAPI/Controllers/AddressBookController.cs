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
        public AddressBookController(IAddressBookServices addressBookServices)
        {
            _addressBookServices = addressBookServices; 
        }

        [HttpPost]
        [Route("account")]
        public async Task<IActionResult> AddNewAddressBook([FromBody]userDTO userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorDTO { type = ModelState.Keys.FirstOrDefault(),
                    description = ModelState.Values.Select(s => s.Errors.Select(s => s.ErrorMessage).FirstOrDefault() ).FirstOrDefault() }) ;
            }

            ErrorDTO response = await _addressBookServices.AddNewAddressBook(userModel);

            if(response != null)
            {
                return StatusCode(409, response);
            }
            var savetoDb = await _addressBookServices.saveToDatabase(userModel);
            if(savetoDb == null)
            {
                return NotFound(); 
            }
            return StatusCode(201, savetoDb);
            
            
        }

        [HttpPost]
        [Route("sinup")]
        public async Task<ActionResult> SinUpAdmin([FromBody] signupDTO sinupModel)
        {
           if(!ModelState.IsValid)
            {
                return  BadRequest(new ErrorDTO { type = ModelState.Keys.FirstOrDefault(), description = ModelState.Values.Select(s => s.Errors.Select(s => s.ErrorMessage).FirstOrDefault()).FirstOrDefault() });
            }

            var response = _addressBookServices.SignupAdmin(sinupModel);
            if (response != null)
            {
                return Ok(response);
            }
            return StatusCode(409, "username already exists");
        }


        [HttpDelete]
        [Route("account/{id:Guid}")]
        public async Task<IActionResult>  DeleteAddressBook(Guid id)
        {
             Guid? response = await _addressBookServices.DeletAddressBook(id);
            if(response == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPut]
        [Route("account/{id:Guid}")]
        public async Task<IActionResult> UpdateAddressBook(Guid id, [FromBody] userDTO userModel)
        {
            Guid? response = await _addressBookServices.UpdateAddressBook(id, userModel);
            if(response == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("auth/signin")]
        public async Task<IActionResult> VerifyUser([FromBody]logInDTO logInModel ) 
        {
            logInResponseDTO response = await _addressBookServices.VerifyUser(logInModel);
            if (response == null)
            {
                return Unauthorized("Wrong Password");
              
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("account")]
        public async Task<ActionResult<List<userDTO>>> GetAllAddressBooksAccounts(int size, string pageNo, string sortBy, string sortOrder)
        {
             List<userDTO> response = await _addressBookServices.GetAllAddressBooks(size,pageNo,sortBy,sortOrder);           
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
            int count =  _addressBookServices.GetAddressBookCount();
            return Ok(count);
        }

        [HttpGet]
        [Route("get/account/{id:Guid}")]
        public async Task<ActionResult<userDTO>> GetAddressBook(Guid id)
        {
            userDTO response = await _addressBookServices.GetAddressBook(id);
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
            UploadResponseDTO response = await _addressBookServices.UploadFile(file);
            return Ok(response);
        }

        [HttpGet]
        [Route("asset/downloadFile/{id:Guid}")]
        public async Task<IActionResult> Download(Guid id)
        {
            byte[] fileBytes = _addressBookServices.Download(id);
            if(fileBytes == null)
            {
                return NotFound();
            }
            return File(fileBytes, "image/jpeg");
        }

    }
}

