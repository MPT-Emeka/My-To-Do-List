using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

            if(usersFromRepo == null)
            {
                return NotFound(); 
            }

            return new JsonResult(usersFromRepo); 
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(Guid id)
        {
            var userFromRepo = _libraryRepository.GetUser(id);
            if(userFromRepo == null)
            {
                return NotFound();
            }
            return Ok(User);
        }
        
        // private ILibraryRepository _libraryRepository 



    }
}
