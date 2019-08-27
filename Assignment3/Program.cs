using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    class Program
    {
        static string connectionString = @"Server = DESKTOP-HHSU5LE\SQLEXPRESS01;Database = School; Trusted_Connection = True;";

        static SqlConnection sqlConnection = new SqlConnection(connectionString);

        static void Main(string[] args)       //TRY username = h and password = m , if you want to log in as a head master, 
        {                                         //username = d, password = d for a specific student
            using (sqlConnection)
            {
                sqlConnection.Open();

                try
                {
                    var school = new School();

                    var dbreader = new DBReader(sqlConnection);
                    dbreader.GetAllCourses(school);
                    dbreader.GetAllAssignments(school);
                    dbreader.GetUsers(school);
                    dbreader.GetTrainers(school);
                    dbreader.GetStudents(school);
                    dbreader.GetAllScedules(school);
                    dbreader.GetAllStudentsPerCourses(school);
                    dbreader.GetAllTrainersPerCourses(school);
                    dbreader.GetAllMarks(school);

                    var menu = new Menu(sqlConnection);
                    menu.LogIn(school);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }         
        }
                      
    }
}
