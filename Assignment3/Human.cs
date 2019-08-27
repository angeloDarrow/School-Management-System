using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    abstract class Human
    {
        //private string firstName;
        public string FirstName { get; private set; }
        //  private string lastName;
        public string LastName { get; private set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }


        public Human(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.
                AppendLine($"First Name : {FirstName} ")
                .AppendLine($"Last Name : {LastName} ");
                //.Append($"Full Name is {FullName}");

            return sb.ToString();

        }

        public void SetFirstName(string firstName)
        {
            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            LastName = lastName;
        }

    }
}
