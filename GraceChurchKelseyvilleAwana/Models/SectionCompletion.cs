using GraceChurchKelseyvilleAwana.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraceChurchKelseyvilleAwana.Models
{
    public class SectionCompletion
    {
        public int StudentID { get; set; }

        public string BookID { get; set; }

        public int ChapterID { get; set; }

        public int SectionID { get; set; }

        public DateTime DateCompleted { get; set; }


        public virtual Student Student { get; set; }

        public virtual Section Section { get; set; }
    }
}