using System.Collections.Generic;
namespace My_To_Do_List.Models
{
    public class UserForCreationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Task { get; set; }
        public ICollection<ToDoListForCreationDto> ToDoLists { get; set; }
        = new List<ToDoListForCreationDto>();

    }
}
