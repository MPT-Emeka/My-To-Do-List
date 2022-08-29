using System;
using System.Collections.Generic;


namespace My_To_Do_List.Entities
{
    public static class LibraryContextExtensions
    {
            public static void EnsureSeedDataForContext(this LibraryContext context)
            {
                // this is not proper for production environments.

                context.Users.RemoveRange(context.Users);
                context.SaveChanges();

                var users = new List<User>()
                {
                    new User()
                    {
                        Id = new Guid("42670c4d-f56a-5b1f-b46a-8ee05a830bdf"),
                        FirstName = "Emmanuel",
                        LastName = "Odoala",
                        Email = "ifeanyie31@yahoo.com",
                        ToDoLists = new List<ToDoList>()
                        {
                            new ToDoList()
                            {
                                Id = new Guid("52670c3d-f43a-4b1f-b43a-8ee04a860bcf"),
                                Task = "Cook paki rice and Grocery",
                                Description = "I will cook paki rice by 6pm, and Go to the store by 8pm."
                            },

                            new ToDoList()
                            {
                                Id = new Guid("32570c4d-f36a-4c1f-b36a-8de04a870bcf"),
                                Task = "Study",
                                Description = "Study C# and JavaScript by 10pm."
                            }
                        }
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();






            }
       
    }
}
