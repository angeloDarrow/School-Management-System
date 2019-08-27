using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    class Menu
    {
        //FIELD
        private SqlConnection sqlConnection;

        //CONSTRUCTOR
        public Menu(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        //LOG IN METHOD
        public void LogIn(School school)
        {
            if (school == null)
            {
                throw new ArgumentNullException("Something went wrong, try again later...");
            }
            Console.WriteLine("\nHello! Please enter your username and password to Log In\n");
           
            var allGood = true;
            do
            {
                // TRY CATCH IN CASE USER IS NULL
                 //LINQ TO GET THE USER
                    Console.Write("Username :");
                    var userName = Console.ReadLine();
                    userName = SHA.GenerateSHA512String(userName);
                    Console.Write("Password :");
                    var passWord = Console.ReadLine();
                    passWord = SHA.GenerateSHA512String(passWord);
                    User user = school.Users.SingleOrDefault(u => u.UserName == userName && u.PassWord == passWord);
                if (user == null)
                {
                    allGood = false;
                    Console.WriteLine("There is no user with this username or password, please try again");
                }
                else
                {
                    switch (user.Type)
                    {
                        case UserType.HeadMaster:
                            GetHeadMasterMainMenu(school);
                            break;
                        case UserType.Student:
                            StudentMainMenu(school, user);
                            break;
                        case UserType.Trainer:
                            TrainerMainMenu(school, user);
                            break;
                    }
                }
            } while (!allGood);
        }

        //HEADMASTER MAIN MENU
        private void GetHeadMasterMainMenu(School school)
        {
            Console.Clear();
            Console.WriteLine("\nWelcome headmaster!\nYou have these options(Press the corresponding number)\n ");
            Console.WriteLine("1 : CRUD ON COURSES\n2 : CRUD ON STUDENTS\n3 : CRUD ON StUDENTPERCOURSE\n4 : CRUD ON TRAINERS\n" +
                "5 : CRUD ON TRAINERS PER COURSES\n6 : CRUD ON ASSIGNMENTS\n7 : CRUD ON MARKS\n8 : CRUD ON SCHEDULE PER COURSES\n9 : LOG OUT");

            var option = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 9);

            switch (option)
            {
                case 1:
                    CrudCourse(school);
                    break;
                case 2:
                    CrudStudent(school);
                    break;
                case 3:
                    CrudStudentPerCourse(school);
                    break;
                case 4:
                    CrudTrainer(school);
                    break;
                case 5:
                    CrudTrainerPerCourse(school);
                    break;
                case 6:
                    CrudAssignment(school);
                    break;
                case 7:
                    CrudMarks(school);
                    break;
                case 8:
                    CrudSchedules(school);
                    break;
                case 9:
                    LogIn(school);
                    break;
            }

        }
        //                                                              --------------------COURSE CRUD--------------------------
        private void CrudCourse(School school)
        {
            Console.Clear();
            Console.WriteLine("\nChoose the corrensponded number\n");
           
           
                Console.WriteLine("1 : CREATE\n2 : VIEW\n3 : UPDATE\n4 : DELETE\n5 : GO TO PREVIOUS MENU");
             
                var crudOption = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 5);
                switch (crudOption)
                {
                    case 1:
                        Console.WriteLine("You chose create");
                        CreateCourse(school);
                        break;
                    case 2:
                        Console.WriteLine("You chose view");
                        ViewCourses(school);
                        break;
                    case 3:
                        Console.WriteLine("You chose update");
                        ChooseCourseForEdit(school);
                        break;
                    case 4:
                        Console.WriteLine("You chose delete");
                        DeleteCourse(school);
                        break;
                    case 5:
                        GetHeadMasterMainMenu(school);
                        break;
                }
        }

        private void CreateCourse(School school)
        {
            Console.WriteLine("Course Stream(Type 0 for Java,1 for Csharp) :");
            var stream = InputConstraint.BitConstraint(Console.ReadLine());
            Console.WriteLine("Time Schedule(Type 0 for full, 1 for Part) : ");
            var timeSchedule = InputConstraint.BitConstraint(Console.ReadLine());
            Console.WriteLine("Start Date(mm/dd/yyyy) : ");
            var startDate = InputConstraint.DateTimeConstraint(Console.ReadLine());
            Console.WriteLine("End Date(mm/dd/yyyy) : ");
            var endDate = InputConstraint.DateTimeConstraint(Console.ReadLine());
            Console.WriteLine("Course Title : ");
            var title = InputConstraint.StringConstraint(Console.ReadLine());

            Course course = new Course((Stream) stream, (TimeSchedule) timeSchedule, startDate, endDate, title);
            SqlCommand cmdInsert = new SqlCommand($"INSERT INTO Course(Stream,TimeSchedule,StartDate,EndDate,Title) VALUES('{stream}','{timeSchedule}','{startDate}','{endDate}','{title}')", sqlConnection);
            int insertedRows = cmdInsert.ExecuteNonQuery();
            //an isxuei to apo katw, tote exv potelsma
            if (insertedRows > 0)
            {
                Console.WriteLine("Insertion Succesful");
            }

            SqlCommand cmdSelect = new SqlCommand("SELECT MAX(Course.ID) FROM Course", sqlConnection);
            SqlDataReader selectReader = cmdSelect.ExecuteReader();
            while (selectReader.Read())
            {
                course.SetID(selectReader.GetInt32(0));
            }
            selectReader.Close();
            school.Courses.Add(course);

            CrudCourse(school);
        }

        private void ViewCourses(School school)
        {
            Console.Clear();
            Console.WriteLine("Courses : \n");
            school.Courses.ForEach(c => Console.WriteLine(c));
            Console.WriteLine("Press enter to go to previous menu");
            while (!String.IsNullOrEmpty(Console.ReadLine()))
            {
                var inpout = Console.ReadLine();
            }
            CrudCourse(school);
        }

        private void  ChooseCourseForEdit(School school)
        {
            Console.Clear();
            //LINQ TO TAKE MIN AND MAX IDS
            var minID = school.Courses.Min(c => c.ID);
            var maxID = school.Courses.Max(c => c.ID);

            Console.WriteLine("/nChoose which course to update by its ID\n");
            school.Courses.ForEach(c => Console.WriteLine(c));
            Console.WriteLine("\nChoose the course you want to edit by its ID and press the corresponded number : ");
            var id = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minID, maxID);

            Course course = school.Courses.Single(c => c.ID == id);

            Console.Clear();
            EditCourseMenu(school,course);            
        }

        private void EditCourseMenu(School school,Course course)
        {
            Console.Clear();
            Console.WriteLine("\nChoose what you want to update(Press the corresponded number)\n\n1 : STREAM\n2 : TIME SCHEDULE\n" +
                "3 : START DATE\n4 : END DATE\n5 : TITLE\n6 : GO TO PREVIOUS MENU ");
            var editOption = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 6);

            switch (editOption)
            {
                case 1:
                    EditStreamCourse(school,course);
                    break;
                case 2:
                    EditTimeScheduleCourse(school, course);
                    break;
                case 3:
                    EditStartDate(school, course);
                    break;
                case 4:
                    EditEndDate(school, course);
                    break;
                case 5:
                    EditTitle(school, course);
                    break;
                case 6:
                    CrudCourse(school);
                    break;
            }
        }

        private void EditStreamCourse(School school,Course course)
        {
            Console.WriteLine(course);
            Console.WriteLine("\nGive me the new stream(Type 0 for Java, 1 for Csharp) : ");
            var newStream = InputConstraint.BitConstraint(Console.ReadLine());
            SqlCommand updateStream = new SqlCommand($"UPDATE Course SET Stream = '{newStream}' WHERE ID = '{course.ID}'", sqlConnection);
    
            int rowsUpdated = updateStream.ExecuteNonQuery();
            course.SetStream((Stream) newStream);

            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }

           

        }


        private void EditTimeScheduleCourse(School school,Course course)
        {
            Console.WriteLine(course);

            Console.WriteLine("\nGive me the new TimeSchedule(Type 0 for Full, 1 for Part) : ");
            var newTimeSchedule = InputConstraint.BitConstraint(Console.ReadLine());
            SqlCommand updateTimeSchedule = new SqlCommand($"UPDATE Course SET TimeSchedule = '{newTimeSchedule}' WHERE ID = '{course.ID}'", sqlConnection);
            int rowsUpdated = updateTimeSchedule.ExecuteNonQuery();

            course.SetTimeSchedule((TimeSchedule) newTimeSchedule);

            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }

            EditCourseMenu(school, course);
        }

        private void EditStartDate(School school,Course course)
        {
            Console.WriteLine(course);

            Console.WriteLine("\nGive me the new Start Date(mm/dd/yyyy) : ");
            var newDateTime = InputConstraint.DateTimeConstraint(Console.ReadLine());
            SqlCommand updateStartDate = new SqlCommand($"UPDATE Course SET StartDate ='{newDateTime}' WHERE ID = '{course.ID}'", sqlConnection);

            course.SetStartDate(newDateTime);

            int rowsUpdated = updateStartDate.ExecuteNonQuery();
            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }

            EditCourseMenu(school, course);
        }

        private void EditEndDate(School school,Course course)
        {
            Console.WriteLine(course);

            Console.WriteLine("\nGive me the new End Date(mm/dd/yyyy) : ");
            var newDateTime = InputConstraint.DateTimeConstraint(Console.ReadLine());
            SqlCommand updateEndDate = new SqlCommand($"UPDATE Course SET EndDate ='{newDateTime}' WHERE ID = '{course.ID}'", sqlConnection);

            course.SetEndDate(newDateTime);

            int rowsUpdated = updateEndDate.ExecuteNonQuery();
            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }
            EditCourseMenu(school, course);
        }

        private void EditTitle(School school,Course course)
        {
            Console.WriteLine(course);

            Console.WriteLine("\nGive me the new Title : ");
            var newTitle = InputConstraint.StringConstraint(Console.ReadLine());
            SqlCommand updateTitle = new SqlCommand($"UPDATE Course SET Title = '{newTitle}' WHERE ID = '{course.ID}'", sqlConnection);

            course.SetTitle(newTitle);

            int rowsUpdated = updateTitle.ExecuteNonQuery();
            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }
            EditCourseMenu(school, course);
        }

        private void DeleteCourse(School school)
        {
            Console.Clear();

 
            var minID = school.Courses.Min(c => c.ID);
            var maxID = school.Courses.Max(c => c.ID);
            Console.WriteLine("/nChoose which course to delete by its ID\n");
            school.Courses.ForEach(c => Console.WriteLine(c));
            Console.WriteLine("Choose the course you want to delete by its ID and press the corresponded number : ");
            var id = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(),minID,maxID);

            Course course = school.Courses.SingleOrDefault(c => c.ID == id);
            SqlCommand deleteCourseFromStudentPerCourse = new SqlCommand($"DELETE FROM StudentPerCourse WHERE CourseID = '{course.ID}'", sqlConnection);
            deleteCourseFromStudentPerCourse.ExecuteNonQuery();

            SqlCommand deleteCourseFromTrainerPerCourse = new SqlCommand($"DELETE FROM TrainerPerCourse WHERE CourseID = '{course.ID}'", sqlConnection);
            deleteCourseFromTrainerPerCourse.ExecuteNonQuery();



            //INNER JOIN EDW
            SqlCommand deleteMarks = new SqlCommand("DELETE * FROM Mark INNER JOIN Assignment ON Assignment.ID = Mark.AssignmentID INNER JOIN Course ON Course.ID = Assignment.CourseID WHERE" +
                $"Course.ID = '{course.ID}'");
            deleteMarks.ExecuteNonQuery();
            SqlCommand deleteAssignments = new SqlCommand($"DELETE FROM Assignment WHERE CourseID = '{course.ID}'",sqlConnection);
            deleteAssignments.ExecuteNonQuery();
            

            SqlCommand deleteSchedules = new SqlCommand($"DELETE FROM SCHEDULE WHERE CourseID = '{course.ID}'", sqlConnection);
            deleteSchedules.ExecuteNonQuery();

            //var 

            //SqlCommand deleteAssignments =  new SqlCommand($"DELETE FROM Assignment WHERE CourseID = '{course.ID}'", sqlConnection);
            //deleteAssignments.ExecuteNonQuery();

            SqlCommand deleteCourse = new SqlCommand($"DELETE FROM Course WHERE ID = '{course.ID}'", sqlConnection);
             deleteCourse.ExecuteNonQuery();
        

            school.Courses.Remove(course);
            school.StudentsPerCourses.RemoveAll(spc => spc.Course.ID == course.ID);
            school.TrainersPerCourses.RemoveAll(tpc => tpc.Course.ID == course.ID);
            school.Assignments.RemoveAll(a => a.Course == course);
            school.Marks.RemoveAll(m => m.Assignment.Course == course);

            CrudCourse(school);
        }
                                              // ----------------------------------------CRUD STUDENT----------------------------------------//

        private void CrudStudent(School school)
        {
            Console.Clear();
            Console.WriteLine("\nChoose the corrensponded number\n");


            Console.WriteLine("1 : CREATE\n2 : VIEW\n3 : UPDATE\n4 : DELETE\n5 : GO TO PREVIOUS MENU");

            var crudOption = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 5);
            switch (crudOption)
            {
                case 1:
                    CreateStudent(school);
                    break;
                case 2:
                    ViewStudents(school);
                    break;
                case 3:
                    ChooseStudentForEdit(school);
                    break;
                case 4:
                    DeleteStudent(school);
                    break;
                case 5:
                    GetHeadMasterMainMenu(school);
                    break;
            }
        }

        private void CreateStudent(School school)
        {
            Console.Clear();
            Console.WriteLine("First name :");
            var firstName = InputConstraint.StringConstraint(Console.ReadLine());
            Console.WriteLine("Last name : ");
            var lastName = InputConstraint.StringConstraint(Console.ReadLine());
            Console.WriteLine("Date of birth : ");
            var dateOfBirth = InputConstraint.DateTimeConstraint(Console.ReadLine());
            Console.WriteLine("Give me a username for the online platform : ");
            var userName = InputConstraint.UsernameCheck(Console.ReadLine(),school);
            Console.WriteLine("Give me a passWord for the oline platform :");
            var passWord = InputConstraint.PasswordCheck(Console.ReadLine(), school);

            User user = new User(UserType.Student, userName, passWord);
                
            Student student = new Student(firstName, lastName, dateOfBirth);

            SqlCommand cmdInsertUser = new SqlCommand($"INSERT INTO UserDetails(UserTypeID,UserName,PassWord) VALUES('{3}','{userName}','{passWord}')", sqlConnection);
            int inserRows = cmdInsertUser.ExecuteNonQuery();

            SqlCommand cmdSelectUserID = new SqlCommand("SELECT MAX(UserDetails.ID) FROM UserDetails", sqlConnection);
            SqlDataReader selectReaderUserID = cmdSelectUserID.ExecuteReader();
            while (selectReaderUserID.Read())
            {
                user.SetID(selectReaderUserID.GetInt32(0));
            }
            selectReaderUserID.Close();

            SqlCommand cmdInsert = new SqlCommand($"INSERT INTO Student(FirstName,LastName,DateBirth,UserID) VALUES('{firstName}','{lastName}','{dateOfBirth}','{user.ID}')", sqlConnection);

            
            int insertedRows = cmdInsert.ExecuteNonQuery();
            //an isxuei to apo katw, tote exv potelsma
            if (insertedRows > 0)
            {
                Console.WriteLine("Insertion Succesful");
            }
            

            SqlCommand cmdSelect = new SqlCommand("SELECT MAX(Student.ID) FROM Student", sqlConnection);
            SqlDataReader selectReader = cmdSelect.ExecuteReader();
            while (selectReader.Read())
            {
                student.SetID(selectReader.GetInt32(0));
            }
            selectReader.Close();

          
            student.SetUser(user);
            school.AddUser(user);
            school.Students.Add(student);

            CrudStudent(school);
        }

        private void ViewStudents(School school)
        {
            Console.Clear();
            Console.WriteLine("Students : \n");
            school.Students.ForEach(c => Console.WriteLine(c));
            Console.WriteLine("\nPress enter to go to previous menu");
            while (!String.IsNullOrEmpty(Console.ReadLine()))
            {
                var inpout = Console.ReadLine();
            }
            CrudStudent(school);
        }

        private void ChooseStudentForEdit(School school)
        {
            Console.Clear();

            //SQL TO CHOOSE THE MIN AND MAX ID
            var minID = school.Students.Min(s => s.ID);
            var maxID = school.Students.Max(s => s.ID);


            Console.WriteLine("/nChoose which student to update by its ID\n");
            school.Students.ForEach(c => Console.WriteLine(c));
            Console.WriteLine("\nChoose the student you want to edit by its ID and press the corresponded number : ");
            var id = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minID, maxID);

            Student student = school.Students.Single(s => s.ID == id);

            Console.Clear();
            EditStudentMenu(school, student);
        }

        private void EditStudentMenu(School school, Student student)
        {
            Console.Clear();
            Console.WriteLine("\nChoose what you want to update(Press the corresponded number)\n\n1 : FIRST NAME\n2 : LAST NAME\n" +
                "3 : DATE OF BIRTH\n4 : STUDENT USERNAME\n5 : STUDENT PASSWORD\n6 : GO TO PREVIOUS MENU ");
            var editOption = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 6);

            switch (editOption)
            {
                case 1:
                    EditFirstNameStudent(school, student);
                    break;
                case 2:
                    EditLastNameStudent(school, student);
                    break;
                case 3:
                    EditDateOfBirth(school, student);
                    break;
                case 4:
                    EditStudentUserName(school, student);
                    break;
                case 5:
                    EditStudentPassword(school, student);
                    break;
                case 6:
                    CrudStudent(school);
                    break;
      
            }
        }

        private void EditFirstNameStudent(School school,Student student)
        {
            Console.WriteLine(student);
            Console.WriteLine("\nGive me the new first Name : ");
            var newName = InputConstraint.StringConstraint(Console.ReadLine());
            SqlCommand updateFirstName = new SqlCommand($"UPDATE Student SET FirstName = '{newName}' WHERE ID = '{student.ID}'", sqlConnection);

            int rowsUpdated = updateFirstName.ExecuteNonQuery();
            student.SetFirstName(newName);

            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }
            EditStudentMenu(school, student);
        }

        private void EditLastNameStudent(School school, Student student)
        {
            Console.WriteLine(student);
            Console.WriteLine("\nGive me the new Last Name : ");
            var newName = InputConstraint.StringConstraint(Console.ReadLine());
            SqlCommand updateLasttName = new SqlCommand($"UPDATE Student SET LastName = '{newName}' WHERE ID = '{student.ID}'", sqlConnection);

            int rowsUpdated = updateLasttName.ExecuteNonQuery();
            student.SetLastName(newName);

            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }
            EditStudentMenu(school, student);

        }

        private void EditDateOfBirth(School school, Student student)
        {
            Console.WriteLine(student);
            Console.WriteLine("\nGive me the new date OF birth : ");
            var dateBirth = InputConstraint.DateTimeConstraint(Console.ReadLine());
            SqlCommand updateDateBirth = new SqlCommand($"UPDATE Student SET DareBirth = '{dateBirth}' WHERE ID = '{student.ID}'", sqlConnection);

            int rowsUpdated = updateDateBirth.ExecuteNonQuery();
            student.SetDateBirth(dateBirth);

            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }
            EditStudentMenu(school, student);

        }

        private void EditStudentUserName(School school, Student student)
        {
            Console.Clear();
            Console.WriteLine(student);

            Console.WriteLine("Give me the new userName : ");
            var userName = InputConstraint.UsernameCheck(Console.ReadLine(), school);
   
            SqlCommand updateUserName = new SqlCommand($"UPDATE UserDetails SET UserName = '{userName}' WHERE ID = '{student.User.ID}'", sqlConnection);

            int rowsUpdated = updateUserName.ExecuteNonQuery();
            student.User.SetUserName(userName);

            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }
            EditStudentMenu(school, student);

        }

        private void EditStudentPassword(School school, Student student)
        {
            Console.Clear();
            Console.WriteLine(student);

            Console.WriteLine("Give me the new password : ");
            var passWord = InputConstraint.PasswordCheck(Console.ReadLine(), school);

            SqlCommand updatePassWord = new SqlCommand($"UPDATE UserDetails SET PassWord = '{passWord}' WHERE ID = '{student.User.ID}'", sqlConnection);

            int rowsUpdated = updatePassWord.ExecuteNonQuery();
            student.User.SetPassword(passWord);

            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }
            EditStudentMenu(school, student);

        }

        private void DeleteStudent(School school)
        {
            var minID = school.Students.Min(c => c.ID);
            var maxID = school.Students.Max(c => c.ID);
            Console.WriteLine("/nChoose which Student to delete by its ID\n");
            school.Students.ForEach(c => Console.WriteLine(c));
            Console.WriteLine("Choose the student you want to delete by its ID and press the corresponded number : ");
            var id = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minID, maxID);

            Student student = school.Students.SingleOrDefault(s => s.ID == id);

            SqlCommand deleteStudentPerCourse = new SqlCommand($"DELETE FROM StudentPerCourse WHERE StudentID = '{student.ID}'", sqlConnection);
            int rowsdel = deleteStudentPerCourse.ExecuteNonQuery();

            SqlCommand deleteMarks = new SqlCommand($"DELETE FROM Mark WHERE StudentID = '{student.ID}'", sqlConnection);
            deleteMarks.ExecuteNonQuery();

            SqlCommand deleteStudent = new SqlCommand($"DELETE FROM Student WHERE ID = '{student.ID}'", sqlConnection);
            int rowsUpdated = deleteStudent.ExecuteNonQuery();
            if (rowsUpdated > 0)
            {
                Console.WriteLine("Deleted Succesfully");
            }

            SqlCommand deleteUser = new SqlCommand($"DELETE FROM UserDetails WHERE ID = '{student.User.ID}'", sqlConnection);
            int rowsDel = deleteUser.ExecuteNonQuery();

            school.Students.Remove(student);
            school.Users.Remove(school.Users.SingleOrDefault(u => u.ID == student.User.ID));
            school.StudentsPerCourses.RemoveAll(spc => spc.Student.ID == student.ID);

            CrudStudent(school);
        }

        //         ---------------------------------------TRAINER-----------------------------------------------------

        private void CrudTrainer(School school)
        {
            Console.Clear();
            Console.WriteLine("\nChoose the corrensponded number\n");


            Console.WriteLine("1 : CREATE\n2 : VIEW\n3 : UPDATE\n4 : DELETE\n5 : GO TO PREVIOUS MENU");

            var crudOption = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 5);
            switch (crudOption)
            {
                case 1:
                    CreateTrainer(school);
                    break;
                case 2:
                    VieTrainers(school);
                    break;
                case 3:
                    ChooseTrainerForEdit(school);
                    break;
                case 4:
                    DeleteTrainer(school);
                    break;
                case 5:
                    GetHeadMasterMainMenu(school);
                    break;
            }
        }

        private void CreateTrainer(School school)
        {
            Console.Clear();
            Console.WriteLine("First name :");
            var firstName = InputConstraint.StringConstraint(Console.ReadLine());
            Console.WriteLine("Last name : ");
            var lastName = InputConstraint.StringConstraint(Console.ReadLine());
            Console.WriteLine("Subject that teaches : ");
            var subject = InputConstraint.StringConstraint(Console.ReadLine());
            Console.WriteLine("Give me a username for the online platform : ");
            var userName = InputConstraint.UsernameCheck(Console.ReadLine(), school);
            Console.WriteLine("Give me a passWord for the oline platform :");
            var passWord = InputConstraint.PasswordCheck(Console.ReadLine(), school);
            User user = new User(UserType.Trainer, userName, passWord);

            Trainer trainer = new Trainer(firstName, lastName, subject);

            SqlCommand cmdInsertUser = new SqlCommand($"INSERT INTO UserDetails(UserTypeID,UserName,PassWord) VALUES('{2}','{userName}','{passWord}')", sqlConnection);
            int rows = cmdInsertUser.ExecuteNonQuery();

            SqlCommand cmdSelectUserID = new SqlCommand("SELECT MAX(UserDetails.ID) FROM UserDetails", sqlConnection);
            SqlDataReader selectReaderUserID = cmdSelectUserID.ExecuteReader();
            while (selectReaderUserID.Read())
            {
                user.SetID(selectReaderUserID.GetInt32(0));
            }
            selectReaderUserID.Close();

            //Student student = new Student(firstName, lastName, dateOfBirth);
            SqlCommand cmdInsert = new SqlCommand($"INSERT INTO Trainer(FirstName,LastName,Subject,UserID) VALUES('{firstName}','{lastName}','{subject}',{user.ID})", sqlConnection);
            int insertedRows = cmdInsert.ExecuteNonQuery();
            //an isxuei to apo katw, tote exv potelsma
            if (insertedRows > 0)
            {
                Console.WriteLine("Insertion Succesful");
            }

            
            SqlCommand cmdSelect = new SqlCommand("SELECT MAX(Trainer.ID) FROM Trainer", sqlConnection);
            SqlDataReader selectReader = cmdSelect.ExecuteReader();
            while (selectReader.Read())
            {
                trainer.SetID(selectReader.GetInt32(0));
            }
            selectReader.Close();

         
            trainer.SetUser(user);
            school.AddUser(user);
            school.Trainers.Add(trainer);

            CrudTrainer(school);
        }

        private void VieTrainers(School school)
        {
            Console.Clear();
            Console.WriteLine("Trainers : \n");
            school.Trainers.ForEach(c => Console.WriteLine(c));
            Console.WriteLine("\nPress enter to go to previous menu");
            while (!String.IsNullOrEmpty(Console.ReadLine()))
            {
                var inpout = Console.ReadLine();
            }
            CrudTrainer(school);
        }

        private void ChooseTrainerForEdit(School school)
        {
            Console.Clear();

            //SQL TO CHOOSE THE MIN AND MAX ID
            var minID = school.Trainers.Min(s => s.ID);
            var maxID = school.Trainers.Max(s => s.ID);


            Console.WriteLine("/nChoose which trainer to update by its ID\n");
            school.Trainers.ForEach(c => Console.WriteLine(c));
            Console.WriteLine("\nChoose the trainer you want to edit by its ID and press the corresponded number : ");
            var id = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minID, maxID);

            Trainer trainer = school.Trainers.SingleOrDefault(t => t.ID == id);

            Console.Clear();
            EditTrainerMenu(school, trainer);
        }

        private void EditTrainerMenu(School school, Trainer trainer)
        {
            Console.Clear();
            Console.WriteLine("\nChoose what you want to update(Press the corresponded number)\n\n1 : FIRST NAME\n2 : LAST NAME\n" +
                "3 : SUBJECT\n4 : TRAINER USERNAME\n5 : TRAINER PASSWORD\n6 : GO TO PREVIOUS MENU ");
            var editOption = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 6);

            switch (editOption)
            {
                case 1:
                    EditFirstNameTrainer(school, trainer);
                    break;
                case 2:
                    EditLastNameTrainer(school, trainer);
                    break;
                case 3:
                    EditSubject(school, trainer);
                    break;
                case 4:
                    EditTrainerUserName(school, trainer);
                    break;
                case 5:
                    EditTrainerPassword(school, trainer);
                    break;
                case 6:
                    CrudTrainer(school);
                    break;

            }
        }

        private void EditFirstNameTrainer(School school, Trainer trainer)
        {
            Console.WriteLine(trainer);
            Console.WriteLine("\nGive me the new first Name : ");
            var newName = InputConstraint.StringConstraint(Console.ReadLine());
            SqlCommand updateFirstName = new SqlCommand($"UPDATE Trainer SET FirstName = '{newName}' WHERE ID = '{trainer.ID}'", sqlConnection);

            int rowsUpdated = updateFirstName.ExecuteNonQuery();
            trainer.SetFirstName(newName);

            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }

            EditTrainerMenu(school, trainer);
        }

        private void EditLastNameTrainer(School school, Trainer trainer)
        {
            Console.WriteLine(trainer);
            Console.WriteLine("\nGive me the new Last Name : ");
            var newName = InputConstraint.StringConstraint(Console.ReadLine());
            SqlCommand updateLasttName = new SqlCommand($"UPDATE Trainer SET LastName = '{newName}' WHERE ID = '{trainer.ID}'", sqlConnection);

            int rowsUpdated = updateLasttName.ExecuteNonQuery();
            trainer.SetLastName(newName);

            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }
            EditTrainerMenu(school, trainer);

        }

        private void EditSubject(School school, Trainer trainer)
        {
            Console.WriteLine(trainer);
            Console.WriteLine("\nGive me the new subject : ");
            var newSubject = InputConstraint.StringConstraint(Console.ReadLine());
            SqlCommand updateSubject = new SqlCommand($"UPDATE Trainer SET Subject = '{newSubject}' WHERE ID = '{trainer.ID}'", sqlConnection);

            int rowsUpdated = updateSubject.ExecuteNonQuery();
            trainer.SetSubject(newSubject);

            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }
            EditTrainerMenu(school, trainer);

        }

        private void EditTrainerUserName(School school, Trainer trainer)
        {
            Console.Clear();
            Console.WriteLine(trainer);

            Console.WriteLine("Give me the new userName : ");
            var userName = InputConstraint.UsernameCheck(Console.ReadLine(), school);
            SqlCommand updateUserName = new SqlCommand($"UPDATE UserDetails SET UserName = '{userName}' WHERE ID = '{trainer.User.ID}'", sqlConnection);

            int rowsUpdated = updateUserName.ExecuteNonQuery();
            trainer.User.SetUserName(userName);

            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }
            EditTrainerMenu(school, trainer);

        }

        private void EditTrainerPassword(School school, Trainer trainer)
        {
            Console.Clear();
            Console.WriteLine(trainer);

            Console.WriteLine("Give me the new password : ");
            var passWord = InputConstraint.PasswordCheck(Console.ReadLine(), school);
            SqlCommand updatePassWord = new SqlCommand($"UPDATE UserDetails SET PassWord = '{passWord}' WHERE ID = '{trainer.User.ID}'", sqlConnection);

            int rowsUpdated = updatePassWord.ExecuteNonQuery();
            trainer.User.SetPassword(passWord);

            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }
            EditTrainerMenu(school, trainer);

        }

        private void DeleteTrainer(School school)
        {
            var minID = school.Trainers.Min(c => c.ID);
            var maxID = school.Trainers.Max(c => c.ID);
            Console.WriteLine("/nChoose which Trainer to delete by its ID\n");
            school.Trainers.ForEach(c => Console.WriteLine(c));
            Console.WriteLine("Choose the Trainer you want to delete by its ID and press the corresponded number : ");
            var id = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minID, maxID);

            Trainer trainer = school.Trainers.SingleOrDefault(s => s.ID == id);

            SqlCommand deleteTrainersPerCourse = new SqlCommand($"DELETE FROM TrainerPerCourse WHERE TrainerID = '{trainer.ID}'", sqlConnection);
            int rowsdel = deleteTrainersPerCourse.ExecuteNonQuery();


            SqlCommand deleteTrainer = new SqlCommand($"DELETE FROM Trainer WHERE ID = '{trainer.ID}'", sqlConnection);
            int rowsUpdated = deleteTrainer.ExecuteNonQuery();

            SqlCommand deleteUser = new SqlCommand($"DELETE FROM UserDetails WHERE ID = '{trainer.User.ID}'", sqlConnection);
            int rows3 = deleteUser.ExecuteNonQuery();


            school.Trainers.Remove(trainer);
            school.Users.Remove(school.Users.SingleOrDefault(u => u.ID == trainer.User.ID));
            school.TrainersPerCourses.RemoveAll(tpc => tpc.Trainer.ID == trainer.ID);

            CrudTrainer(school);
        }


        ///               ---------------------------------STUDENT PER COURSE------------------------------------

        private void CrudStudentPerCourse(School school)
        {
            Console.Clear();
            Console.WriteLine("\nChoose the corrensponded number\n");

            Console.WriteLine("1 : CREATE\n2 : VIEW\n3 : DELETE\n4 : GO TO PREVIOUS MENU");

            var crudOption = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 4);
            switch (crudOption)
            {
                case 1:
                    CreateStudentPerCourse(school);
                    break;
                case 2:
                    ViewStudentsPerCourses(school);
                    break;
                case 3:
                    DeleteStudentPerCourse(school);
                    break;
                case 4:
                    GetHeadMasterMainMenu(school);
                    break;
           
            }
        }

        private void ViewStudentsPerCourses(School school)
        {
            Console.Clear();
            Console.WriteLine("Students Per Courses : \n");
            school.StudentsPerCourses.ForEach(spc => Console.WriteLine(spc));
            Console.WriteLine("\nPress enter to go to previous menu");
            while (!String.IsNullOrEmpty(Console.ReadLine()))
            {
                var inpout = Console.ReadLine();
            }
            CrudStudentPerCourse(school);
        }

        private void CreateStudentPerCourse(School school)
        {
            Console.Clear();
            Console.WriteLine("STUDENTS");
            school.Students.ForEach(s => Console.WriteLine(s));
            Console.WriteLine("\n");
            Console.WriteLine("COURSES");
            school.Courses.ForEach(c => Console.WriteLine(c));
            Console.WriteLine("\n");
            Console.WriteLine("STUDENTS PER COURSES");
            school.StudentsPerCourses.ForEach(spc => Console.WriteLine(spc));


            var minStudentsID = school.Students.Min(s => s.ID);
            var maxStudentID = school.Students.Max(S => S.ID);

            var minCourseID = school.Courses.Min(c => c.ID);
            var maxCourseID = school.Courses.Max(c => c.ID);

            Console.WriteLine("Choose the student you want by typing the corresponding ID");
            var chosenStudentID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minStudentsID, maxStudentID);
            Console.WriteLine("Choose the course you want by typing the corresponding ID");
            var chosenCourseID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minCourseID, maxCourseID);

            Student student = school.Students.SingleOrDefault(s => s.ID == chosenStudentID);
            Course course = school.Courses.SingleOrDefault(c => c.ID == chosenCourseID);

            StudentPerCourse studentPerCourse = new StudentPerCourse(student, course);

            //ANOTHER LINQ WAY
            //if(school.StudentsPerCourses.Any(spc => spc.Student ==student && spc.Course == course))
          
            if (school.StudentsPerCourses.Exists(spc => spc.Student == student && spc.Course == course))
            {
                Console.WriteLine("There is already this enrollment.");
                Console.WriteLine("Press ENTER to go to previous menu");
                while (!String.IsNullOrEmpty(Console.ReadLine()))
                {
                    var inpout = Console.ReadLine();
                }
                CrudStudentPerCourse(school);

            }

            student.Courses.Add(studentPerCourse);
            course.Students.Add(studentPerCourse);

            school.StudentsPerCourses.Add(studentPerCourse);

            SqlCommand cmdInsert = new SqlCommand($"INSERT INTO StudentPerCourse(StudentID,CourseID) VALUES('{student.ID}','{course.ID}')", sqlConnection);

            int insertedRows = cmdInsert.ExecuteNonQuery();
            //an isxuei to apo katw, tote exv potelsma
            if (insertedRows > 0)
            {
                Console.WriteLine("Insertion Succesful");
            }

            CrudStudentPerCourse(school);

        }

        private void ChooseStudentPerCourseForEdit(School school)
        {

            Console.Clear();

            //SQL TO CHOOSE THE MIN AND MAX ID
            var minStudentsID = school.Students.Min(s => s.ID);
            var maxStudentID = school.Students.Max(S => S.ID);

            var minCourseID = school.Courses.Min(c => c.ID);
            var maxCourseID = school.Courses.Max(c => c.ID);


            Console.WriteLine("/nChoose which studentPerCourse to update by pressing the studentID first and then the courseID\n");
            school.StudentsPerCourses.ForEach(spc => Console.WriteLine(spc));
            var studentID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minStudentsID, maxStudentID);
            var courseID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minCourseID, maxCourseID);

            //Student student = school.Students.Single(s => s.ID == id);
            StudentPerCourse studentPerCourse = school.StudentsPerCourses.SingleOrDefault(spc => spc.Student.ID == studentID && spc.Course.ID == courseID);

            Console.Clear();
        }

     
        private void DeleteStudentPerCourse(School school)
        {

            Console.Clear();

            //SQL TO CHOOSE THE MIN AND MAX ID
            var minStudentsID = school.Students.Min(s => s.ID);
            var maxStudentID = school.Students.Max(S => S.ID);

            var minCourseID = school.Courses.Min(c => c.ID);
            var maxCourseID = school.Courses.Max(c => c.ID);


            Console.WriteLine("/nChoose which studentPerCourse to delete by pressing the studentID first and then the courseID\n");
            school.StudentsPerCourses.ForEach(spc => Console.WriteLine(spc));
            var studentID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minStudentsID, maxStudentID);
            var courseID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minCourseID, maxCourseID);
            Student student = school.Students.SingleOrDefault(s => s.ID == studentID);
            
            Course course = school.Courses.SingleOrDefault(c => c.ID == courseID);

            SqlCommand deleteSpc = new SqlCommand($"DELETE FROM StudentPerCourse WHERE StudentID = '{studentID}' AND CourseID = '{courseID}' ",sqlConnection);

            int rowsUpdated = deleteSpc.ExecuteNonQuery();
            if (rowsUpdated > 0)
            {
                Console.WriteLine("Deleted Succesfully");
            }

            school.StudentsPerCourses.Remove(school.StudentsPerCourses.Single(spc => spc.Student.ID == studentID && spc.Course.ID == courseID));
            student.Courses.Remove(student.Courses.SingleOrDefault(spc => spc.Student.ID == studentID && spc.Course.ID == courseID));

            CrudStudentPerCourse(school);

        }

        // ---------------------------------------TRAINER PER COURSE---------------------------------------------------------------------//


        private void CrudTrainerPerCourse(School school)
        {
            Console.Clear();
            Console.WriteLine("\nChoose the corrensponded number\n");

            Console.WriteLine("1 : CREATE\n2 : VIEW\n3 : DELETE\n4 : GO TO PREVIOUS MENU");

            var crudOption = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 5);
            switch (crudOption)
            {
                case 1:
                    CreateTrainerPerCourse(school);
                    break;
                case 2:
                    ViewTrainerPerCourses(school);
                    break;
                case 3:
                    DeleteTrainerPerCourse(school);
                    break;
                case 4:
                    GetHeadMasterMainMenu(school);
                    break;

            }
        }

        private void ViewTrainerPerCourses(School school)
        {
            Console.Clear();
            Console.WriteLine("Trainers Per Courses : \n");
            school.TrainersPerCourses.ForEach(spc => Console.WriteLine(spc));
            Console.WriteLine("\nPress enter to go to previous menu");
            while (!String.IsNullOrEmpty(Console.ReadLine()))
            {
                var inpout = Console.ReadLine();
            }
            CrudTrainerPerCourse(school);
        }

        private void CreateTrainerPerCourse(School school)
        {
            Console.Clear();
            Console.WriteLine("TRAINERS");
            school.Trainers.ForEach(s => Console.WriteLine(s));
            Console.WriteLine("\n");
            Console.WriteLine("COURSES");
            school.Courses.ForEach(c => Console.WriteLine(c));
            Console.WriteLine("\n");
            Console.WriteLine("TRAINERS PER COURSES");
            school.TrainersPerCourses.ForEach(tpc => Console.WriteLine(tpc));


            var minTrainerID = school.Trainers.Min(t => t.ID);
            var maxTrainerID = school.Trainers.Max(t => t.ID);

            var minCourseID = school.Courses.Min(c => c.ID);
            var maxCourseID = school.Courses.Max(c => c.ID);

            Console.WriteLine("Choose the trainer you want by typing the corresponding ID");
            var chosenTrainerID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minTrainerID, maxTrainerID);
            Console.WriteLine("Choose the course you want by typing the corresponding ID");
            var chosenCourseID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minCourseID, maxCourseID);

            Trainer trainer = school.Trainers.SingleOrDefault(t => t.ID == chosenTrainerID);
            Course course = school.Courses.SingleOrDefault(c => c.ID == chosenCourseID);

            TrainerPerCourse trainerPerCourse = new TrainerPerCourse(trainer, course);

            //ANOTHER LINQ WAY
            //if(school.StudentsPerCourses.Any(spc => spc.Student ==student && spc.Course == course))

            if (school.TrainersPerCourses.Exists(tpc => tpc.Trainer == trainer && tpc.Course == course))
            {
                Console.WriteLine("There is already this enrollment.");
                Console.WriteLine("Press ENTER to go to previous menu");
                while (!String.IsNullOrEmpty(Console.ReadLine()))
                {
                    var inpout = Console.ReadLine();
                }
                CrudTrainerPerCourse(school);

            }

            trainer.Courses.Add(trainerPerCourse);
            course.Trainers.Add(trainerPerCourse);

            school.TrainersPerCourses.Add(trainerPerCourse);

            SqlCommand cmdInsert = new SqlCommand($"INSERT INTO TrainerPerCourse(TrainerID,CourseID) VALUES('{trainer.ID}','{course.ID}')", sqlConnection);

            int insertedRows = cmdInsert.ExecuteNonQuery();
            //an isxuei to apo katw, tote exv potelsma
            if (insertedRows > 0)
            {
                Console.WriteLine("Insertion Succesful");
            }

            CrudTrainerPerCourse(school);

        }

        private void DeleteTrainerPerCourse(School school)
        {

            Console.Clear();

            //SQL TO CHOOSE THE MIN AND MAX ID
            var minTrainerID = school.Trainers.Min(t => t.ID);
            var maxTrainerID = school.Trainers.Max(t => t.ID);

            var minCourseID = school.Courses.Min(c => c.ID);
            var maxCourseID = school.Courses.Max(c => c.ID);


            Console.WriteLine("/nChoose which trainerPerCourse to delete by pressing the trainerID first and then the courseID\n");
            school.TrainersPerCourses.ForEach(tpc => Console.WriteLine(tpc));
            var trainerID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minTrainerID, maxTrainerID);
            var courseID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minCourseID, maxCourseID);
            Trainer trainer = school.Trainers.SingleOrDefault(t => t.ID == trainerID);

            Course course = school.Courses.SingleOrDefault(c => c.ID == courseID);

            SqlCommand deleteSpc = new SqlCommand($"DELETE FROM TrainerPerCourse WHERE TrainerD = '{trainerID}' AND CourseID = '{courseID}' ", sqlConnection);

            int rowsUpdated = deleteSpc.ExecuteNonQuery();
            if (rowsUpdated > 0)
            {
                Console.WriteLine("Deleted Succesfully");
            }

            school.TrainersPerCourses.Remove(school.TrainersPerCourses.Single(tpc => tpc.Trainer.ID == trainerID && tpc.Course.ID == courseID));
            trainer.Courses.Remove(trainer.Courses.SingleOrDefault(spc => spc.Trainer.ID == trainerID && spc.Course.ID == courseID));

            CrudTrainerPerCourse(school);

        }

        //                     ----------------------------------------------ASSIGNMENTS-------------------------------------------------------------------------------//

        private void CrudAssignment(School school)
        {
            Console.Clear();
            Console.WriteLine("\nChoose the corrensponded number\n");


            Console.WriteLine("1 : CREATE\n2 : VIEW\n3 : DELETE\n4 : GO TO PREVIOUS MENU");

            var crudOption = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 5);
            switch (crudOption)
            {
                case 1:
                    CreateAssignment(school);
                    break;
                case 2:
                    ViewAssignments(school);
                    break;
                case 3:
                    DeleteAssignment(school);
                    break;
                case 4:
                    GetHeadMasterMainMenu(school);
                    break;
            }
        }

        private void CreateAssignment(School school)
        {
            Console.WriteLine("Give me a title");
            Console.WriteLine("Select a course that you want to put this assignment by its ID");

            var minIDCourse = school.Courses.Min(c => c.ID);
            var maxIDCourse = school.Courses.Max(c => c.ID);

            school.Courses.ForEach(c => Console.WriteLine(c));
            var courseID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minIDCourse, maxIDCourse);
            Course course = school.Courses.SingleOrDefault(c => c.ID == courseID);

            Console.Clear();
            Console.WriteLine(course);
            Console.WriteLine("Give me a title for the assignment");
            var title = InputConstraint.StringConstraint(Console.ReadLine());
            var subDateAndTime = new DateTime();
            do
            {
                Console.WriteLine("Give me a submission date and time for the assignment that is between the course startdate and enddate(mm-dd-yyyy)");
                subDateAndTime = InputConstraint.DateTimeConstraint(Console.ReadLine());
            } while (subDateAndTime > course.EndDate || subDateAndTime < course.StartDate);

            Assignment assignment = new Assignment(title, subDateAndTime);
           
            SqlCommand cmdInsert = new SqlCommand($"INSERT INTO Assignment(CourseID,Title,SubmissionDateAndTime) VALUES('{course.ID}','{title}','{subDateAndTime}')", sqlConnection);
            int insertedRows = cmdInsert.ExecuteNonQuery();
            //an isxuei to apo katw, tote exv potelsma
            if (insertedRows > 0)
            {
                Console.WriteLine("Insertion Succesful");
            }

            SqlCommand cmdSelect = new SqlCommand("SELECT MAX(Assignment.ID) FROM Assignment", sqlConnection);
            SqlDataReader selectReader = cmdSelect.ExecuteReader();
            while (selectReader.Read())
            {
                assignment.SetAssignmentID(selectReader.GetInt32(0));
            }
            selectReader.Close();
            school.Assignments.Add(assignment);
            course.Assignments.Add(assignment);
            assignment.SetCourse(course);

            //DO I PUT IT AT STUDENTS???

            CrudAssignment(school);
        }

        private void ViewAssignments(School school)
        {
            Console.Clear();
            Console.WriteLine("Assignments : \n");
            school.Assignments.ForEach(a => Console.WriteLine(a));
            Console.WriteLine("Press enter to go to previous menu");
            while (!String.IsNullOrEmpty(Console.ReadLine()))
            {
                var inpout = Console.ReadLine();
            }
            CrudAssignment(school);
        }

        private void ChooseAssignmentForEdit(School school)
        {
            Console.Clear();
            //LINQ TO TAKE MIN AND MAX IDS
            var minID = school.Assignments.Min(a => a.ID);
            var maxID = school.Assignments.Max(a => a.ID);

            Console.WriteLine("/nChoose which assignment to update by its ID\n");
            school.Assignments.ForEach(a => Console.WriteLine(a));
            Console.WriteLine("\nChoose the assignment you want to edit by its ID and press the corresponded number : ");
            var id = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minID, maxID);

            Assignment assignment = school.Assignments.Single(a => a.ID == id);

            Console.Clear();
            EditAssignmentMenu(school, assignment);
        }

        private void EditAssignmentMenu(School school, Assignment assignment)
        {
            Console.Clear();
            Console.WriteLine("\nChoose what you want to update(Press the corresponded number)\n\n1 : TITLE\n2 : SUBMISSION DATE \n" +
                "3 : GO TO PREVIOUS MENU");
            var editOption = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 6);

            switch (editOption)
            {
                case 1:
                    EditTitleAssignment(school, assignment);
                    break;
                case 2:
                    EditAssignmentSubDate(school, assignment);
                    break;
                case 3:
                    CrudAssignment(school);
                    break;
              
            }
        }

        private void EditTitleAssignment(School school, Assignment assignment)
        {
            Console.WriteLine(assignment);
            Console.WriteLine("\nGive me the new title : ");
            var newTitle = InputConstraint.StringConstraint(Console.ReadLine());
            SqlCommand updateTitle = new SqlCommand($"UPDATE Assignment SET Title = '{newTitle}' WHERE ID = '{assignment.ID}'", sqlConnection);

            int rowsUpdated = updateTitle.ExecuteNonQuery();
            assignment.SetAssignmentTitle(newTitle);

            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }

            EditAssignmentMenu(school, assignment);

        }


        private void EditAssignmentSubDate(School school, Assignment assignment)
        {
            Console.WriteLine(assignment);
            var subDateAndTime = new DateTime();
            do
            {
                Console.WriteLine("Give me a submission new date  for the assignment that is between the course startdate and enddate that belongs(mm-dd-yyyy)");
                subDateAndTime = InputConstraint.DateTimeConstraint(Console.ReadLine());
            } while (subDateAndTime > assignment.Course.EndDate || subDateAndTime < assignment.Course.StartDate);

            SqlCommand updateSubDateAndTime = new SqlCommand($"UPDATE Assignment SET SubmissionDateAndTime = '{subDateAndTime}' WHERE ID = '{assignment.ID}'", sqlConnection);
            int rowsUpdated = updateSubDateAndTime.ExecuteNonQuery();

            assignment.SetSubDate(subDateAndTime);

            if (rowsUpdated > 0)
            {
                Console.WriteLine("Updated Succesfully");
                Console.WriteLine($"{rowsUpdated} rows updated succesfully");
            }

            EditAssignmentMenu(school, assignment);
        }

        
        private void DeleteAssignment(School school)
        {
            Console.Clear();

            var minID = school.Assignments.Min(a => a.ID);
            var maxID = school.Assignments.Max(a => a.ID);
            Console.WriteLine("/nChoose which assignment to delete by its ID\n");
            school.Assignments.ForEach(a => Console.WriteLine(a));
            Console.WriteLine("Choose the assignment you want to delete by its ID and press the corresponded number : ");
            var id = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minID, maxID);

            Assignment assignment = school.Assignments.SingleOrDefault(a => a.ID == id);

            SqlCommand deleteAssignment = new SqlCommand($"DELETE FROM Assignment WHERE ID = '{assignment.ID}'", sqlConnection);

            int rowsUpdated = deleteAssignment.ExecuteNonQuery();
          

            school.Assignments.Remove(assignment);
            var course = assignment.Course;
            course.Assignments.Remove(assignment);

            CrudCourse(school);
        }

        //    -------------------------------- MARKS ---------------------------------------------

        private void CrudMarks(School school)
        {
            Console.Clear();
            Console.WriteLine("\nChoose the corrensponded number\n");

            Console.WriteLine("1 : CREATE\n2 : VIEW\n3 : DELETE\n4 : GO TO PREVIOUS MENU");

            var crudOption = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 5);
            switch (crudOption)
            {
                case 1:
                    CreateTrainerPerCourse(school);
                    break;
                case 2:
                    ViewMarks(school);
                    break;
                case 3:
                    DeleteMarks(school);
                    break;
                case 4:
                    GetHeadMasterMainMenu(school);
                    break;

            }
        }

        private void ViewMarks(School school)
        {
            Console.Clear();
            Console.WriteLine("Trainers Per Courses : \n");
            school.TrainersPerCourses.ForEach(spc => Console.WriteLine(spc));
            Console.WriteLine("\nPress enter to go to previous menu");
            while (!String.IsNullOrEmpty(Console.ReadLine()))
            {
                var inpout = Console.ReadLine();
            }
            CrudTrainerPerCourse(school);
        }

        private void CreateMarks(School school)
        {
            Console.Clear();
            Console.WriteLine("MARKS");
            school.Students.ForEach(s => Console.WriteLine(s));
            Console.WriteLine("\n");
            Console.WriteLine("ASSIGNMENTS");
            school.Courses.ForEach(c => Console.WriteLine(c));
            Console.WriteLine("\n");
            Console.WriteLine("MARKS");
            school.TrainersPerCourses.ForEach(m => Console.WriteLine(m));


            var minStudentID = school.Students.Min(s => s.ID);
            var maxStudentID = school.Students.Max(s => s.ID);

            var minAssignmentID = school.Assignments.Min(c => c.ID);
            var maxAssignmentID = school.Assignments.Max(c => c.ID);

            Console.WriteLine("Choose the stundet you want by typing the corresponding ID");
            var chosenStundetID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minStudentID, maxStudentID);
            Console.WriteLine("Choose the assignment you want by typing the corresponding ID");
            var chosenAssignmentID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minAssignmentID, maxAssignmentID);

            Student student = school.Students.SingleOrDefault(s => s.ID == chosenStundetID);
            Assignment assignment = school.Assignments.SingleOrDefault(a => a.ID == chosenAssignmentID);

            var course = assignment.Course;

            Mark mark = new Mark(student, assignment,false);

            //ANOTHER LINQ WAY
            //if(school.StudentsPerCourses.Any(spc => spc.Student ==student && spc.Course == course))

            if (school.Marks.Exists(m => m.Student == student && m.Assignment == assignment))
            {
                Console.WriteLine("There is already this enrollment.");
                Console.WriteLine("Press ENTER to go to previous menu");
                while (!String.IsNullOrEmpty(Console.ReadLine()))
                {
                    var inpout = Console.ReadLine();
                }
                CrudAssignment(school);
            }

            //if (assignment.Course =! student.Courses.)

            student.Marks.Add(mark);
            assignment.Marks.Add(mark);

            school.Marks.Add(mark);

            SqlCommand cmdInsert = new SqlCommand($"INSERT INTO Mark(StudentID,AssignmentID,ISSubmitted) VALUES('{student.ID}','{assignment.ID}','{0}')", sqlConnection);

            int insertedRows = cmdInsert.ExecuteNonQuery();
            //an isxuei to apo katw, tote exv potelsma
            if (insertedRows > 0)
            {
                Console.WriteLine("Insertion Succesful");
            }

            CrudMarks(school);

        }

        private void DeleteMarks(School school)
        {

            Console.Clear();

            //SQL TO CHOOSE THE MIN AND MAX ID
            var minStudentID = school.Students.Min(t => t.ID);
            var maxStudentID = school.Students.Max(t => t.ID);

            var minAssignmentID = school.Assignments.Min(c => c.ID);
            var maxAssignmentID = school.Assignments.Max(c => c.ID);


            Console.WriteLine("/nChoose which mark to delete by pressing the studentID first and then the assignmentID\n");
            school.Marks.ForEach(m => Console.WriteLine(m));
            var studentID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minStudentID, maxStudentID);
            var assignmentID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minAssignmentID, maxAssignmentID);
            Student student = school.Students.SingleOrDefault(s => s.ID == studentID);

            Assignment assignment = school.Assignments.SingleOrDefault(a => a.ID == assignmentID);

            SqlCommand deleteSpc = new SqlCommand($"DELETE FROM Mark WHERE StudentID = '{student.ID}' AND AssignmentID = '{assignment.ID}' ", sqlConnection);

            int rowsUpdated = deleteSpc.ExecuteNonQuery();

            school.Marks.Remove(school.Marks.Single(m => m.Student.ID == studentID && m.Assignment.ID == assignmentID));
            student.Marks.Remove(student.Marks.SingleOrDefault(m => m.Assignment.ID == assignmentID && m.Student.ID == studentID));

            CrudTrainerPerCourse(school);

        }

        // -------------------------------------------------CRUD SCHEDULES-----------------------------------------------------------------------------//

        private void CrudSchedules(School school)
        {
            Console.Clear();
            Console.WriteLine("\nChoose the corrensponded number\n");

            Console.WriteLine("1 : CREATE\n2 : VIEW\n3 : UPDATE\n4 : DELETE\n5 : GO TO PREVIOUS MENU");

            var crudOption = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 5);
            switch (crudOption)
            {
                case 1:
                    CreateSchedulesPerCourse(school);
                    break;
                case 2:
                    ViewSchedulesPerCourses(school);
                    break;
                case 3:
                    UpdateSchedule(school);
                    break;
                case 4:
                    DeleteSchedule(school);
                    break;
                case 5:
                    GetHeadMasterMainMenu(school);
                    break;

            }
        }

     

        private void ViewSchedulesPerCourses(School school)
        {
            Console.Clear();
            Console.WriteLine("Schedules Per Courses : \n");
            school.Schedules.ForEach(s => Console.WriteLine(s));
            Console.WriteLine("\nPress enter to go to previous menu");
            while (!String.IsNullOrEmpty(Console.ReadLine()))
            {
                var inpout = Console.ReadLine();
            }
            CrudSchedules(school);
        }

        private void CreateSchedulesPerCourse(School school)
        {
            Console.Clear();
            Console.WriteLine("Courses");
            school.Courses.ForEach(c => Console.WriteLine(c));

            var minCourseID = school.Courses.Min(c => c.ID);
            var maxCourseID = school.Courses.Max(c => c.ID);

            Console.WriteLine("Choose the course you want by typing the corresponding ID");
            var chosenCourseID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minCourseID, maxCourseID);
            Console.Clear();
            Course course = school.Courses.SingleOrDefault(c => c.ID == chosenCourseID);
            Console.WriteLine(course);

            Console.WriteLine("Give me the day(1 for Monday,2 for Tuesday,3 for Wednesday,4 for Thursday,5 for Friday) :");
            var day = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 5);
            Console.WriteLine("Give me the description :");
            var description = Console.ReadLine();

            Schedule schedule = new Schedule(description, (DayOfWeek)day);

            SqlCommand insertSchedule = new SqlCommand($"INSERT INTO Schedule(Description,DayID,CourseID) VALUES ('{description}','{day}','{course.ID}')", sqlConnection);
            insertSchedule.ExecuteNonQuery();

            SqlCommand selectMaxID = new SqlCommand("SELECT MAX(Schedule.ID) FROM Schedule");
            SqlDataReader readID = selectMaxID.ExecuteReader();
            while (readID.Read())
            {
                schedule.SetID(readID.GetInt32(0));
            }
            readID.Close();

            school.Schedules.Add(schedule);
            course.Schedules.Add(schedule);
            CrudSchedules(school);
        }

        private void UpdateSchedule(School school)
        {
            Console.Clear();
            Console.WriteLine("Courses");
            school.Courses.ForEach(c => Console.WriteLine(c));

            var minCourseID = school.Courses.Min(c => c.ID);
            var maxCourseID = school.Courses.Max(c => c.ID);

            Console.WriteLine("Choose the course you want by typing the corresponding ID");
            var chosenCourseID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minCourseID, maxCourseID);
            Console.Clear();
            Course course = school.Courses.SingleOrDefault(c => c.ID == chosenCourseID);
            Console.WriteLine(course);

            Console.WriteLine("\nGive me the day(1 for Monday,2 for Tuesday,3 for Wednesday,4 for Thursday,5 for Friday) :");
            var day = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 5);
            Console.WriteLine("Give me the new description :");
            var description = Console.ReadLine();

            SqlCommand updateDescription = new SqlCommand($"UPDATE Schedule SET Description = '{description}' WHERE CourseID = '{course.ID}' AND DayID = '{day}'", sqlConnection);
            updateDescription.ExecuteNonQuery();

            var schedule = course.Schedules.Single(s => s.DayOfTheWeek == (DayOfWeek)day);
            schedule.SetDescription(description);
            CrudSchedules(school);

        }

        private void DeleteSchedule(School school)
        {

            Console.Clear();
            Console.WriteLine("Courses");
            school.Courses.ForEach(c => Console.WriteLine(c));

            var minCourseID = school.Courses.Min(c => c.ID);
            var maxCourseID = school.Courses.Max(c => c.ID);

            Console.WriteLine("Choose the course you want by typing the corresponding ID");
            var chosenCourseID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minCourseID, maxCourseID);
            Console.Clear();
            Course course = school.Courses.SingleOrDefault(c => c.ID == chosenCourseID);
            Console.WriteLine(course);

            Console.WriteLine("\nGive me the day(1 for Monday,2 for Tuesday,3 for Wednesday,4 for Thursday,5 for Friday) :");
            var day = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 5);

            SqlCommand deleteSchedule = new SqlCommand($"DELETE FROM Schedule WHERE CourseID = '{course.ID}' AND DayID = '{day}'", sqlConnection);
            deleteSchedule.ExecuteNonQuery();

            var schedule = course.Schedules.Single(s => s.DayOfTheWeek == (DayOfWeek)day);

            school.Schedules.Remove(schedule);
            course.Schedules.Remove(schedule);

            CrudSchedules(school);
        }

      

        //   ---------------------------------------------TRAINER MENU----------------------------------------------------------------------------//


        private void TrainerMainMenu(School school,User user)
        {
            Console.Clear();
            var trainer = school.Trainers.SingleOrDefault(t => t.User == user);
            Console.WriteLine($"Hello {trainer.FullName}!");
            Console.WriteLine("You have these options : \n1 : VIEW ALL THE COURSES YOU ARE ENROLLED" +
                "\n2 : VIEW ALL THE STUDENTS PER COURSE\n3 : VIEW ALL THE ASSIGNMENTS PER STUDENT PER COURSE" +
                "\n4 : MARK ALL THE ASSIGNMENTS PER STUDENT PER COURSE\n5 : LOG OUT");

            var answer = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 5);

            switch (answer)
            {
                case 1:
                    ViewAllTheCourses(school, trainer);
                    break;
                case 2:
                    ViewAllTheStudentsPerCourse(school, trainer);
                    break;
                case 3:
                    ViewAllTheAssignmentsPerCoursePerStudent(school, trainer);
                    break;
                case 4:
                    MarkAssignments(school, trainer);
                    break;
                case 5:
                    LogIn(school);
                    break;
            }
        }

        private void ViewAllTheCourses(School school, Trainer trainer)
        {
            Console.Clear();
            var listWithCourses = school.TrainersPerCourses.Where(tpc => tpc.Trainer.ID == trainer.ID).Select(spc => spc.Course).ToList();
            listWithCourses.ForEach(c => Console.WriteLine(c));

            
            Console.WriteLine("\nPress enter to go to previous menu");
            while (!String.IsNullOrEmpty(Console.ReadLine()))
            {
                var inpout = Console.ReadLine();
            }
            var user = trainer.User;
            TrainerMainMenu(school,user);
        }

        private void ViewAllTheStudentsPerCourse(School school,Trainer trainer)
        {
            Console.Clear();
            var listWithCourses = school.TrainersPerCourses.Where(tpc => tpc.Trainer.ID == trainer.ID).Select(spc => spc.Course).ToList();

            //listWithCourses.Select(c => c.Students).ToList().ForEach(spc => Console.WriteLine($"Course : {spc.cou}"););

            var courses = school.StudentsPerCourses.Where(spc => listWithCourses.Contains(spc.Course)).ToList();

            courses.ForEach(spc => Console.WriteLine($" Course : {spc.Course} \nStudent : {spc.Student}"));

            Console.WriteLine("\nPress enter to go to previous menu");
            while (!String.IsNullOrEmpty(Console.ReadLine()))
            {
                var inpout = Console.ReadLine();
            }
            var user = trainer.User;
            TrainerMainMenu(school, user);

        }

        private void ViewAllTheAssignmentsPerCoursePerStudent(School school,Trainer trainer)
        {
            Console.Clear();

            var listWithCourses = school.TrainersPerCourses.Where(tpc => tpc.Trainer.ID == trainer.ID).Select(spc => spc.Course).ToList();

            var assignments = school.Assignments.Where(a => listWithCourses.Contains(a.Course)).ToList();

            var assignmentsPerStudent = school.Marks.Where(m => assignments.Contains(m.Assignment)).ToList();

            assignmentsPerStudent.ForEach(m => Console.WriteLine(m));
            Console.WriteLine("\nPress enter to go to previous menu");

            while (!String.IsNullOrEmpty(Console.ReadLine()))
            {
                var inpout = Console.ReadLine();
            }
            var user = trainer.User;
            TrainerMainMenu(school, user);
           
        }

        private void MarkAssignments(School school, Trainer trainer)
        {
            Console.Clear();


            var listWithCourses = school.TrainersPerCourses.Where(tpc => tpc.Trainer.ID == trainer.ID).Select(spc => spc.Course).ToList();

            var assignments = school.Assignments.Where(a => listWithCourses.Contains(a.Course)).ToList();

            var marks = school.Marks.Where(m => assignments.Contains(m.Assignment)).ToList();
            marks.ForEach(m => Console.WriteLine(m));

            var minStudentID = marks.Min(m => m.Student.ID);
            var maxStudentID = marks.Max(m => m.Student.ID);

            var minAssignmentID = marks.Min(m => m.Assignment.ID);
            var maxAssignment = marks.Max(m => m.Assignment.ID);

            Console.WriteLine("Choose the student first by its ID and the assignment then by its ID to mark it");
            var studentID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minStudentID, maxStudentID);
            var assignmentID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minStudentID, maxStudentID);

            var mark = school.Marks.SingleOrDefault(m => m.Student.ID == studentID && m.Assignment.ID == assignmentID);

            if (mark.IsSubmitted)
            {
                Console.WriteLine("Put the oral mark(0 - 100)");
                var oralMark = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 0, 100);
                Console.WriteLine("Put the total mark(0 -100)");
                var totalMark = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 0, 100);

                SqlCommand inserTOralMark = new SqlCommand($"UPDATE Mark SET OralMark = '{oralMark}' WHERE StudentID ='{studentID}' AND AssignmentID = '{assignmentID}'", sqlConnection);
                inserTOralMark.ExecuteNonQuery();
                SqlCommand inserTotalMark = new SqlCommand($"UPDATE Mark SET TotalMark = '{totalMark}' WHERE StudentID ='{studentID}' AND AssignmentID = '{assignmentID}'", sqlConnection);
                inserTotalMark.ExecuteNonQuery();

                mark.SetOralMark(oralMark);
                mark.SetTotalMark(totalMark);

            }
            else
            {
                Console.WriteLine("This mark has not yet submitted");
            }
            var user = trainer.User;
            TrainerMainMenu(school, user);
        }

        // --------------------------------------------------------------STUDENT MENU----------------------------------------------------------------//

        private void StudentMainMenu(School school,User user)
        {
           
                Console.Clear();
            var student = school.Students.SingleOrDefault(s=> s.User == user);
            Console.WriteLine($"Hello {student.FullName}!");
            Console.WriteLine("You have these options : \n1 : VIEW YOUR SCHEDULE PER COURSE"+
                "\n2 : VIEW THE DATES OF THE SUBMISSION OF YOUR ASSIGNMENTS PER COURSE\n3 : SUBMIT ANY ASSIGNMENTS" +
                "\n4 : LOG OUT");

            var answer = InputConstraint.IntChoiseForMenu(Console.ReadLine(), 1, 4);

            switch (answer)
            {
                case 1:
                    ViewSchedulePerCourse(school, student);
                    break;
                case 2:
                    ViewSubmissionDates(school, student);
                    break;
                case 3:
                    SubmitAnyAssignments(school, student);
                    break;
                case 4:
                    LogIn(school);
                    break;
            }
        }

        private void ViewSchedulePerCourse(School school,Student student)
        {
            Console.Clear();
            var courses = school.StudentsPerCourses.Where(spc => spc.Student == student).Select(spc => spc.Course).ToList();

            var schedules = school.Schedules.Where(s => courses.Contains(s.Course)).ToList();

            var dateTime = DateTime.Now;
            var todayDay = dateTime.DayOfWeek;

            var schedulesOfTheday = schedules.Where(s => s.DayOfTheWeek == todayDay).ToList();

            schedulesOfTheday.ForEach(s => Console.WriteLine(s + "" + dateTime));
            Console.WriteLine("\nPress enter to go to previous menu");

            while (!String.IsNullOrEmpty(Console.ReadLine()))
            {
                var inpout = Console.ReadLine();
            }
            var user = student.User;
            StudentMainMenu(school, user);
        }

        private void ViewSubmissionDates(School school,Student student)
        {
            Console.Clear();

            var listWithCourses = school.StudentsPerCourses.Where(spc => spc.Student.ID == student.ID).Select(spc => spc.Course).ToList();

            var assignments = school.Assignments.Where(a => listWithCourses.Contains(a.Course)).ToList();

            var assignmentsPerStudent = school.Marks.Where(m => assignments.Contains(m.Assignment)).ToList();

            assignmentsPerStudent.ForEach(aps => Console.WriteLine(aps.Assignment.Course + "\n"+ aps.Assignment + "\n" +aps.Assignment.SubmissionDateAndTime));

            while (!String.IsNullOrEmpty(Console.ReadLine()))
            {
                var inpout = Console.ReadLine();
            }
            var user = student.User;
            StudentMainMenu(school, user);
        }

        private void SubmitAnyAssignments(School school,Student student)
        {
            Console.Clear();

            var listWithCourses = school.StudentsPerCourses.Where(spc => spc.Student.ID == student.ID).Select(spc => spc.Course).ToList();

            var assignments = school.Assignments.Where(a => listWithCourses.Contains(a.Course)).ToList();

            var assignmentsPerStudent = school.Marks.Where(m => assignments.Contains(m.Assignment)).ToList();

            var assignmentsNotSubmitted = assignmentsPerStudent.Where(m => !m.IsSubmitted).ToList();

            var minAssignmentID = assignmentsNotSubmitted.Min(aps => aps.Assignment.ID);
            var maxAssignmentID = assignmentsNotSubmitted.Max(aps => aps.Assignment.ID);

            Console.WriteLine("Here are the assignments that you have not yet still submitted ");
            assignmentsNotSubmitted.ForEach(m => Console.WriteLine(m.Assignment));
            Console.WriteLine("\nChoose the assignment you want to submit by its id");
            var assignmentID = InputConstraint.IDChoiseForEditOrDelete(Console.ReadLine(), minAssignmentID, maxAssignmentID);

            SqlCommand submitAss = new SqlCommand($"UPDATE Mark SET ISSubmitted = '{1}' WHERE AssignmentID = '{assignmentID}' AND StudentID = '{student.ID}'", sqlConnection);
            submitAss.ExecuteNonQuery();

            var mark =  student.Marks.SingleOrDefault(m => m.Assignment.ID == assignmentID);
            mark.SetIsSubmitted(true);

            while (!String.IsNullOrEmpty(Console.ReadLine()))
            {
                var inpout = Console.ReadLine();
            }
            var user = student.User;
            StudentMainMenu(school, user);
        }

    }
}
