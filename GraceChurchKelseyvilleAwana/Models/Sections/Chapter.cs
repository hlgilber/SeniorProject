using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GraceChurchKelseyvilleAwana.Models.Sections
{
    public class Chapter
    {
//        [Key, Column(Order=1)]
        public string BookID { get; set; }

//        [Key, Column(Order=0)]
        public int ChapterID { get; set; }

        public virtual Book Book { get; set; }
        public virtual ICollection<Section> Sections { get; set; }
    }
}