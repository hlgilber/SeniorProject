
namespace GraceChurchKelseyvilleAwana.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class Attendance
    {
        [Key, Column(Order=0)]
        [DataType (DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd}")]
        public System.DateTime AttendanceDate { get; set; }

        public bool Attended { get; set; }

        [Key, Column(Order=1)]
        public int StudentID { get; set; }

        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }
    }
}
