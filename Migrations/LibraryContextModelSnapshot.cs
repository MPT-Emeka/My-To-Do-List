using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using My_To_Do_List.Entities;

namespace My_To_Do_List.Migrations
{   
    [DbContext(typeof(LibraryContext))]
    partial class LibraryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1") // check product version
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn); // check code

            modelBuilder.Entity("My_To_Do_List.Entities.User", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                //  b.Property<DateTimeOffset>("DateOfBirth");


                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasAnnotation("MaxLength", 50);

                b.Property<string>("Email")
                    .IsRequired()
                    .HasAnnotation("MaxLength", 50);

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasAnnotation("MaxLength", 50);

                b.HasKey("Id");

                b.ToTable("Users");
            });

            modelBuilder.Entity("My_To_Do_List.Entities.ToDoList", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<Guid>("UserId");

                b.Property<string>("Description")
                    .HasAnnotation("MaxLength", 300);

                b.Property<string>("Task")
                    .IsRequired()
                    .HasAnnotation("MaxLength", 100);

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("ToDoLists");
            });

            modelBuilder.Entity("My_To_Do_List.Entities.ToDoList", b =>
            {
                b.HasOne("My_To_Do_List.Entities.User", "User")
                    .WithMany("ToDoLists")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }


    }
}
