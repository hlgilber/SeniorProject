using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GraceChurchKelseyvilleAwana.Models.Sections
{
    public class Book
    {
        [Key]
        public string BookID { get; set; }
        public AgeGroups AgeGroup { get; set; }

        public virtual ICollection<Chapter> Chapters { get; set; }
    }
}