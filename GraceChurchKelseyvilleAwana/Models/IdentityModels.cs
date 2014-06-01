using GraceChurchKelseyvilleAwana.Models.Sections;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Principal;

namespace GraceChurchKelseyvilleAwana.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }

        //Operates under the assumption that a user can never have more than one role
        [NotMapped]
        public AwanaRoles Role
        {
            get
            {
                using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
                {
                    var roles = userManager.GetRoles(Id);
                    AwanaRoles returnRole;
                    Enum.TryParse<AwanaRoles>(roles[0], out returnRole);
                    return returnRole;
                }
            }
            set
            {
                using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
                {
                    userManager.RemoveFromRole(Id, userManager.GetRoles(Id)[0]);
                    userManager.AddToRole(Id, value.ToString());
                }
            }
        }

        [NotMapped]
        public AwanaRoles TempRole
        {
            get;
            set;
        }

        public virtual ICollection<Student> Students { get; set; }

        public static ApplicationUser GetFromUserIdentity(System.Security.Principal.IIdentity identity)
        {
            using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
            {
                return userManager.FindById(identity.GetUserId());
            }
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>().HasOptional<ApplicationUser>(s => s.Leader).WithMany(s => s.Students).HasForeignKey(s => s.LeaderID);
            modelBuilder.Entity<Chapter>().HasKey(c => new { c.ChapterID, c.BookID}).HasRequired<Book>(c => c.Book).WithMany(b => b.Chapters).HasForeignKey(c => c.BookID);
            modelBuilder.Entity<Section>().HasKey(s => new { s.SectionID, s.ChapterID, s.BookID }).HasRequired<Chapter>(s => s.Chapter).WithMany(c => c.Sections).HasForeignKey(s => new { s.ChapterID, s.BookID });
            modelBuilder.Entity<SectionCompletion>().HasKey(sc => new { sc.StudentID, sc.BookID, sc.ChapterID, sc.SectionID }).HasRequired(sc => sc.Student).WithMany(s => s.SectionsCompleted).HasForeignKey(s => new { s.StudentID })
                .WillCascadeOnDelete();
            modelBuilder.Entity<SectionCompletion>().HasRequired(sc => sc.Section).WithMany(s => s.Completions).HasForeignKey(sc => new { sc.SectionID, sc.ChapterID, sc.BookID });
        }

        public List<Student> StudentsUserHasAccessTo(IPrincipal User)
        {
            List<Student> accessibleStudents = new List<Student>();
            if (User.IsInRole(AwanaRoles.Leader.ToString()))
            {
                var user = ApplicationUser.GetFromUserIdentity(User.Identity);
                accessibleStudents = user.Students.ToList();
            }
            else if (User.IsInRole(AwanaRoles.Admin.ToString()))
            {
                accessibleStudents = Students.ToList();
            }
            else if (User.IsInRole(AwanaRoles.Director.ToString()))
            {
                // TODO: Add directors implementation
                accessibleStudents = Students.ToList();
            }
            else if (User.IsInRole(AwanaRoles.Parent.ToString()))
            {
                // TODO: Add parents implementation
                //accessibleStudents = db.Students.ToList();
            }

            return accessibleStudents;
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<Book> Books { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Section> Sections { get; set; }
    }
}