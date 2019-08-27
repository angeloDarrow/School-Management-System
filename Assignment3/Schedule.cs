using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    class Schedule
    {
        public int ID { get; private set; }
        public Course Course { get; private set; }
        public string Description { get; private set; }
        public DayOfWeek DayOfTheWeek { get; private set; }
        

        public Schedule(string description,DayOfWeek dayOfWeek)
        {
            Description = description;
            DayOfTheWeek = dayOfWeek;
        }

        public override string ToString()
        {
            StringBuilder sb= new StringBuilder();

            sb.
                AppendLine($"Schedule ID : {ID}")
                .AppendLine($"CourseDetails : {Course}")
                .AppendLine($"Description : {Description}")
                .AppendLine($"Day : {DayOfTheWeek}");

            return sb.ToString();

        }

        public void SetID(int id)
        {
            ID = id;
        }

        public void SetCourse(Course course)
        {
            Course = course;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetDayofWeek(DayOfWeek day)
        {
            DayOfTheWeek = day;
        }
    }
}
