using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    class Trainer : Human
    {
        public int ID { get; private set; }
        public string Subject { get; private set; }
        public int PhoneNumber { get; }
        public List<TrainerPerCourse> Courses { get;  }
        public User User { get; private set; }

        public Trainer(string firstName,string lastName,string subject) :base(firstName,lastName)
        {
            Subject = subject;        
            Courses = new List<TrainerPerCourse>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb
            .AppendLine($" Trainer ID : {ID} ")
            .AppendLine($"Subject is {Subject} ")
            .AppendLine($"UserDetails : {User}");

            return base.ToString() + sb.ToString();
        }

        public void SetID(int id)
        {
            ID = id;
        }

        public void SetUser(User user)
        {
            User = user;
        }

        public void SetSubject(string subject)
        {
            Subject = subject;
        }


    }
}
