using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My_To_Do_List.Entities
{
    public class ToDoList
    {
        [key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Task { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid UserId { get; set; }

    }
}
