using My_To_Do_List.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace My_To_Do_List.Services
{
    public class LibraryRepository : ILibraryRepository
    {
        private LibraryContext _context;

        public LibraryRepository(LibraryContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            user.Id = Guid.NewGuid();
            _context.Users.Add(user);

            // the repository fills the id (instead of using identity columns)
            if (user.ToDoLists.Any())
            {
                foreach (var toDoList in user.ToDoLists)
                {
                    toDoList.Id = Guid.NewGuid();
                }
            }
        }

        public void AddToDoListForUser(Guid userId, ToDoList toDoList)
        {
            var user = GetUser(userId);
            if (user != null)
            {
                // if there isn't an id filled out (ie: we're not upserting),
                // we should generate one
                if (toDoList.Id == Guid.Empty)
                {
                    toDoList.Id = Guid.NewGuid();
                }
                user.ToDoList.Add(toDoList);
            }
        }

        public bool UserExists(Guid userId)
        {
            return _context.Users.Any(a => a.Id == userId);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public void DeleteToDoList(ToDoList toDoList)
        {
            _context.ToDoLists.Remove(toDoList);
        }

        public User GetUser(Guid userId)
        {
            return _context.Users.FirstOrDefault(a => a.Id == userId);
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.OrderBy(a => a.FirstName).ThenBy(a => a.LastName);
        }

        public IEnumerable<User> GetUsers(IEnumerable<Guid> userIds)
        {
            return _context.Users.Where(a => userIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public void UpdateUser(User user)
        {
            // an void implementation
        }

        public ToDoList GetToDoListForUser(Guid userId, Guid toDoListId)
        {
            return _context.ToDoLists
              .Where(b => b.UserId == userId && b.Id == toDoListId).FirstOrDefault();
        }

        public IEnumerable<ToDoList> GetToDoListsForUser(Guid userId)
        {
            return _context.ToDoLists
                        .Where(b => b.UserId == userId).OrderBy(b => b.Task).ToList();
        }

        public void UpdateBookForAuthor(ToDoList toDoList)
        {
            // a void implementation
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
        
    
}
