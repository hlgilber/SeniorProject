﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

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
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
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

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public AwanaRoles TempRole
        {
            get;
            set;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
    }
}