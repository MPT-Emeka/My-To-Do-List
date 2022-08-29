using System.ComponentModel.DataAnnotations;

namespace My_To_Do_List.Models
{
    public class ToDoListForUpdateDto : ToDoListForManipulationDto
    {   
        [Required(ErrorMessage = "Describe your task.")]
        public override string Description
        {
            get
            {
                return base.Description;
            }

            set
            {
                base.Description = value;   
            }
        }
    }
}
