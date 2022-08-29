using My_To_Do_List.Entities;
using System;
using System.Collections.Generic;


namespace My_To_Do_List.Services
{
    public interface ILibraryRepository
    {

        IEnumerable<User> GetUsers();
        User GetUser(Guid UserId);
        IEnumerable<User> GetUsers(IEnumerable<Guid> userIds);
        void AddUser(User user);
        void DeleteUser(User user);
        void UpdateUser(User user);
        bool UserExists(Guid userId);
        IEnumerable<ToDoList> GetToDoListsForUser(Guid userId);
        ToDoList GetToDoListForUser(Guid userId, Guid toDoListId);
        void AddToDoListForUser(Guid userId, ToDoList toDoList);
        void UpdateToDoListForUser(ToDoList toDoList);
        void DeleteToDoList(ToDoList toDoList);
        bool Save();   

    }
}
