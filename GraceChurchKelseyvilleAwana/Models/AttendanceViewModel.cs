using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraceChurchKelseyvilleAwana.Models
{
    public class AttendanceViewModel
    {
        public PagedList.PagedList<Attendance> Attendances { get; set; }
        public List<AttendanceStatistics> Statistics { get; set; }

        public IEnumerable<Attendance> UniqueAttendanceDates()
        {
            return Attendances.Distinct(new UniqueDateComparer());
        }

        public class UniqueDateComparer : IEqualityComparer<Attendance>
        {
            public bool Equals(Attendance x, Attendance y)
            {
                return x.AttendanceDate.Equals(y.AttendanceDate);
            }

            public int GetHashCode(Attendance obj)
            {
                return obj.AttendanceDate.GetHashCode();
            }
        }
    }

    public class AttendanceStatistics
    {
        public Student AttendanceStatisticsStudent { get; set; }
        public float AttendanceRate { get; set; }
        public int WeeksSinceLastAttendance { get; set; }
    }
}