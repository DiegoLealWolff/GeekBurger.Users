using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GeekBurger.Users.Contract
{
    public class UserRetrieved
    {
        [Key]
        public int UserId { get; set; }
        
        public bool AreRestrictionsSet { get; set; }        
    }
}
