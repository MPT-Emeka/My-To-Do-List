using AutoMapper;
using My_To_Do_List.Assist_Folder;
using My_To_Do_List.Models;
using My_To_Do_List.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace My_To_Do_List.Controllers
{   
    [Route("api/users")]
    public class UsersController : Controller
    {
        private ILibraryRepository _libraryRepository;
        public UsersController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }
        

       [HttpGet()]
       public IActionResult GetUsers()
        {
            var usersFromRepo = _libraryRepository.GetUsers();

            var users = Mapper.Map<IEnumerable<UserDto>>(usersFromRepo);


            return Ok(users);                                  // new JsonResult(usersFromRepo); 
        }

        [HttpGet("{id}", Name = "GetUser")] 
        public IActionResult GetUser(Guid id)
        {

            var userFromRepo = _libraryRepository.GetUser(id);

            var user = Mapper.Map<UserDto>(userFromRepo);

            if (userFromRepo == null)                // if(!_libraryRepository.UserExists(id)) => {return NotFound();}
            {
                return NotFound();
            }
            return Ok(user);
        }
        
        // private ILibraryRepository _libraryRepository 


        [HttpPost()]
        public IActionResult CreateUser([FromBody] UserForCreationDto user)
        {
            if(user == null)
            {
                return BadRequest();
            }
            var userEntity = Mapper.Map<User>(user);

            _libraryRepository.AddUser(userEntity);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"The creation operation failed on save");
            }

            var userToReturn = Mapper.Map<UserDto>(userEntity);

            return CreatedAtRoute("GetUser", new { id = userToReturn.Id }, userToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockUserCreation(Guid id)
        {
            if (!_libraryRepository.UserExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
            return NotFound(); 
        }

        public IActionResult DeleteUser(Guid id)
        {
            var userFromRepo = _libraryRepository.GetUser(id);
            if (userFromRepo == null)
            {
                return NotFound();
            }

            _libraryRepository.DeleteUser(userFromRepo);
            if (!_libraryRepository.Save())
            {
                throw new Exception($"Deleting a user {id} failed on save"); 
            }
            return NoContent(); 

        }
    }
}
