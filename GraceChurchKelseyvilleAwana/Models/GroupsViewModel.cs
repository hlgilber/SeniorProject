using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraceChurchKelseyvilleAwana.Models
{
    public class GroupsViewModel
    {
        public ApplicationUser[] Leaders { get; set; }
    }

    public class GroupingViewModel
    {
        /*public ApplicationUser Leader { get; set; }
        public List<Student> UnassignedStudents { get; set; }*/

        public ApplicationUser Leader { get; set; }
        public List<StudentCheckBox> AssignedStudents { get; set; }
        public List<StudentCheckBox> UnassignedStudents { get; set; }
    }

    public class StudentCheckBox
    {
        public bool IsChecked { get; set; }
        public Student student { get; set; }
    }
}