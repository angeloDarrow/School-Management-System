using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    class Mark
    {
        public Student Student { get; private set; }
        public Assignment Assignment { get; private set; }
        public bool IsSubmitted { get; private set; }
        public int TotalMark { get; private set; }
        public int OralMark { get; private set; }

        public Mark(Student student,Assignment assignment,bool isSubmitted)
        {
            Student = student;
            Assignment = assignment;
            IsSubmitted = isSubmitted;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.
            AppendLine($"Student is {Student}\n")
            .AppendLine($" Assignment is {Assignment}\n")
            .AppendLine($"Is submitted is {IsSubmitted}")
            .AppendLine ($"TOTAL MARK IS {TotalMark}")
            .AppendLine($"ORAL MARK IS {OralMark}");

            return sb.ToString();
        }

        public void SetIsSubmitted(bool isSub)
        {
            IsSubmitted = isSub;
        }

        public void SetTotalMark(int totalMark)
        {
            TotalMark = totalMark;
        }

        public void SetOralMark(int oralMark)
        {
            OralMark = oralMark;
        }

        //public void SetStudent(Student student)
        //{

        //}

    }
}
