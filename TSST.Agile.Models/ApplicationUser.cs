using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSST.Agile.Models
{
    public class ApplicationUser
    {
        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private string pictureUrl;

        public string PictureUrl
        {
            get { return pictureUrl; }
            set { pictureUrl = value; }
        }
    }
}