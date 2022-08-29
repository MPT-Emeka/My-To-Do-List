using System.ComponentModel.DataAnnotations;
namespace My_To_Do_List.Models
{
    public abstract class ToDoListForManipulationDto
    {
        [Required(ErrorMessage = "Task cannot be empty")]
        [MaxLength(100, ErrorMessage = "The task title shouldn't have more than 100 characters")]
        public string Task { get; set; }

        [MaxLength(500, ErrorMessage = "The task description shouldn't have more than 500 characters")]
        public virtual string Description { get; set; }
    }
}
