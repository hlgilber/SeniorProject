using GraceChurchKelseyvilleAwana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraceChurchKelseyvilleAwana.Controllers
{
    public class GroupsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /Groups/
        public ActionResult Index()
        {
            var leaders = GetLeaders();
            return View(new GroupsViewModel { Leaders = leaders });
        }

        //
        // GET: /Groups/Grouping/id
        public ActionResult Grouping(string id)
        {
            var leader = db.Users.First(x => x.Id.Equals(id));

            var assignedStudents = new List<StudentCheckBox>();
            foreach(var student in leader.Students.OrderBy(x => x.LastName).ThenBy(x => x.FirstName))
            {
                assignedStudents.Add(new StudentCheckBox { student = student });
            }
            var unassignedStudents = new List<StudentCheckBox>();
            foreach(var student in db.Students.Where(x => x.Leader == null).OrderBy(x => x.LastName).ThenBy(x => x.FirstName))
            {
                unassignedStudents.Add(new StudentCheckBox { student = student });
            }

            return View(new GroupingViewModel { Leader = leader, AssignedStudents = assignedStudents, UnassignedStudents = unassignedStudents });
        }

        public ApplicationUser[] GetLeaders()
        {
            var leaders = new List<ApplicationUser>();

            foreach (var user in db.Users)
            {
                if (user.Role.Equals(AwanaRoles.Leader))
                {
                    leaders.Add(user);
                }
            }

            return leaders.ToArray();
        }

        [HttpPost]
        public ActionResult Assign(GroupingViewModel vm)
        {
            foreach (var student in vm.UnassignedStudents.Where(x => x.IsChecked))
            {
                var actualStudent = db.Students.First(x => x.StudentID == student.student.StudentID);
                actualStudent.LeaderID = vm.Leader.Id;
            }
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        public ActionResult Unassign(GroupingViewModel vm)
        {
            foreach (var student in vm.AssignedStudents.Where(x => x.IsChecked))
            {
                var actualStudent = db.Students.First(x => x.StudentID == student.student.StudentID);
                actualStudent.LeaderID = null;
            }
            db.SaveChanges();
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}