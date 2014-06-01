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


        //private void GenerateTestBook()
        //{
        //    var testBook = new Book { BookID = "TestBook" };
        //    db.Books.Add(testBook);
        //    var testChapter1 = new Chapter { BookID = testBook.BookID, ChapterID = 1 };
        //    var testChapter2 = new Chapter { BookID = testBook.BookID, ChapterID = 2 };
        //    db.Chapters.Add(testChapter1);
        //    db.Chapters.Add(testChapter2);

        //    var testChapterSections = new List<Section>();
        //    for (int i = 0; i < 5; i++)
        //    {
        //        testChapterSections.Add(new Section { ChapterID = testChapter1.ChapterID, BookID = testChapter1.BookID, SectionID = i + 1 });
        //    }
        //    db.Sections.AddRange(testChapterSections);

        //    testChapterSections.Clear();
        //    for (int i = 0; i < 5; i++)
        //    {
        //        testChapterSections.Add(new Section { ChapterID = testChapter2.ChapterID, BookID = testChapter2.BookID, SectionID = i + 1 });
        //    }
        //    db.Sections.AddRange(testChapterSections);

        //    db.SaveChanges();
        //}


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