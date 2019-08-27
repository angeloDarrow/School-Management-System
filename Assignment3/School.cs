using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    class School
    {
       // private List<Course> courses;
        public List<Course> Courses { get; }    
        public List<Student> Students { get;}
        public List<Trainer> Trainers { get;  }
        public List<User> Users { get;  }
        public List<Assignment> Assignments { get;  }
        public List<StudentPerCourse> StudentsPerCourses { get; }
        public List<TrainerPerCourse> TrainersPerCourses { get; }
        public List<Mark> Marks { get; }
        public List<Schedule> Schedules { get; }

        public School()
        {
            Courses = new List<Course>();
            Users = new List<User>();
            Students = new List<Student>();
            Trainers = new List<Trainer>();
            Assignments = new List<Assignment>();
            StudentsPerCourses = new List<StudentPerCourse>();
            TrainersPerCourses = new List<TrainerPerCourse>();
            Marks = new List<Mark>();
            Schedules = new List<Schedule>();
        }

        public void AddCourse(Course course)
        {
            Courses.Add(course);
        }

        public void AddStudent(Student student)
        {
            Students.Add(student);
        }

        public void AddTrainer(Trainer trainer)
        {
            Trainers.Add(trainer);
        }

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        public void AddAssignment(Assignment assignment)
        {
            Assignments.Add(assignment);
        }

        public void AddStudentPerCourse(StudentPerCourse studentPerCourse)
        {
            StudentsPerCourses.Add(studentPerCourse);
        }

        public void AddTrainerPerCOurse(TrainerPerCourse trainerPerCourse)
        {
            TrainersPerCourses.Add(trainerPerCourse);
        }

        public void AddMark(Mark mark)
        {
            Marks.Add(mark);
        }
    }

}
