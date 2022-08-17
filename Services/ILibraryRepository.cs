namespace My_To_Do_List.Services
{
    public interface ILibraryRepository
    {

        IEnumerable<User> GetUsers();
        User GetUser(Guid UserId);
        void AddUser(User user);
        void DeleteUser(User user);
        void UpdateUser(User user);
        bool UserExists(Guid userId);
        IEnumerable<ToDoList> GetToDoListsForUser(Guid userId);
        ToDoList GetToDoListForUser(Guid userId, Guid bookId);
        void AddToDoListForUser(Guid userId, ToDoList toDoList);
        void UpdateToDoListForUser(ToDoList toDoList);
        void DeleteToDoList(ToDoList toDoList);
        bool Save();   

    }
}
