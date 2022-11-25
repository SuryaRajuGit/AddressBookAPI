
using AddressBookAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
using AddressBookAPI.Entity.Dto;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Swashbuckle.Swagger.Annotations;

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
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType( StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        //  [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public  IActionResult AddNewAddressBook([FromBody]UserDTO userDTO)
        {
            _logger.LogInformation("adding new addressbook started");
          
            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong user data");
                ErrorDTO badRequest = _addressBookServices.modelStateInvalid(ModelState);
                return BadRequest(badRequest) ;
            }
            //takes Arg as Email Model returns string, checks email exists in database
            ErrorDTO isEmailExists = _addressBookServices.EmailExists(userDTO.Email);
            if(isEmailExists != null)
            {
                _logger.LogError("Email Already Exists "+isEmailExists);
                return StatusCode(409, isEmailExists);
               
            }
            //takes Arg as Phone Model returns string, checks phone exists in database
            ErrorDTO isPhoneExists = _addressBookServices.PhoneExists(userDTO.Phone);
            if(isPhoneExists != null)
            {
                _logger.LogError("Phone Already Exists "+isPhoneExists);
                return StatusCode(409, isPhoneExists);
                
            }
            //takes Arg as Address Model returns string, checks Address exists in database
            ErrorDTO isAddressExists = _addressBookServices.AddressExists(userDTO.Address);
            if (isAddressExists != null)
            {
                _logger.LogError("Address Already Exists "+isAddressExists);
                return StatusCode(409, isAddressExists);
                
            }
            //takes Arg as user Dto returns Guid, saves user data into database
            Guid? savetoDb =  _addressBookServices.saveToDatabase(userDTO);
            if (savetoDb == null)
            {
                _logger.LogError("meta data not found");
                return NotFound(new ErrorDTO {type="NotFound",description="Meta Data not found in the database" });
            }
            _logger.LogInformation("New AddressBook Created");
            return StatusCode(201, savetoDb);


        }

        [HttpPost]
        [Route("signup")] //Creates new Admin
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult SignUpAdmin([FromBody] SignupDTO sinupDTO)
        {
            _logger.LogInformation("Signup admin started");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong data");
                ErrorDTO badRequest = _addressBookServices.modelStateInvalidSinupAPI(ModelState);
                return BadRequest(badRequest);
            }
            // takes Arg as sinup Dto returns string, checks new login details exists in database
           
            string response = _addressBookServices.SignupAdmin(sinupDTO);
            if (response == null)
            {
                _logger.LogError("username already exists");
                return StatusCode(409, new ErrorDTO
                {
                    type = "Conflict",
                    description = response +
                    "  already exists"
                });
            }
            _logger.LogInformation("new signup created");
            
            return Ok();
        }


        [HttpDelete]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [Route("account/{id:Guid}")] //Deletes Addressbook from database
        public IActionResult  DeleteAddressBook(Guid id)
        {
            _logger.LogInformation("deleting addressbook started");
            //takes Args as string returns string,checks id exists in database
            string response =  _addressBookServices.DeletAddressBook(id);
            if(response == null)
            {
                _logger.LogError("user notfound");
                return NotFound(new ErrorDTO { type = "Delete AddressBook", 
                    description = "Data not found with the Guid in the database" });
            }
            _logger.LogInformation("address boook deleted");
            return Ok();
        }

        [HttpPut]
        [Route("account/{id:Guid}")] //Updates existing AddressBook in database
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        public IActionResult UpdateAddressBook([FromBody] UserDTO userDTO, [FromRoute] Guid id)
        {
            
            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong data");
                ErrorDTO badRequest = _addressBookServices.modelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }
            _logger.LogInformation("updating  addressbook started");
            //  var e = ModelState.Values.SelectMany(s => s ValidationState.ToString() == "Valid").    //Select(s => s).FirstOrDefault(s => s.Select(s => s.) .ToString() == "Valid").ToString();
            //var ve = ModelState.Values.Select(s => s.Errors.Select(s => s.ErrorMessage)
            //       .FirstOrDefault()).FirstOrDefault();
            try
            {
                ErrorDTO response = _addressBookServices.Duplicates(userDTO);
                if (response !=null )
                {
                    _logger.LogError("Conflict");
                    return StatusCode(409,response);
                }
            }
            catch
            {
                _logger.LogError("NotFound");
                return NotFound(new ErrorDTO { type = "NotFound", description = "meta-data not found" });
            }
            
                //takes Args Guid and user Dto ,updates Addressbook in database.
             ErrorDTO saveResponse =  _addressBookServices.UpdateAddressBook(id, userDTO);
            if (saveResponse != null)
            {
                _logger.LogError("NotFound");
                return NotFound(saveResponse);
            }


            _logger.LogInformation("addressbook updated");
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("auth/signin")] //checks the user login details in database
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult VerifyUser([FromBody]LogInDTO logInDTO ) 
        {
            _logger.LogInformation("login user started");

            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong login data");
                ErrorDTO badRequest = _addressBookServices.modelStateInvaliLoginAPI(ModelState);
                return BadRequest(badRequest);
            }

            //takes Args as login details Dto returns string ,verfies the user details with existing login details in db.
            logInResponseDTO response =  _addressBookServices.VerifyUser(logInDTO);
            if (response == null)
            {
                _logger.LogError("entered wrong password");
                return Unauthorized(new ErrorDTO
                {
                    type = "Unauthorized",
                    description = 
                    "Entered wrong login details"
                });
            }
            _logger.LogInformation("user verified");
            return Ok(response);
        }

        [HttpGet]
        [Route("account")] //Retervies all the Addressbooks in the database
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        public ActionResult<List<UserDTO>> GetAllAddressBooksAccounts(int size, int pageNo, string sortBy, string sortOrder)
        {
            _logger.LogInformation("getting all addressbooks started");
            // takes Args size Int ,pageNo Int,sortBy string ,sortOrder string,return UserDTO dto,retervies data from the db.
            List<UserDTO> response =  _addressBookServices.GetAllAddressBooks(size,pageNo,sortBy,sortOrder);           
             if (response == null)
            {
                _logger.LogError("data not found");
                return NotFound(new ErrorDTO {type="AddressBooks",description="Data not found in the database" }) ;
            }
            _logger.LogInformation("all addressbooks retrieved");
            return Ok(response);
        }

        [HttpGet]
        [Route("account/count")] // Gets the count of addressbooks in the database
        [ProducesResponseType(typeof(ResponseGetCountDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        public IActionResult GetAddressBookCount()
        {
            _logger.LogInformation("getting number of addressbooks started");
            // returns Int ,gets the count of addressbooks in the database.
            int count =  _addressBookServices.GetAddressBookCount();
            _logger.LogInformation("number of addressbooks returned");
            return Ok(new ResponseGetCountDTO {count=count });
        }

        [HttpGet] 
        [Route("get/account/{id:Guid}")] // Gets single  AddressBook based on user Id.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        public ActionResult<UserDTO> GetAddressBook(Guid id)
        {
            _logger.LogInformation("getting single addressbook started");
            // takes Args as Guid ,returns UserDTO Dto, gets addressbook from database based on Id.
            UserDTO response =  _addressBookServices.GetAddressBook(id);
            if(response == null)
            {
                _logger.LogError("data not found");
                return NotFound(new ErrorDTO {type= "NotFound",description="AdderessBook not found in the database" });
            }
            _logger.LogInformation("addressbook retrieved");
            return Ok(response);
        }

        [HttpPost]
        [Route("asset")] // uploads file into database 
        [ProducesResponseType(typeof(UploadResponseDTO) ,StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        public ActionResult<UploadResponseDTO> UploadFile(IFormFile file)
        {
            _logger.LogInformation("uploading file started");
            // takes Args file IFormFile ,returns UploadResponseDTO Dto,uploads file into database
            UploadResponseDTO response =  _addressBookServices.UploadFile(file);
            _logger.LogInformation("file uploaded");
            return Ok(response);
        }

        [HttpGet] //downloads file from database
        [Route("asset/{id:Guid}")]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Download(Guid id)
        {
            _logger.LogInformation("download file started");
            // takes Args Guid ,returns file bytes[] ,gets file bytes[] from database based on file id.
            byte[] fileBytes = _addressBookServices.Download(id);
            if(fileBytes == null)
            {
                _logger.LogError("file not found");
                return NotFound(new ErrorDTO {type= "NotFound",description="asset not exists in database" });
            }
            _logger.LogInformation("file returned");
            return File(fileBytes, "image/jpeg");
        }

    }
}

