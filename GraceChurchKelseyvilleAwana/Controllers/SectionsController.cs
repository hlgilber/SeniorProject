using GraceChurchKelseyvilleAwana.Models;
using GraceChurchKelseyvilleAwana.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraceChurchKelseyvilleAwana.Controllers
{
    public class SectionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /Sections/Index
        public ActionResult Index()
        {
            if (!User.IsInRole(AwanaRoles.Admin.ToString()))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new IndexSectionsViewModel { Books = db.Books.ToList() });
        }

        //
        // GET: /Sections/Progress
        public ActionResult Progress()
        {
            var students = db.StudentsUserHasAccessTo(User);

            return View(new SectionsProgressListingViewModel { Students = students });
        }

        //
        // GET: /Sections/TrackProgress/studentID
        public ActionResult TrackProgress(int id)
        {
            var books = db.Books.ToList();
            var student = db.Students.First(s => s.StudentID == id);
            var completedSections = student.SectionsCompleted;

            var bookCheckList = new List<BookCheckList>();
            foreach (var book in books)
            {
                var chapterCheckList = new List<ChapterCheckList>();
                foreach (var chapter in book.Chapters)
                {
                    var sectionCheckList = new List<SectionCheckList>();
                    foreach(var section in chapter.Sections)
                    {
                        sectionCheckList.Add(new SectionCheckList { Section = section, Completed = completedSections.FirstOrDefault(c => c.Section.Equals(section)) != null});
                    }
                    chapterCheckList.Add(new ChapterCheckList { ChapterNumber = chapter.ChapterID, Sections = sectionCheckList});
                }
                bookCheckList.Add(new BookCheckList { Title = book.BookID, Chapters = chapterCheckList});
            }

            return View(new TrackProgressViewModel { Books = bookCheckList, Student = student });
        }

        [HttpPost]
        public ActionResult TrackProgress(TrackProgressViewModel vm)
        {
            var allSections = vm.Books.SelectMany(b => b.Chapters.SelectMany(c => c.Sections));
            var completedSections = allSections.Where(s => s.Completed);
            var student = db.Students.First(s => s.StudentID == vm.Student.StudentID);

            foreach(var completedSection in completedSections)
            {
                var sectionCompletion = ToSectionCompletion(completedSection, student);

                if (student.SectionsCompleted.FirstOrDefault(s => s.BookID.Equals(sectionCompletion.BookID) && s.ChapterID == sectionCompletion.ChapterID && s.SectionID == sectionCompletion.SectionID) == null)
                {

                    student.SectionsCompleted.Add(sectionCompletion);
                }
            }

            db.SaveChanges();

            return RedirectToAction("Progress");
        }



        public SectionCompletion ToSectionCompletion(SectionCheckList selectionCheckList, Student student)
        {
            var Section = db.Sections.First(s => s.BookID.Equals(selectionCheckList.Section.BookID) && s.ChapterID == selectionCheckList.Section.ChapterID && s.SectionID == selectionCheckList.Section.SectionID);
            return new SectionCompletion { Student = student, BookID = Section.BookID, ChapterID = Section.ChapterID, SectionID = Section.SectionID, StudentID = student.StudentID, DateCompleted = DateTime.Today };
        }

        //
        // GET: /Sections/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Sections/Create()
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            var chapters = form.AllKeys.Where(s => s.Contains("section"));
            var numChapters = chapters.Count();
            string bookName = form.GetValue("bookName").AttemptedValue;

            var book = new Book { BookID = bookName, AgeGroup = (AgeGroups)Enum.Parse(typeof(AgeGroups), form.GetValue("ageGroup").AttemptedValue, true) };
            db.Books.Add(book);
            
            var counter = 1;
            foreach (var chapter in chapters)
            {
                var dbChapter = new Chapter { BookID = book.BookID, ChapterID = counter++ };
                db.Chapters.Add(dbChapter);

                int numSections = int.Parse(form.GetValue(chapter).AttemptedValue);
                for (int i = 0; i < numSections; i++)
                {
                    db.Sections.Add(new Section { ChapterID = dbChapter.ChapterID, BookID = dbChapter.BookID, SectionID = i + 1 });
                }
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //
        // GET: /Sections/Delete/bookId
        [HttpGet]
        public ActionResult Delete(string bookId)
        {
            db.Books.Remove(db.Books.Find(bookId));
            db.SaveChanges();

            return RedirectToAction("Index");
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