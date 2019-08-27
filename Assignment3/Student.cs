using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    class Student : Human
    {
        public int ID { get; private set; }
        public DateTime DateBirth { get; private set; }
        public int PhoneNumber { get;  }
        public List<StudentPerCourse> Courses{ get;  }
        public List<Mark> Marks { get;  }
        public User User { get; private set; }

        public Student(string firstName, string lastName, DateTime dateBirth) : base( firstName,  lastName )
        {
            DateBirth = dateBirth;
            Courses = new List<StudentPerCourse>();
            Marks = new List<Mark>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb
                       .AppendLine($" Student ID : {ID} ")
                       .Append($"DateBirth : {DateBirth.ToShortDateString()} ")
                       .AppendLine($"UserDetails :   {User} ");

            return base.ToString() + sb.ToString();
        }

        public void SetID(int id)
        {
            ID = id;
        }

        public void SetDateBirth(DateTime dateBirth)
        {
            DateBirth = dateBirth;
        }

        public void SetUser(User user)
        {
            User = user;
        }
     

    }
}
