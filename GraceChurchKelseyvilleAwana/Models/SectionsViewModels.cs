using GraceChurchKelseyvilleAwana.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraceChurchKelseyvilleAwana.Models
{
    public class IndexSectionsViewModel
    {
        public List<Book> Books { get; set; }
    }

    public class SectionsProgressListingViewModel
    {
        public List<Student> Students { get; set; }
    }

    public class TrackProgressViewModel
    {
        public List<BookCheckList> Books { get; set; }
        public Student Student { get; set; }
    }

    public class SectionCheckList
    {
        public Section Section { get; set; }
        public bool Completed { get; set; }
    }

    public class ChapterCheckList
    {
        public List<SectionCheckList> Sections { get; set; }
        public int ChapterNumber { get; set; }
    }

    public class BookCheckList
    {
        public List<ChapterCheckList> Chapters { get; set; }
        public string Title { get; set; }
    }
}