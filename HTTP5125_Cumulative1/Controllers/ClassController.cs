using HTTP5125_Cumulative1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTTP5125_Cumulative1.Controllers {
    public class ClassController : Controller {
        // GET: Class
        public ActionResult Index() {
            return View();
        }        

        //GET: localhost:xx/Class/List --> returns a page listing courses in the system
        public ActionResult List(string SearchKey = null) {
            List<Class> Classes = new List<Class>();
            ClassDataController controller = new ClassDataController();
            Classes = controller.ListClasses(SearchKey);

            //navigate to Views/Class/List.cshtml
            return View(Classes);
        }

        //GET : localhost:xx/Class/Show/{ClassId} --> Show a particular class matching that ID
        public ActionResult Show(int id) {
            ClassDataController controller = new ClassDataController();
            Class SelectedClass = controller.FindClass(id);

            //navigate to Views/Class/Show.cshtml
            return View(SelectedClass);
        }
    }
}