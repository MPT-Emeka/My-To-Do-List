using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_To_Do_List.Entities
{
    public class User
    {
        [key]
        public Guid Id { get; set; }

        [Required] 
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public string Email { get; set; }

        public ICollection<ToDoList> ToDoLists { get; set; }
            = new List<ToDoList>();


    }
}
