using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.Users.Contract
{
    public class UserRetrieved
    {
        public string AreRestrictionsSet { get; set; }

        public int UserId { get; set; }
    }
}
