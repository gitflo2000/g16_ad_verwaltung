using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace g16_ad_verwaltung
{
    class UserParams
    {
        string coursename;
        string lastname;
        string firstname;
        string password;

        public UserParams(string course, string name, string prename, string pw)
        {
            coursename = course;
            lastname = name;
            firstname = prename;
            password = pw;
        }

        public string CourseName
        {
            get
            {
                return coursename;
            }
        }

        public string LastName
        {
            get
            {
                return lastname;
            }
        }

        public string FirstName
        {
            get
            {
                return firstname;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
        }

    }
}
