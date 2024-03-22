using System;
using System.Collections.Generic;
using System.Web.Http;
using HTTP5125_Cumulative1.Models;
using MySql.Data.MySqlClient;

namespace HTTP5125_Cumulative1.Controllers {
    public class TeacherDataController : ApiController {
        //The database class to allow for access to the MySQL Database;
        private SchoolDbContext School = new SchoolDbContext();

        ///<summary>
        ///List the teachers in the system
        ///</summary>
        ///<param name="id">the teacher's ID in the database</param>
        ///<returns> A list of the teachers</returns>
        ///<example>
        ///GET api/TeacherData/ListTeachers
        ///</example>
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public List<Teacher> ListTeachers(string SearchKey = null) {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            string query = "SELECT * FROM teachers WHERE teacherfname LIKE lower(@key) OR teacherlname LIKE lower(@key) OR lower(CONCAT(teacherfname, ' ', teacherlname)) LIKE lower(@key) OR hiredate LIKE lower(@key) OR salary LIKE lower(@key)";
            cmd.CommandText = query;

            //Define what the @key is
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query in to a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Teacher Names
            List<Teacher> Teachers = new List<Teacher>();

            //Loop through each row of the expected result set
            while (ResultSet.Read()) {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNum = ResultSet["employeenumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                Decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNum = EmployeeNum;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

                Teachers.Add(NewTeacher);
            }

            //Close the connection between the MySQL Database and the web server
            Conn.Close();

            //Return the final list of teacher names
            return Teachers;
        }

        ///<summary>
        /// Find the teacher in the system given an ID
        ///</summary>
        ///<param name="id">The teacher ID in the database</param>
        ///<returns>The teacher object</returns>
        [HttpGet]
        [Route("api/TeacherData/FindTeacher/{id}")]
        public Teacher FindTeacher(int id) {

            Teacher NewTeacher = new Teacher();
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            //Simply select alll the teacher information when identified by their id
            string query = "SELECT * FROM teachers WHERE teacherid = " + id;
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read()) {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNum = ResultSet["employeenumber"].ToString();
                DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNum = EmployeeNum;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;
            }

            Conn.Close();

            MySqlConnection NewConn = School.AccessDatabase();
            //Open a new connection between the web server and database
            NewConn.Open();

            //Establish a new SQL query for our database
            MySqlCommand NewCmd = NewConn.CreateCommand();

            //Join the teachers and classes table to display class information associated by teacher
            string NewQuery = "SELECT teachers.*, classes.classcode, classes.classname, classes.classid FROM teachers LEFT OUTER JOIN classes ON classes.teacherid = teachers.teacherid WHERE classes.teacherid = @teacherid";
            NewCmd.CommandText = NewQuery;

            //Define the what @teacherid is
            NewCmd.Parameters.AddWithValue("@teacherid", id);
            NewCmd.Prepare();

            //Populate a list to store the classes taught by the teacher
            List<string> ClassList = new List<string>();

            ResultSet = NewCmd.ExecuteReader();

            while(ResultSet.Read()) {
                //if course data exists, add it to the list and display the corresponding course information into the list of classes
                if (ResultSet["classname"] != null)  {
                    string classes = ResultSet["classcode"].ToString() + " - " + ResultSet["classname"].ToString();
                    ClassList.Add(classes);
                }
            }
 
            NewTeacher.Classes = ClassList;

            //Close the new connection
            NewConn.Close();

            return NewTeacher;
        }
    }
}
