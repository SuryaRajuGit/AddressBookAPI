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
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Logging;

namespace AddressBookAPI.Controllers
{
    [Route("api")]
    [ApiController]
  //  [Authorize(AuthenticationSchemes ="Bearer")]
    public class AddressBookController : ControllerBase
    {
        // private readonly IServer _server;
        private readonly ILogger _logger;
        private readonly IAddressBookServices _addressBookServices;
        public AddressBookController(IAddressBookServices addressBookServices,ILogger logger )
        {
            _addressBookServices = addressBookServices;
            _logger = logger;
      
        }

        [HttpPost]
        [Route("account")] //Adds new AddressBook to database
        public async Task<IActionResult> AddNewAddressBook([FromBody]UserDTO userDTO)
        {
            _logger.LogInformation("adding new addressbook started");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong user data");
                return BadRequest(new ErrorDTO { type = ModelState.Keys.FirstOrDefault(),
                    description = ModelState.Values.Select(s => s.Errors.Select(s => s.ErrorMessage).FirstOrDefault() ).FirstOrDefault() }) ;
                
            }
            //takes Arg as Email Model returns string, checks email exists in database
            var isEmailExists = _addressBookServices.EmailExists(userDTO.Email);
            if(isEmailExists != null)
            {
                _logger.LogError("Email Already Exists "+isEmailExists);
                return StatusCode(409, isEmailExists + "Already Exists");
               
            }
            //takes Arg as Phone Model returns string, checks phone exists in database
            var isPhoneExists = _addressBookServices.PhoneExists(userDTO.Phone);
            if(isPhoneExists != null)
            {
                _logger.LogError("Phone Already Exists "+isPhoneExists);
                return StatusCode(409, isPhoneExists + "Already Exists");
                
            }
            //takes Arg as Address Model returns string, checks Address exists in database
            var isAddressExists = _addressBookServices.AddressExists(userDTO.Address);
            if (isAddressExists != null)
            {
                _logger.LogError("Address Already Exists "+isAddressExists);
                return StatusCode(409, "Already Exists"+ isAddressExists);
                
            }
            //takes Arg as user Dto returns Guid, saves user data into database
            Guid? savetoDb =  _addressBookServices.saveToDatabase(userDTO);
            if (savetoDb == null)
            {
                _logger.LogError("user information not found");
                return NotFound();
            }
            _logger.LogInformation("New AddressBook Created");
            return StatusCode(201, savetoDb);


        }

        [HttpPost]
        [Route("signup")] //Creates new Admin
        public async Task<ActionResult> SignUpAdmin([FromBody] SignupDTO sinupDTO)
        {
            _logger.LogInformation("Signup admin started");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong data");
                return  BadRequest(new ErrorDTO { type = ModelState.Keys.FirstOrDefault(), description = ModelState.Values.Select(s => s.Errors.Select(s => s.ErrorMessage).FirstOrDefault()).FirstOrDefault() });
            }
            // takes Arg as sinup Dto returns string, checks new login details exists in database
            var response = _addressBookServices.SignupAdmin(sinupDTO);
            if (response == null)
            {
                _logger.LogError("username already exists");
                return StatusCode(409, "username already exists");
            }
            _logger.LogInformation("new signup created");
            return Ok(response);
        }


        [HttpDelete]
        [Route("account/{id:Guid}")] //Deletes Addressbook from database
        public async Task<IActionResult>  DeleteAddressBook(Guid id)
        {
            _logger.LogInformation("deleting addressbook started");
            //takes Args as string returns string,checks id exists in database
            string response =  _addressBookServices.DeletAddressBook(id);
            if(response == null)
            {
                _logger.LogError("user notfound");
                return NotFound();
            }
            _logger.LogInformation("address boook deleted");
            return Ok();
        }

        [HttpPut]
        [Route("account/{id:Guid}")] //Updates existing AddressBook in database
        public async Task<IActionResult> UpdateAddressBook(Guid id, [FromBody]UserDTO userDTO)
        {
            _logger.LogInformation("updating  addressbook started");
            try
            {
                //takes Args Guid and user Dto ,updates Addressbook in database.
                _addressBookServices.UpdateAddressBook(id, userDTO);
            }
            catch
            {
                _logger.LogError("user notfound");
                return NotFound();
            }
            _logger.LogInformation("addressbook updated");
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("auth/signin")] //checks the user login details in database
        public async Task<IActionResult> VerifyUser([FromBody]LogInDTO logInDTO ) 
        {
            _logger.LogInformation("login user started");
            //takes Args as login details Dto returns string ,verfies the user details with existing login details in db.
            logInResponseDTO response =  _addressBookServices.VerifyUser(logInDTO);
            if (response == null)
            {
                _logger.LogError("entered wrong password");
                return Unauthorized("Wrong Password");
              
            }
            _logger.LogInformation("user verified");
            return Ok(response);
        }

        [HttpGet]
        [Route("account")] //Retervies all the Addressbooks in the database
        public async Task<ActionResult<List<UserDTO>>> GetAllAddressBooksAccounts(int size, int pageNo, string sortBy, string sortOrder)
        {
            _logger.LogInformation("getting all addressbooks started");
            // takes Args size Int ,pageNo Int,sortBy string ,sortOrder string,return UserDTO dto,retervies data from the db.
            List<UserDTO> response =  _addressBookServices.GetAllAddressBooks(size,pageNo,sortBy,sortOrder);           
             if (response == null)
            {
                _logger.LogError("data not found");
                return NotFound();
            }
            _logger.LogInformation("all addressbooks retrieved");
            return Ok(response);
        }

        [HttpGet]
        [Route("account/count")] // Gets the count of addressbooks in the database
        public async Task<IActionResult> GetAddressBookCount()
        {
            _logger.LogInformation("getting number of addressbooks started");
            // returns Int ,gets the count of addressbooks in the database.
            int count =  _addressBookServices.GetAddressBookCount();
            _logger.LogInformation("number of addressbooks returned");
            return Ok(count);
        }

        [HttpGet] 
        [Route("get/account/{id:Guid}")] // Gets single  AddressBook based on user Id.
        public async Task<ActionResult<UserDTO>> GetAddressBook(Guid id)
        {
            _logger.LogInformation("getting single addressbook started");
            // takes Args as Guid ,returns UserDTO Dto, gets addressbook from database based on Id.
            UserDTO response =  _addressBookServices.GetAddressBook(id);
            if(response == null)
            {
                _logger.LogError("data not found");
                return NotFound();
            }
            _logger.LogInformation("addressbook retrieved");
            return Ok(response);
        }

        [HttpPost]
        [Route("upload")] // uploads file into database 
        public async Task<ActionResult<UploadResponseDTO>> UploadFile(IFormFile file)
        {
            _logger.LogInformation("uploading file started");
            // takes Args file IFormFile ,returns UploadResponseDTO Dto,uploads file into database
            UploadResponseDTO response =  _addressBookServices.UploadFile(file);
            _logger.LogInformation("file uploaded");
            return Ok(response);
        }

        [HttpGet] //downloads file from database
        [Route("asset/downloadFile/{id:Guid}")]
        public async Task<IActionResult> Download(Guid id)
        {
            _logger.LogInformation("download file started");
            // takes Args Guid ,returns file bytes[] ,gets file bytes[] from database based on file id.
            byte[] fileBytes = _addressBookServices.Download(id);
            if(fileBytes == null)
            {
                _logger.LogError("file not found");
                return NotFound();
            }
            _logger.LogInformation("file returned");
            return File(fileBytes, "image/jpeg");
        }

    }
}

