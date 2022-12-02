
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
using AddressBookAPI.Entity.Models;
using System.Net.Mime;

namespace AddressBookAPI.Controllers
{
    [Route("api")]
    [ApiController]
    // [Authorize(AuthenticationSchemes ="Bearer")]
    public class AddressBookController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAddressBookServices _addressBookServices;
        public AddressBookController(IAddressBookServices addressBookServices, ILogger logger)
        {
            _addressBookServices = addressBookServices;
            _logger = logger;
        }
        [HttpPost]
        [Route("account")]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
        ///<summary>
        ///Adds new AddressBook to database
        ///</summary>
        ///<param name="userDTO">new addressbook data</param>
        ///<returns>New AddressBook created</returns>
        public IActionResult AddNewAddressBook([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation("adding new addressbook started");

            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong user data");
                ErrorDTO badRequest = _addressBookServices.ModelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }
            Guid id = Guid.Empty;
            //takes Arg as Email Model returns string, checks email exists in database
            ErrorDTO isEmailExists = _addressBookServices.EmailExists(userDTO.Email, id);
            if (isEmailExists != null)
            {
                _logger.LogError("Email Already Exists " + isEmailExists);
                return StatusCode(409, isEmailExists);
            }
            //takes Arg as Phone Model returns string, checks phone exists in database
            ErrorDTO isPhoneExists = _addressBookServices.PhoneExists(userDTO.Phone, id);
            if (isPhoneExists != null)
            {
                _logger.LogError("Phone Already Exists " + isPhoneExists);
                return StatusCode(409, isPhoneExists);

            }
            //takes Arg as Address Model returns string, checks Address exists in database
            ErrorDTO isAddressExists = _addressBookServices.AddressExists(userDTO.Address, id);
            if (isAddressExists != null)
            {
                _logger.LogError("Address Already Exists " + isAddressExists);
                return StatusCode(409, isAddressExists);

            }
            //takes Arg as user Dto returns Guid, saves user data into database
            Guid? response = _addressBookServices.SaveToDatabase(userDTO);
            if (response == null)
            {
                _logger.LogError("meta data not found");
                return NotFound(new ErrorDTO { type = "meta-data", description = "Meta Data not found in the database" });
            }
            _logger.LogInformation("New AddressBook Created");
            return StatusCode(201, response);


        }

