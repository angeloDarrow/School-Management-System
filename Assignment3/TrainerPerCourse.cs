using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    class TrainerPerCourse
    {
        //NO ID BECAUSE I CHOOSE IDS FROM TRAINER AND STUDENT TO CRUD ON THE OBJECT
        public Trainer Trainer { get; }
        public Course Course { get;  }

        public TrainerPerCourse(Trainer trainer,Course course)
        {
            Trainer = trainer;
            Course = course;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.
            AppendLine($"Trainer is {Trainer}\n")
            .AppendLine($" Course is {Course}\n");

            return sb.ToString();
        }
    }
}
