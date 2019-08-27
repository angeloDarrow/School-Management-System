using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
     class InputConstraint //HELPFUL METHODS FOR REPEATED CONSTRAINTS
    {
        //STRING CONSTRAINT
        public static string StringConstraint(string input)
        {
            while (String.IsNullOrEmpty(input) || input.Any(c => Char.IsSeparator(c)))
            {               
                Console.WriteLine("Wrong input, try again");
                input = Console.ReadLine();          
            }
            return input;
        }
        //INT CONSTRAINT
        public static int IntConstraint(string input)
        {
            int outpout;
            
            while(!int.TryParse(input,out outpout))
            {
                Console.WriteLine("Wrong Input, try again");
                input = Console.ReadLine();
            }
            return outpout;
        }

        //FOR ENUMS STREAM AND TIMESCHEDULE
        public static int BitConstraint(string inpout)
        {
            int outpout;

            while(!int.TryParse(inpout,out outpout) || int.Parse(inpout) < 0 || int.Parse(inpout) > 1)
            {
                Console.WriteLine("Wrong input, try again");
                inpout = Console.ReadLine();
            }

            return outpout;      
        }

        //FOR DATETIME
        public static DateTime DateTimeConstraint(string inpout)
        {
            DateTime outpout;

            while(!DateTime.TryParse(inpout,out outpout))
            {
                Console.WriteLine("Wrong inpout, try again");
                inpout = Console.ReadLine();
            }

            return outpout;
        }

        //ID CHOISE FOR EDIT OR DELETE
        public static int IDChoiseForEditOrDelete(string inpout, int minID, int maxID)
        {
            int outpout;

            while (!int.TryParse(inpout, out outpout) || int.Parse(inpout) < minID || int.Parse(inpout) > maxID)
            {
                Console.WriteLine("Wrong input, try again");
                inpout = Console.ReadLine();
            }

            return outpout;
        }

        //MENU INT CHOISE
        public static int IntChoiseForMenu(string inpout, int minOption, int maxOption)
        {
            int outpout;
            while (!int.TryParse(inpout, out outpout) || int.Parse(inpout) < minOption || int.Parse(inpout) > maxOption)
            {
                Console.WriteLine("Wrong input, try again");
                inpout = Console.ReadLine();
            }

            return outpout;
        }

        //CONSTRAINT FOR THE USERNAME IF ALREADY EXISTS
        public static string UsernameCheck(string userName,School school)
        {
            userName = SHA.GenerateSHA512String(userName);

            while (String.IsNullOrEmpty(userName) || school.Users.Exists(u => u.UserName == userName ))
            {
                Console.WriteLine("This username already exists,try another");
                userName = SHA.GenerateSHA512String(Console.ReadLine());
            }

            return userName;
        }

        //CONSTRAINT FOR THE PASSWORD IF ALREADY EXISTS
        public static string PasswordCheck(string passWord, School school)
        {
            passWord = SHA.GenerateSHA512String(passWord);
            while (String.IsNullOrEmpty(passWord) || school.Users.Exists(u => u.PassWord == passWord))
            {              
                Console.WriteLine("This password already exists,try another");
                passWord = SHA.GenerateSHA512String(Console.ReadLine());
            }

            return passWord;
        }
    }
}
