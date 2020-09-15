using GeekBurger.Users.Model;
using GeekBurger.Users.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Extensions
{
    public static class UsersContextExtensions
    {
        public static void Seed(this UsersDbContext context)
        {
            //TODO REMOVER
            context.Users.RemoveRange(context.Users);

            context.Users.AddRange(
                new List<UserModel> {
                    new UserModel { UserId = 111,
                                    Face = "8048e9ec-80fe-4bad-bc2a-e4f4a75c834e",
                                    AreRestrictionsSet = "false"                                        
                    },
                    new UserModel { UserId = 222,
                                    Face = "9872ddda-80fe-4bad-bc2a-e4f4a75c834e",
                                    AreRestrictionsSet = "true",
                                    Restrictions = "ovo, leite"
                    }
        });

            context.SaveChanges();            
        }
    }
}
