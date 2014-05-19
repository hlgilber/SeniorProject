using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GraceChurchKelseyvilleAwana.Models;
using GraceChurchKelseyvilleAwana.Email;
using System.Threading.Tasks;

namespace GraceChurchKelseyvilleAwana.Controllers
{
    public class EmailController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Email/
        public ActionResult Index(bool confirm = false)
        {
            return View(new EmailViewModel { Confirm = confirm });
        }

        [HttpPost]
        public ActionResult Index(EmailViewModel vm)
        {
            if (vm.Recipients.Equals("Leaders"))
            {
                EmailLeaders(vm);
            }
            else if (vm.Recipients.Equals("Students"))
            {
                EmailStudents(vm);
            }

            return RedirectToAction("Index", new { @confirm = true });
        }

        //TODO: Add full implementation
        private void EmailStudents(EmailViewModel vm)
        {
            foreach (var student in db.Students.Where(s => !string.IsNullOrEmpty(s.EmailAddress)))
            {
                EmailHelper.SendEmail("Grace Church Awana Announcement", vm.EmailBody, student.EmailAddress);
            }
        }

        //TODO: Add full implementation
        private void EmailLeaders(EmailViewModel vm)
        {
            foreach(var leader in db.Users.Where(s => !string.IsNullOrEmpty(s.EmailAddress)))
            {
                EmailHelper.SendEmail("Grace Church Awana Announcement", vm.EmailBody, leader.EmailAddress);
            }
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