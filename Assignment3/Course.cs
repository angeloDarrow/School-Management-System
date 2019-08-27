using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    enum Stream
    {
        Java, Csharp
    }
    enum TimeSchedule
    {
        Full,Part
    }

    class Course
    {
        public int ID { get; private set; }
        public Stream Stream { get; private set; }
        public TimeSchedule TimeSchedule { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string Title { get; private set; }
        public List<StudentPerCourse> Students { get; }
        public List<TrainerPerCourse> Trainers { get; }
        public List<Assignment> Assignments { get; }
        public List<Schedule> Schedules { get; }

        public Course( Stream stream,TimeSchedule timeSchedule, DateTime startDate,DateTime endDate,string title)
        {
            Stream = stream;           
            TimeSchedule = timeSchedule;
            StartDate = startDate;
            EndDate = endDate;
            Title = title;
            Students = new List<StudentPerCourse>();
            Trainers = new List<TrainerPerCourse>();
            Assignments = new List<Assignment>();
            Schedules = new List<Schedule>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb
                .AppendLine($"Course ID : {ID} ")
                .AppendLine($"Stream : {Stream} ")
                .AppendLine($"TimeSchedule : {TimeSchedule} ")
                .AppendLine($"StartDate : {StartDate.ToShortDateString()} ")
                .AppendLine($"EndDate : {EndDate.ToShortDateString()} ")
                .AppendLine($"Title : {Title}");

            return sb.ToString();
        }

        public void SetID(int id)
        {
            ID = id;
        }

        public void SetStream(Stream stream)
        {
            Stream = stream;
        }

        public void SetTimeSchedule(TimeSchedule timeSchedule)
        {
            TimeSchedule = timeSchedule;
        }

        public void SetStartDate(DateTime startDate)
        {
            StartDate = startDate;
        }

        public void SetEndDate(DateTime endDate)
        {
            EndDate = endDate;
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

    }
}
