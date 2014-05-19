
namespace GraceChurchKelseyvilleAwana.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class Student
    {
        public Student()
        {
            this.Attendances = new HashSet<Attendance>();
        }
    
        [Display (Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Student")]
        public string BothNames
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [DisplayFormat (DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> BirthDate { get; set; }

        public int Age
        {
            get
            {
                int age = 0;
                if (BirthDate.HasValue)
                {
                    var hasHadBirthdayThisYear = BirthDate.Value.CompareTo(DateTime.Today) >= 0;
                    age = (DateTime.Today.Year - BirthDate.Value.Year) + (hasHadBirthdayThisYear ? 0 : -1);
                }
                return age;
            }
        }

        public int Grade { get; set; }

        public string Gender { get; set; }

        [Display (Name = "Medical Information")]
        public string MedicalInformation { get; set; }

        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email address is not valid")]
        public string EmailAddress { get; set; }

        [Key]
        public int StudentID { get; set; }
    
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
