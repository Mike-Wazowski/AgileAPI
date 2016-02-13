using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSST.Agile.Database.Models;

namespace TSST.Agile.Web.Helpers
{
    public class UserEqualityComparerById : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(User obj)
        {
            return obj.GetHashCode();
        }
    }
}