using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Assignment3
{
    class DBReader //CLASS RESPONSIBLE TO READ ALL DATA FROM ALL TABLES OF DATABASE 
    {
        readonly SqlConnection sqlConnection;

        public DBReader(SqlConnection connection)
        {
            sqlConnection = connection;
        }


        public void GetUsers(School school)
        {
            try
            {
                SqlCommand userTypes = new SqlCommand("SELECT UserDetails.ID,UserType.Type,UserDetails.UserName, UserDetails.PassWord " +
               "FROM UserType INNER JOIN UserDetails ON UserDetails.UserTypeID = UserType.ID", sqlConnection);
                SqlDataReader readerUsers = userTypes.ExecuteReader();

                while (readerUsers.Read())
                {
                    var id = readerUsers.GetInt32(0);
                    var newUser = new User((UserType)Enum.Parse(typeof(UserType), readerUsers.GetString(1)), readerUsers.GetString(2), readerUsers.GetString(3));
                    newUser.SetID(id);
                    school.AddUser(newUser);
                }
                readerUsers.Close();
            }
            catch(Exception)
            {
                Console.WriteLine("Something went wrong with the UserDetails...Try later!");
            }
           
        }

        public void GetAllCourses(School school)
        {
            SqlCommand cmdCourses = new SqlCommand("SELECT * FROM Course", sqlConnection);
            SqlDataReader readerCourses = cmdCourses.ExecuteReader();
            while (readerCourses.Read())
            {
                int id = readerCourses.GetInt32(0);
                var stringaki = String.Empty;
                if (readerCourses.GetBoolean(1))
                {
                    stringaki = Stream.Java.ToString();
                }
                else
                {
                    stringaki = Stream.Csharp.ToString();
                }
                var stringara = String.Empty;
                if (readerCourses.GetBoolean(2))
                {
                    stringara = TimeSchedule.Part.ToString();
                }
                else
                {
                    stringara = TimeSchedule.Full.ToString();
                }
                var newCourse = new Course( (Stream)Enum.Parse(typeof(Stream), stringaki, readerCourses.GetBoolean(1)), (TimeSchedule)Enum.Parse(typeof(TimeSchedule), stringara, readerCourses.GetBoolean(2)), readerCourses.GetDateTime(3), readerCourses.GetDateTime(4), readerCourses.GetString(5));
                newCourse.SetID(id);
                school.AddCourse(newCourse);
                
            }
            readerCourses.Close();
        }

        public void GetTrainers(School school)
        {
            SqlCommand cmdTrainers = new SqlCommand("SELECT * FROM Trainer", sqlConnection);
            SqlDataReader readerTrainers = cmdTrainers.ExecuteReader();
            while (readerTrainers.Read())
            {
                int id = readerTrainers.GetInt32(0);
                var userID = readerTrainers.GetInt32(5);
                var newTrainer = new Trainer( readerTrainers.GetString(1), readerTrainers.GetString(2), readerTrainers.GetString(3));
                newTrainer.SetID(id);
                var user = school.Users.SingleOrDefault(t => t.ID == userID);
                newTrainer.SetUser(user);
                school.AddTrainer(newTrainer);
            }
            readerTrainers.Close();
        }

        public void GetStudents(School school)
        {
            SqlCommand cmdStudents = new SqlCommand("SELECT * FROM Student", sqlConnection);
            SqlDataReader readerStudents = cmdStudents.ExecuteReader();

            

            while (readerStudents.Read())
            {
                int id = readerStudents.GetInt32(0);
                int userID = readerStudents.GetInt32(5);
                var newStudent = new Student( readerStudents.GetString(1), readerStudents.GetString(2), readerStudents.GetDateTime(3));
                newStudent.SetID(id);
                var user = school.Users.SingleOrDefault(u => u.ID == userID);
                //newStudent.SetUserID(userID);
                newStudent.SetUser(user);
                school.Students.Add(newStudent);
                
            }
            readerStudents.Close();
        }

        public void GetAllAssignments(School school)
        {
            SqlCommand cmdAssignmnets = new SqlCommand("SELECT * FROM Assignment", sqlConnection);
            SqlDataReader readerAssignments = cmdAssignmnets.ExecuteReader();

            while (readerAssignments.Read())
            {
                var id = readerAssignments.GetInt32(0);
                var courseID = readerAssignments.GetInt32(1);
                var newAssignmnet = new Assignment(  readerAssignments.GetString(2), readerAssignments.GetDateTime(3));
                newAssignmnet.SetAssignmentID(id);
                var course = school.Courses.SingleOrDefault(c => c.ID == courseID);
                newAssignmnet.SetCourse(course);
                school.Assignments.Add(newAssignmnet);
            }
            readerAssignments.Close();
        }

        public void GetAllStudentsPerCourses(School school)
        {
            SqlCommand cmdStudentPerCourse = new SqlCommand("SELECT * FROM StudentPerCourse", sqlConnection);
            SqlDataReader readerStudentsPerCourses = cmdStudentPerCourse.ExecuteReader();

            while (readerStudentsPerCourses.Read())
            {
                // ONE FROM THE STUDENTS WITH LINQ
                Student student = school.Students.SingleOrDefault(s => s.ID == readerStudentsPerCourses.GetInt32(0));
                //ONE FROM THE COURSES WITH LINQ
                Course course = school.Courses.SingleOrDefault(c => c.ID == readerStudentsPerCourses.GetInt32(1));
                var newStudentPerCourse = new StudentPerCourse(student, course);
                student.Courses.Add(newStudentPerCourse);
                course.Students.Add(newStudentPerCourse);
                school.StudentsPerCourses.Add(newStudentPerCourse);
            }

            readerStudentsPerCourses.Close();

            //var newLista = school.StudentsPerCourses.Select(spc => $"Course is  : {spc.Course} and student is : {spc.Student}").ToList();
        }

        public void GetAllTrainersPerCourses(School school)
        {
            SqlCommand cmdTrainerPerCourse = new SqlCommand("SELECT * FROM TrainerPerCourse", sqlConnection);
            SqlDataReader readerTrainerPerCourses = cmdTrainerPerCourse.ExecuteReader();

            while (readerTrainerPerCourses.Read())
            {
                Trainer trainer = school.Trainers.SingleOrDefault(t => t.ID == readerTrainerPerCourses.GetInt32(0));
                Course course = school.Courses.SingleOrDefault(c => c.ID == readerTrainerPerCourses.GetInt32(1));
                var newTrainerPerCourse = new TrainerPerCourse(trainer, course);
                trainer.Courses.Add(newTrainerPerCourse);
                course.Trainers.Add((newTrainerPerCourse));
                school.TrainersPerCourses.Add(newTrainerPerCourse);
            }
            readerTrainerPerCourses.Close();
        }

        public void GetAllMarks(School school)
        {
            SqlCommand cmdMarks = new SqlCommand("SELECT * FROM Mark", sqlConnection);
            SqlDataReader readerMarks = cmdMarks.ExecuteReader();

            while (readerMarks.Read())
            {
                Student student = school.Students.SingleOrDefault(s => s.ID == readerMarks.GetInt32(0));
                Assignment assignment = school.Assignments.SingleOrDefault(a => a.ID == readerMarks.GetInt32(1));
                var isSubmitted = readerMarks.GetBoolean(4);
                var newMark = new Mark(student, assignment,isSubmitted);

                if(newMark.IsSubmitted)
                {
                    newMark.SetOralMark(readerMarks.GetInt32(2));
                    newMark.SetTotalMark(readerMarks.GetInt32(3));
                }
                student.Marks.Add(newMark);
                assignment.Marks.Add(newMark);
                school.Marks.Add(newMark);
            }

            readerMarks.Close();
        }

        public void GetAllScedules(School school)
        {
            SqlCommand selectSchedules = new SqlCommand("SELECT * FROM Schedule", sqlConnection);
            SqlDataReader scheduleReader = selectSchedules.ExecuteReader();

            while (scheduleReader.Read())
            {
                Course course = school.Courses.SingleOrDefault(c => c.ID == scheduleReader.GetInt32(1));
                int dayOfTheWeekNum = scheduleReader.GetInt32(3);
                Schedule schedule = new Schedule(scheduleReader.GetString(2), (DayOfWeek) dayOfTheWeekNum);
                schedule.SetID(scheduleReader.GetInt32(0));
                schedule.SetCourse(course);
                
                course.Schedules.Add(schedule);
                school.Schedules.Add(schedule);

            }
            scheduleReader.Close();

        }

    }
}
