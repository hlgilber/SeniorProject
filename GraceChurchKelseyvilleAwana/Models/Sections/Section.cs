using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GraceChurchKelseyvilleAwana.Models.Sections
{
    public class Section
    {
        public int SectionID { get; set; }

        public int ChapterID { get; set; }

        public string BookID { get; set; }

        public virtual Chapter Chapter { get; set; }

        public virtual ICollection<SectionCompletion> Completions { get; set; }
    }
}