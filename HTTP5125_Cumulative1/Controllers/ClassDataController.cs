using HTTP5125_Cumulative1.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace HTTP5125_Cumulative1.Controllers {
    public class ClassDataController : ApiController {
        //The database class to allow for access to the MySQL Database;
        private SchoolDbContext School = new SchoolDbContext();

        ///<summary>
        ///List the classes in the system
        ///</summary>
        ///<param name="id">the class ID in the database</param>
        ///<returns> A list of the classes</returns>
        ///<example>
        ///GET api/ClassData/ListClasses
        ///</example>
        [HttpGet]
        [Route("api/ClassData/ListClasses/{SearchKey?}")]

        public List<Class> ListClasses(string SearchKey = null) {
            //Create an instance of a connection
            MySqlConnection Conn = School.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            string query = "SELECT * FROM classes WHERE classcode LIKE lower(@key) OR classname LIKE lower(@key)";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Gather Result Set of Query in to a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Classes
            List<Class> Classes = new List<Class>();

            //Loop through each row of the expected result set
            while (ResultSet.Read()) {
                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassCode = ResultSet["classcode"].ToString();
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                string ClassName = ResultSet["classname"].ToString();

                Class NewClass = new Class();
                NewClass.ClassId = ClassId;
                NewClass.ClassCode = ClassCode;
                NewClass.TeacherId = TeacherId;
                NewClass.StartDate = StartDate;
                NewClass.FinishDate = FinishDate;
                NewClass.ClassName = ClassName;

                Classes.Add(NewClass);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of teacher names
            return Classes;
        }

        ///<summary>
        /// Find the teacher in the system given an ID
        ///</summary>
        ///<param name="id">The class ID in the database</param>
        ///<returns>The class object</returns>
        [HttpGet]
        [Route("api/ClassData/FindClass/{id}")]
        public Class FindClass(int id) {
            Class NewClass = new Class();
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            //Join the teachers and the classes table to display teachers information associated with classes
            String query = "SELECT classes.*, teachers.teacherfname, teachers.teacherlname FROM classes LEFT OUTER JOIN teachers ON teachers.teacherid = classes.teacherid WHERE classes.classid = @id";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read()) {
                int ClassId = Convert.ToInt32(ResultSet["classid"]);
                string ClassCode = ResultSet["classcode"].ToString();
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);
                string ClassName = ResultSet["classname"].ToString();
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();

                NewClass.ClassId = ClassId;
                NewClass.ClassCode = ClassCode;
                NewClass.TeacherId = TeacherId;
                NewClass.StartDate = StartDate;
                NewClass.FinishDate = FinishDate;
                NewClass.ClassName = ClassName;
                NewClass.TeacherFname = TeacherFname;
                NewClass.TeacherLname = TeacherLname;
            }
            Conn.Close();

            return NewClass;
        }
    }
}
