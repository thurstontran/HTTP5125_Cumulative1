using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTP5125_Cumulative1.Models;

namespace HTTP5125_Cumulative1.Controllers {
    public class TeacherController : Controller {
        //GET: Teacher
        public ActionResult Index() {
            return View();
        }

        //GET: localhost:xx/Teacher/List --> returns a page listing teachers in the system
        public ActionResult List(string SearchKey = null) {
            List<Teacher> Teachers = new List<Teacher>();
            TeacherDataController controller = new TeacherDataController();
            Teachers = controller.ListTeachers(SearchKey);

            //navigate to Views/Teacher/List.cshtml
            return View(Teachers);
        }

        //GET : localhost:xx/Teacher/Show/{TeacherId} --> Show a particular teacher matching that ID
        public ActionResult Show(int id) {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);

            //navigate to Views/Teacher/Show.cshtml
            return View(NewTeacher);
        }
    }
}