        [HttpPost]
        [Route("signup")] //
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
        ///<summary>
        ///Creates new Admin
        ///</summary>
        ///<param name="sinupDTO">New login detials</param>
        ///<returns>Created New Admin</returns>
        public IActionResult SignUpAdmin([FromBody] SignupDTO sinupDTO)
        {
            _logger.LogInformation("Signup admin started");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong data");
                ErrorDTO badRequest = _addressBookServices.ModelStateInvalidSinupAPI(ModelState);
                return BadRequest(badRequest);
            }
            // takes Arg as sinup Dto returns string, checks new login details exists in database
            string response = _addressBookServices.SignupAdmin(sinupDTO);
            if (response == null)
            {
                _logger.LogError("username already exists");
                return StatusCode(409, new ErrorDTO
                {
                    type = "AddressBook",
                    description = response +
                    "  already exists"
                });
            }
            _logger.LogInformation("new signup created");
            return Ok("Created New Admin Succefully");
        }


        [HttpDelete]
        [Route("account/{id:Guid}")]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
        ///<summary>
        ///Deletes Addressbook from database
        ///</summary>
        ///<param name="id">Guid of AddressBook</param>
        ///<returns>AddressBook Deleted</returns>
        public IActionResult DeleteAddressBook(Guid id)
        {
            _logger.LogInformation("deleting addressbook started");
            //takes Args as string returns string,checks id exists in database
            string response = _addressBookServices.DeletAddressBook(id);
            if (response == null)
            {
                _logger.LogError("user notfound");
                return NotFound(new ErrorDTO { type = "AddressBook",
                    description = "AddressBook not found with the Guid in the database"
                });
            }
            _logger.LogInformation("address boook deleted");
            return Ok("AddressBook has been  Deleted sucessfully");
        }

        [HttpPut]
        [Route("account/{id:Guid}")] //
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
        ///<summary>
        ///Updates existing AddressBook in database
        ///</summary>
        ///<param name="userDTO">Guid of AddressBook</param>
        ///<param name="id">Guid of AddressBook</param>
        ///<returns>AddressBook updated</returns>
        public IActionResult UpdateAddressBook([FromBody] UserDTO userDTO, [FromRoute] Guid id)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong data");
                ErrorDTO badRequest = _addressBookServices.ModelStateInvalid(ModelState);
                return BadRequest(badRequest);
            }
            _logger.LogInformation("updating  addressbook started");
            ErrorDTO isEmailExists = _addressBookServices.EmailExists(userDTO.Email, id);
            if (isEmailExists != null)
            {
                _logger.LogError("Email Already Exists " + isEmailExists);
                return StatusCode(409, isEmailExists);
            }
            //takes Arg as Phone Model returns string, checks phone exists in database
            ErrorDTO isPhoneExists = _addressBookServices.PhoneExists(userDTO.Phone, id);
            if (isPhoneExists != null)
            {
                _logger.LogError("Phone Already Exists " + isPhoneExists);
                return StatusCode(409, isPhoneExists);

            }
            //takes Arg as Address Model returns string, checks Address exists in database
            ErrorDTO isAddressExists = _addressBookServices.AddressExists(userDTO.Address, id);
            if (isAddressExists != null)
            {
                _logger.LogError("Address Already Exists " + isAddressExists);
                return StatusCode(409, isAddressExists);

            }
            //takes Args Guid and user Dto ,updates Addressbook in database.
            ErrorDTO saveResponse = _addressBookServices.UpdateAddressBook(id, userDTO);
            if (saveResponse != null)
            {
                _logger.LogError("NotFound");
                return NotFound(saveResponse);
            }

            _logger.LogInformation("addressbook updated");
            return Ok("AddressBook has been updated sucessfully");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("auth/signin")] //
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
        ///<summary>
        ///checks the user login details in database
        ///</summary>
        ///<param name="logInDTO">login details</param>
        ///<returns>jwt token </returns>
        public IActionResult VerifyUser([FromBody] LogInDTO logInDTO)
        {
            _logger.LogInformation("login user started");

            if (!ModelState.IsValid)
            {
                _logger.LogError("Entered wrong login data");
                ErrorDTO badRequest = _addressBookServices.ModelStateInvaliLoginAPI(ModelState);
                return BadRequest(badRequest);
            }

            //takes Args as login details Dto returns string ,verfies the user details with existing login details in db.
            LogInResponseDTO response = _addressBookServices.VerifyUser(logInDTO);
            if (response == null)
            {
                _logger.LogError("entered wrong password");
                return Unauthorized(new ErrorDTO
                {
                    type = "Login",
                    description =
                    "Entered wrong login details"
                });
            }
            _logger.LogInformation("user verified");
            return Ok(response);
        }

        [HttpGet]
        [Route("account")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
        ///<summary>
        ///Retervies all the Addressbooks in the database
        ///</summary>
        ///<param name="size">size of adressBook ></param>
        ///<param name="pageNo">page no of addressbook</param>
        ///<param name="sortBy">to sort addressbook firstname or lastname</param>
        ///<param name="sortOrder">sort addressbook ascending or descending</param>
        ///<returns>list of addressbooks</returns>
        public ActionResult<List<UserDTO>> GetAllAddressBooksAccounts([FromQuery]int size =Constants.pageSize, [FromQuery(Name = Constants.pageno)] int pageNo = Constants.pageNo, [FromQuery(Name = Constants.sortby)] string sortBy = Constants.firstName, [FromQuery(Name = Constants.sortorder)] string sortOrder= Constants.ASC)
        {
            _logger.LogInformation("getting all addressbooks started");
            // takes Args size Int ,pageNo Int,sortBy string ,sortOrder string,return UserDTO dto,retervies data from the db.
            List<UserDTO> response =  _addressBookServices.GetAllAddressBooks(size,pageNo,sortBy,sortOrder);           
             if (response == null)
            {
                _logger.LogError("data not found");
                return StatusCode(204,"No Data in Database") ;
            }
            _logger.LogInformation("all addressbooks retrieved sucessfully");
            return Ok(response);
        }

        [HttpGet]
        [Route("account/count")] 
        [ProducesResponseType(typeof(ResponseGetCountDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
        ///<summary>
        ///Gets the count of addressbooks in the database
        ///</summary>
        ///<returns>checks the user login details in database</returns>
        public IActionResult GetAddressBookCount()
        {
            _logger.LogInformation("getting number of addressbooks started");
            // returns Int ,gets the count of addressbooks in the database.
            int count =  _addressBookServices.GetAddressBookCount();
            _logger.LogInformation("number of addressbooks returned");
            return Ok(new ResponseGetCountDTO {count=count });
        }

        [HttpGet] 
        [Route("get/account/{id:Guid}")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
        ///<summary>
        ///Gets single  AddressBook based on user Id.
        ///</summary>
        ///<param name="id">Guid of Addressbook ></param>
        ///<returns>AddressBook</returns>
        public ActionResult<UserDTO> GetAddressBook(Guid id)
        {
            _logger.LogInformation("getting single addressbook started");
            // takes Args as Guid ,returns UserDTO Dto, gets addressbook from database based on Id.
            UserDTO response =  _addressBookServices.GetAddressBook(id);
            if(response == null)
            {
                _logger.LogError("data not found");
                return NotFound(new ErrorDTO {type= "AddressBook",description="AdderessBook not found in the database" });
            }
            _logger.LogInformation("addressbook retrieved");
            return Ok(response);
        }

        [HttpPost]
        [Route("asset")] 
        [ProducesResponseType(typeof(UploadResponseDTO) ,StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
        ///<summary>
        ///uploads file into database 
        ///</summary>
        ///<param name="file">Guid of Addressbook ></param>
        ///<returns>UploadResponseDTO</returns>
        public ActionResult<UploadResponseDTO> UploadFile(IFormFile file)
        {
            _logger.LogInformation("uploading file started");
            // takes Args file IFormFile ,returns UploadResponseDTO Dto,uploads file into database
            UploadResponseDTO response =  _addressBookServices.UploadFile(file);
            _logger.LogInformation("file uploaded");
            return Ok(response);
        }

        [HttpGet]
        [Route("asset/{id:Guid}")]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
        ///<summary>
        ///downloads file from database
        ///</summary>
        ///<param name="id">Guid of file ></param>
        ///<returns>image file</returns>
        public ActionResult Download(Guid id)
        {
            _logger.LogInformation("download file started");
            Byte[] fileBytes = _addressBookServices.Download(id);

            if (fileBytes == null)
            {
                _logger.LogError("file not found");
                return NotFound(new ErrorDTO { type = "Asset", description = "asset not exists in database" });
            }
            _logger.LogInformation("file returned");
             return File(fileBytes, Constants.ConetentType);
        }

        [HttpGet]
        [Route("meta-data/ref-set/{key}")]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status500InternalServerError)]
        ///<summary>
        /// gets list of meta-data linked to the key 
        ///</summary>
        ///<param name="key">Guid of file ></param>
        ///<returns>RefSetResponseDto</returns>
        public ActionResult<List<RefSetResponseDto>> RefsetData(string key)
        {
            _logger.LogInformation("geting refset data started");
            List<RefSetResponseDto> response = _addressBookServices.GetRefSetData(key);
            if(response == null)
            {
                return NotFound(new ErrorDTO
                {
                    type="key",
                    description=key +" key not exists in database"
                });
            }
            if(response.Count == 0)
            {
                return NoContent();
            }
            return Ok(response);   
        }
    }
}

