using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.Users.Contract
{
    public class FoodRestriction
    {
        public string Restrictions { get; set; }
        public string Others { get; set; }
        public int UserId { get; set; }
        public int RequesterId { get; set; }
    }
}
