using GraceChurchKelseyvilleAwana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraceChurchKelseyvilleAwana.Controllers
{
    public class AttendanceController : Controller
    {
        private const int NUMBER_OF_WEEKS_TO_SHOW = 3;
        private const int DAYS_IN_WEEK = 7;
        private DateTime _lastAwanaDate;
        private DateTime _lastDateToShow;
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Attendance/
        public ActionResult Index(Int32? page)
        {
            _lastAwanaDate = LastAwanaDate();
            _lastDateToShow = _lastAwanaDate.AddDays(-(DAYS_IN_WEEK * (NUMBER_OF_WEEKS_TO_SHOW - 1)));

            var attendances = db.Attendances.ToList();
            var students = StudentsUserHasAccessTo();
            GenerateAttendancesIfNeeded(_lastDateToShow, _lastAwanaDate, students, attendances);

            var allAttendances = students.SelectMany(x => x.Attendances).OrderByDescending(x => x.AttendanceDate).ThenBy(x => x.Student.LastName).ThenBy(x => x.Student.FirstName).ToList();
            var pageList = new PagedList.PagedList<Attendance>(allAttendances, page ?? 1, NUMBER_OF_WEEKS_TO_SHOW * students.Count);
            return View(pageList/*new AttendanceViewModel { Students = students, Attendances = pageList }*/);
        }
/*
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            var attendanceList = form.GetValues(0);
            var students = db.Students.OrderBy(x => x.LastName).ThenBy(x => x.FirstName);
            var i = 0;

            foreach (var student in students)
            {
                foreach (var attendance in student.Attendances.OrderByDescending(a => a.AttendanceDate))
                {
                    var attended = bool.Parse(attendanceList.ElementAt(i++));
                    if (attended)
                    {
                        i++;
                    }
                    attendance.Attended = attended;
                }
            }
            db.SaveChanges();

            return RedirectToAction("index");
        }
*/
        [HttpPost]
        public ActionResult Index(ICollection<Attendance> attendances)
        {
            foreach (var attendance in attendances)
            {
                var attendanceToUpdate = db.Attendances.Where(x => x.AttendanceDate.Equals(attendance.AttendanceDate) && x.StudentID == attendance.StudentID).First();
                attendanceToUpdate.Attended = attendance.Attended;
            }

            db.SaveChanges();

            return RedirectToAction("index");
        }

        public DateTime LastAwanaDate()
        {
            var lastAwanaDate = DateTime.Today;

            while (!lastAwanaDate.DayOfWeek.Equals(Constants.DayOfAwana))
            {
                lastAwanaDate = lastAwanaDate.AddDays(-1.0);
            }

            return lastAwanaDate;
        }

        //Return value indicates whether any attendances were added
        public bool GenerateAttendancesIfNeeded(DateTime startDate, DateTime finishDate,
            List<Student> students, List<Attendance> existingAttendances)
        {
            var currentDate = startDate;
            var attendancesAdded = false;

            while (currentDate <= finishDate)
            {
                foreach (var student in students)
                {
                    if (!(existingAttendances.Exists(x => x.StudentID == student.StudentID
                        && x.AttendanceDate.Equals(currentDate))))
                    {
                        attendancesAdded = true;
                        student.Attendances.Add(new Attendance
                        {
                            StudentID = student.StudentID,
                            AttendanceDate = currentDate,
                            Attended = false
                        });
                    }
                }
                currentDate = currentDate.AddDays(DAYS_IN_WEEK);
            }
            if (attendancesAdded)
            {
                db.SaveChanges();
            }
            return attendancesAdded;
        }

        // Unimplimented for now
        public List<Student> StudentsUserHasAccessTo()
        {
            //User.IsInRole("Admin"/"Director"/"Leader")
            return db.Students.ToList();
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