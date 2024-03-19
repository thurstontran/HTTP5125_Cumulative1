using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP5125_Cumulative1.Models
{
    public class Teacher
    {
        public int TeacherId;
        public string TeacherFname;
        public string TeacherLname;
        public string EmployeeNum;
        public DateTime HireDate;
        public decimal Salary;
        public List<string> Classes;


    }
}