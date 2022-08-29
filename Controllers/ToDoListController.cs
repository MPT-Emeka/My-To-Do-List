using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using My_To_Do_List.Models;
using Microsoft.AspNetCore.Mvc;
using My_To_Do_List.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.Extensions.Logging;
using My_To_Do_List.Services;

namespace My_To_Do_List.Controllers
{
    [Route("api/users/{userId}/toDoLists")]
    public class ToDoListController : Controller
    {
        private ILogger<ToDoListController> _logger;

        private ILibraryRepository _libraryRepository;


        public ToDoListController(ILibraryRepository libraryRepository,
            ILogger<ToDoListController> logger)
        {
            _logger = logger;

            _libraryRepository = libraryRepository;
        }

        [HttpGet()]
        public IActionResult GetToDoListsForUser(Guid userId)
        {
            if (!_libraryRepository.UserExists(userId))
            {
                return NotFound();
            }

            var toDoListsForUserFromRepo = _libraryRepository.GetToDoListsForUser(userId);

            var toDoListsForUser = Mapper.Map<IEnumerable<ToDoListDto>>(toDoListsForUserFromRepo);

            return Ok(toDoListsForUser);
        }

        [HttpGet("{id}", Name = "GetToDoListForUser")]
        public IActionResult GetToDoListForUser(Guid userId, Guid id)
        {
            if (!_libraryRepository.UserExists(userId))
            {
                return NotFound();
            }
            var toDoListForUserFromRepo = _libraryRepository.GetToDoListForUser(userId, id);

            if (toDoListForUserFromRepo == null)
            {
                return NotFound();
            }
        }

        [HttpPost()]
        public IActionResult CreateToDoListForUser(Guid userId, [FromBody] ToDoListForCreationDto toDoList)
        {
            if (toDoList == null)
            {
                return BadRequest();
            }

            if (toDoList.Description == toDoList.Task)
            {
                ModelState.AddModelError(nameof(ToDoListForCreationDto),
                    "The description provided must be different from the task itself");
            } 

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState); 
            } 

            if (!_libraryRepository.UserExists(userId))
            {
                return NotFound();
            }

            var toDoListEntity = Mapper.Map<ToDoList>(toDoList);

            _libraryRepository.AddToDoListForUser(userId, toDoListEntity);

            if (!_libraryRepository.Save())
            {
                throw new Exception($" The creation operation failed on save");
            }

            var toDoListToReturn = Mapper.Map<ToDoListDto>(toDoListEntity);

            return CreatedAtRoute("GetToDoListForUser", new { userId = userId, id = toDoListToReturn.Id }, toDoListToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteToDoListForUser(Guid userId, Guid id)
        {
            if (!_libraryRepository.GetUser(userId))
            {
                return NotFound();
            }

            var toDoListForUserFromRepo = _libraryRepository.GetToDoListForUser(userId, id);
            if (toDoListForUserFromRepo == null)
            {
                return NotFound();
            }

            _libraryRepository.DeleteToDoList(toDoListForUserFromRepo);
            if (!_libraryRepository.Save())
            {
                throw new Exception(_logger.LogInformation(100, $"todolist {id} for user {userId} was deleted.");
            }
                 return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateToDoListForUser(Guid userId, Guid id, [FromBody] 
        ToDoListForUpdateDto toDoList)
        {
            if(toDoList == null)
            {
                return BadRequest();
            }

            if (toDoList.Description == toDoList.Task)
            {
                ModelState.AddModelError(nameof(ToDoListForUpdateDto), "The description should be different from the task content");
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState); 
            }

            if (!_libraryRepository.UserExists(userId))
            {
                return NotFound();
            }

            var toDoListForUserFromRepo = _libraryRepository.GetToDoListForUser(userId, id);
            if (toDoListForUserFromRepo == null)
            {
                var toDoListToAdd = Mapper.Map<ToDoList>(toDoList);
                toDoListToAdd.Id = id;

                _libraryRepository.AddToDoListForUser(userId, toDoListToAdd);
            }

            // two lines of code below to check ,
            Mapper.Map(toDoList, toDoListForUserFromRepo);
            _libraryRepository.UpdateToDoListForUser(toDoListForUserFromRepo);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Updating a todolist {id} for user {userId} failed on save");
            }

            return NoContent(); 
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateToDoListForUser(Guid userId, Guid id, [FromBody] JsonPatchDocument<ToDoListForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_libraryRepository.UserExists(userId))
            {
                return NotFound();
            }

            var toDoListForUserFromRepo = _libraryRepository.GetToDoListForUser(userId, id);
            
            if (toDoListForUserFromRepo == null)
            {
                var toDoListDto = new ToDoListForUpdateDto();
                patchDoc.ApplyTo(toDoListDto, ModelState);

                if (toDoListDto.Description == toDoListDto.Task)
                {
                    ModelState.AddModelError(nameof(ToDoListForUpdateDto),
                        "The provided description should be different from the task.");
                }

                TryValidateModel(toDoListDto);

                if (!ModelState.IsValid)
                {
                    return new UnprocessableEntityObjectResult(ModelState);
                }



                var toDoListToAdd = Mapper.Map<ToDoList>(toDoListDto);
                toDoListToAdd.Id = id;

                _libraryRepository.AddToDoListForUser(userId, toDoListToAdd);
               
                if (!_libraryRepository.Save())
                {
                    throw new Exception($"Upserting a todolist {id} for user {userId} failed on save");
                }
                var toDoListToReturn = Mapper.Map<ToDoListDto>(toDoListToAdd);

                return CreatedAtRoute("GetToDoListForUser", 
                    new { userId = userId, id = toDoListToReturn.Id }, 
                    toDoListToReturn);
            }

            var toDoListToPatch = Mapper.Map<ToDoListForUpdateDto>(toDoListForUserFromRepo);
            
            patchDoc.ApplyTo(toDoListToPatch);    // ModelState, I removed the ModelState from the applyTo method


            if (toDoListToPatch.Description == toDoListToPatch.Task)
            {
                ModelState.AddModelError(nameof(ToDoListForUpdateDto),
                    "The provided description should be different from task.");
            }

            TryValidateModel(toDoListToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }
            
            // add validation
            
            Mapper.Map(toDoListToPatch, toDoListForUserFromRepo);
            _libraryRepository.UpdateToDoListForUser(toDoListForUserFromRepo);
            if (!_libraryRepository.Save())
            {
                throw new Exception($"Patching todolist {id} for user {userId} failed on save");
            }
            return NoContent(); 
        }
    }
}





