using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GraceChurchKelseyvilleAwana.Models;
using PagedList;

namespace GraceChurchKelseyvilleAwana.Controllers
{
    public class StudentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private const int STUDENT_PAGE_SIZE = 10;

        // GET: /Student/
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, Int32? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            var students = GetAvailableStudents();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                    || s.FirstName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName).ThenBy(s => s.FirstName);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName).ThenBy(s => s.FirstName);
                    break;
            }

            int pageSize = STUDENT_PAGE_SIZE;

            //If not logged in do this:
            //return RedirectToAction("Index", "Home");
            return View(students.ToPagedList((page ?? 1), pageSize));
        }

        public IQueryable<Student> GetAvailableStudents()
        {
            var students = from s in db.Students select s;
            if (User.IsInRole(AwanaRoles.Leader.ToString()))
            {
                var user = ApplicationUser.GetFromUserIdentity(User.Identity);
                students = students.Where(x => x.LeaderID.Equals(user.Id));
            }
            else if (User.IsInRole(AwanaRoles.Admin.ToString()))
            {
                //Admin gets all students
            }
            else if (User.IsInRole(AwanaRoles.Director.ToString()))
            {
                // TODO: Add directors implementation
                //accessibleStudents = db.Students.ToList();
            }
            else if (User.IsInRole(AwanaRoles.Parent.ToString()))
            {
                // TODO: Add parents implementation
                //accessibleStudents = db.Students.ToList();
            }

            return students;
        }

        // GET: /Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: /Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentID,FirstName,LastName,BirthDate,Grade,Gender,MedicalInformation,EmailAddress")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: /Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,FirstName,LastName,BirthDate,Grade,Gender,MedicalInformation,EmailAddress")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: /Student/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
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
