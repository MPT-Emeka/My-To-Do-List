using System;
namespace My_To_Do_List.Models
{
    public class ToDoListDto
    {
        public Guid Id { get; set; }
        public string Task { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
    }
}
