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
        private const int NUMBER_OF_WEEKS_TO_SHOW = 4;
        private const int DAYS_IN_WEEK = 7;
        private DateTime _lastAwanaDate;
        private DateTime _lastDateToShow;
        private ApplicationDbContext db = new ApplicationDbContext();
        private Int32? _lastPage;

        // GET: /Attendance/
        public ActionResult Index(Int32? page)
        {
            _lastAwanaDate = LastAwanaDate();
            _lastDateToShow = Constants.AwanaStartDate;//_lastAwanaDate.AddDays(-(DAYS_IN_WEEK * (NUMBER_OF_WEEKS_TO_SHOW - 1)));

            var students = StudentsUserHasAccessTo();
            var attendances = students.SelectMany(x => x.Attendances).ToList();
            GenerateAttendancesIfNeeded(_lastDateToShow, _lastAwanaDate, students, attendances);

            var allAttendances = students.SelectMany(x => x.Attendances).OrderByDescending(x => x.AttendanceDate).ThenBy(x => x.Student.LastName).ThenBy(x => x.Student.FirstName).ToList();
            var pageList = new PagedList.PagedList<Attendance>(allAttendances, page ?? 1, NUMBER_OF_WEEKS_TO_SHOW * students.Count);

            var statisticsList = new List<AttendanceStatistics>();
            var totalWeeks = ((_lastAwanaDate - Constants.AwanaStartDate).Days / DAYS_IN_WEEK) + 1;

            foreach (var student in students)
            {
                var attendanceRate = (float)student.Attendances.Count(x => x.Student.Equals(student) && x.Attended) / totalWeeks;
                var lastAttendance = student.Attendances.OrderByDescending(x => x.AttendanceDate).FirstOrDefault(x => x.Attended);
                var weeksSinceLastAttendance = (_lastAwanaDate - (lastAttendance != null ? lastAttendance.AttendanceDate : Constants.AwanaStartDate)).Days / DAYS_IN_WEEK;

                statisticsList.Add(new AttendanceStatistics
                    {
                        AttendanceStatisticsStudent = student,
                        AttendanceRate = attendanceRate,
                        WeeksSinceLastAttendance = weeksSinceLastAttendance
                    });
            }

            _lastPage = page;
            return View(new AttendanceViewModel { Attendances = pageList, Statistics = statisticsList });
        }

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


        public List<Student> StudentsUserHasAccessTo()
        {
            List<Student> accessibleStudents = new List<Student>();
            if (User.IsInRole(AwanaRoles.Leader.ToString()))
            {
                var user = ApplicationUser.GetFromUserIdentity(User.Identity);
                accessibleStudents = user.Students.ToList();
            }
            else if (User.IsInRole(AwanaRoles.Admin.ToString()))
            {
                accessibleStudents = db.Students.ToList();
            }
            else if (User.IsInRole(AwanaRoles.Director.ToString()))
            {
                // TODO: Add directors implementation
                accessibleStudents = db.Students.ToList();
            }
            else if (User.IsInRole(AwanaRoles.Parent.ToString()))
            {
                // TODO: Add parents implementation
                //accessibleStudents = db.Students.ToList();
            }

            return accessibleStudents;
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