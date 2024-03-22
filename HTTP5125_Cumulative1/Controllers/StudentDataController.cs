using HTTP5125_Cumulative1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace HTTP5125_Cumulative1.Controllers {
    public class StudentDataController : ApiController {
        //The database class to allow for access to the MySQL Database;
        private SchoolDbContext School = new SchoolDbContext();

        ///<summary>
        ///List the students in the system
        ///</summary>
        ///<param name="id">the student's ID in the database</param>
        ///<returns> A list of names of the students</returns>
        ///<example>
        ///GET api/StudentData/ListStudents
        ///</example>
        [HttpGet]
        [Route("api/StudentData/ListStudents/{SearchKey?}")]
        public List<Student> ListStudents(string SearchKey = null) {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            string query = "SELECT * FROM students WHERE studentfname LIKE lower(@key) OR studentlname LIKE lower(@key) OR lower(concat(studentfname, ' ', studentlname)) LIKE lower(@key)";
            cmd.CommandText = query;

            //Define what the @key is
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query in to a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Student Names
            List<Student> Students = new List<Student>();

            //Loop through each row of the expected result set
            while (ResultSet.Read()) {
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                string StudentNum = ResultSet["studentnumber"].ToString();
                DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                Student NewStudent = new Student();
                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentNum = StudentNum;
                NewStudent.EnrolDate = EnrolDate;

                Students.Add(NewStudent);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of student names
            return Students;
        }

        ///<summary>
        /// Find the student in the system given an ID
        ///</summary>
        ///<param name="id">The student ID in the database</param>
        ///<returns>The student object</returns>
        [HttpGet]
        [Route("api/StudentData/FindStudent/{id}")]
        public Student FindStudent(int id) {
            Student NewStudent = new Student();
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            //Simply select select all the student information when identified by their id
            string query = "SELECT * FROM students WHERE studentid = " + id;
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read()) {
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                string StudentNum = ResultSet["studentnumber"].ToString();
                DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentNum = StudentNum;
                NewStudent.EnrolDate = EnrolDate;
            }

            MySqlConnection NewConn = School.AccessDatabase();
            //Open a new connection between the web server and database
            NewConn.Open();

            //Establish a new SQL query for our database
            MySqlCommand NewCmd = NewConn.CreateCommand();

            //Join the studentsxclasses table with the students table as well as classes table to display the information associated with students enrolled courses
            string NewQuery = "SELECT classes.classcode, classes.classname FROM students LEFT OUTER join studentsxclasses ON studentsxclasses.studentid=students.studentid JOIN classes ON classes.classid=studentsxclasses.classid WHERE students.studentid=@studentid";
            NewCmd.CommandText = NewQuery;
            NewCmd.Parameters.AddWithValue("@studentid", id);
            NewCmd.Prepare();

            //Populate a list to store the classes enrolled by the student
            List<string> ClassList = new List<string>();

            ResultSet = NewCmd.ExecuteReader();

            while (ResultSet.Read()) {

                //if course data exists, add it to the list and display the corresponding course information into the list of classes
                if (ResultSet["classname"] != null) {
                    string classes = ResultSet["classcode"].ToString()+" - "+ResultSet["classname"].ToString();
                    ClassList.Add(classes);
                 }
            }

            NewStudent.Classes = ClassList;

            //Close the new connection
            NewConn.Close();

            return NewStudent;
        }
    }
}
