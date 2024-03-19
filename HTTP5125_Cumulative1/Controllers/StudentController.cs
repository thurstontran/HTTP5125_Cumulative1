using HTTP5125_Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTTP5125_Cumulative1.Controllers {
    public class StudentController : Controller {
        // GET: Student
        public ActionResult Index() {
            return View();
        }

        //GET: localhost:xx/Student/List --> returns a page listing students in the system
        public ActionResult List(string SearchKey = null) {
            List<Student> Students = new List<Student>();
            StudentDataController controller = new StudentDataController();
            Students = controller.ListStudents(SearchKey);

            //navigate to Views/Student/List.cshtml
            return View(Students);
        }

        //GET : localhost:xx/Student/Show/{StudentId} --> Show a particular student matching that ID
        public ActionResult Show(int id) {
            StudentDataController controller = new StudentDataController();
            Student NewStudent = controller.FindStudent(id);

            //navigate to Views/Student/Show.cshtml
            return View(NewStudent);
        }
    }
}