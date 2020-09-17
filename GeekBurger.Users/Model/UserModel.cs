using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Model
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        public string Face { get; set; }               

        public string Restrictions { get; set; }

        public string Others { get; set; }

        public bool AreRestrictionsSet { get; set; }
    }
}
