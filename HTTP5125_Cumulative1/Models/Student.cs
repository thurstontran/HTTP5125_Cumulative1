using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP5125_Cumulative1.Models
{
    public class Student
    {
        public int StudentId;
        public string StudentFname;
        public string StudentLname;
        public string StudentNum;
        public DateTime EnrolDate;
        public List<string> Classes;
    }
}