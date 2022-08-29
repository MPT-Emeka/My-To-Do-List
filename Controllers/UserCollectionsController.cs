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
    [Route("api/usercollections")]
    public class UserCollectionsController: Controller
    {
        private ILibraryRepository _libraryRepository;

            public UserCollectionsController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        [HttpPost] 
        public IActionResult CreateUserCollection([FromBody] IEnumerable<UserForCreationDto> userCollection)
        {
            if(userCollection == null)
            {
                return BadRequest();
            }

            var userEntities = Mapper.Map<IEnumerable<User>>(userCollection);
            foreach(var user in userEntities)
            {
                _libraryRepository.Add(user);
            }
            if (!_libraryRepository.Save())
            {
                throw new Exception("Creating a user collection failed on save");
            }

            var userCollectionToReturn = Mapper.Map<IEnumerable<UserDto>>(userEntities);
            var idsAsString = string.Join(",",
                userCollectionToReturn.Select(a => a.Id));

            return CreatedAtRoute("GetUserCollection",
                new { ids = idsAsString },
                userCollectionToReturn);
        }

        [HttpGet("({ids})", Name = "GetUserCollection")]
        public IActionResult GetUserCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var userEntities = _libraryRepository.GetUsers(ids);

            if (ids.Count()!= userEntities.Count())
            {
                return NotFound();
            }

            var usersToReturn = Mapper.Map<IEnumerable<UserDto>>(userEntities);
            return Ok(usersToReturn);   
        }



    }



}
