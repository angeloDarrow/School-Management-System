using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    class Assignment
    {
        public int ID { get; private set; }
        public string Title { get; private set; }
        public DateTime SubmissionDateAndTime { get; private set; }
        public List<Mark> Marks { get;  }
        public Course Course { get; private set; }
        

        public Assignment(string title, DateTime submissionDateAndTime)
        {
            Title = title;
            SubmissionDateAndTime = submissionDateAndTime;
            Marks = new List<Mark>();           
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb
            .AppendLine($"Assignment ID : {ID} ")

            .AppendLine($"Title : {Title} ")
            .AppendLine($"SUbmission Date :{SubmissionDateAndTime.ToShortDateString()}")
            .AppendLine($"Course that belongs : {Course} ");

            return  sb.ToString();
        }

        public void SetAssignmentID(int id)
        {
            ID = id;
        }

        public void SetCourse(Course course)
        {
            Course = course;
        }

        public void SetAssignmentTitle(string title)
        {
            Title = title;
        }

       public void SetSubDate(DateTime dateTime)
        {
            SubmissionDateAndTime = dateTime;
        }
    }
}
