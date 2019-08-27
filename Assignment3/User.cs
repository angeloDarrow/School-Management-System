using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    public enum UserType
    {
        HeadMaster,Trainer,Student
    }
 
    class User
    {
        public int ID { get; private set; }
        public UserType Type { get; private set; }
        public string UserName { get; private set; }
        public string PassWord { get; private set; }
        
        public User( UserType type,string userName, string passWord)
        {
            Type = type;
            UserName = userName;
            PassWord = passWord;        
        }

        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb

                .AppendLine($"User ID is {ID} ")
                .AppendLine($"User Type is {Type} ");
                //.AppendLine($"UserName is {UserName} ")
                //.AppendLine($"Password is {PassWord}");
            return sb.ToString();          
        }

        public void SetID(int id)
        {
            ID = id;
        }

        public void SetUserType(UserType userType)
        {
            Type = userType;
        }

        public void SetUserName(string userName)
        {
            UserName = userName;
        }

        public void SetPassword(string passWord)
        {
            PassWord = passWord;
        }




    }
}
