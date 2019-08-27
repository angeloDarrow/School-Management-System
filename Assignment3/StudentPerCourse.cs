using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    class StudentPerCourse
    {
        //NO ID BECAUSE I CHOOSE IDS FROM TRAINER AND STUDENT TO CRUD ON THE OBJECT
        public Student Student { get; set; }
        public Course Course { get; set; }

        public StudentPerCourse(Student student,Course course)
        {
            Student = student;
            Course = course;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.
            AppendLine($"Student is : {Student}\n")
            .AppendLine($" Course is : {Course}\n");

            return sb.ToString();
        }
    }
}
