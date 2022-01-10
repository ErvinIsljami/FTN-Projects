using StudentServiceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRole.Models;

namespace WebRole.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListAll()
        {
            StudentDataRepository repo = new StudentDataRepository();
            
            return View(repo.RetrieveAllStudents());
        }

        public ActionResult InputStudent()
        {
            return View();
        }

        public ActionResult Add(StudentModel model)
        {
            Random r = new Random();
            Student student = new Student(r.Next(1000).ToString());
            student.LastName = model.LastName;
            student.Name = model.Name;

            StudentDataRepository repo = new StudentDataRepository();
            repo.AddStudent(student);



            return RedirectToAction("ListAll");
        }
    }
}