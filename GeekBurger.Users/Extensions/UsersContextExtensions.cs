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
            context.Users.RemoveRange(context.Users);

            context.Users.AddRange(
                new List<UserModel> {
                    new UserModel { UserId = 111,
                                    Face = "8048e9ec-80fe-4bad-bc2a-e4f4a75c834e",
                                    AreRestrictionsSet = false                                     
                    },
                    new UserModel { UserId = 222,
                                    Face = "9872ddda-80fe-4bad-bc2a-e4f4a75c834e",
                                    AreRestrictionsSet = true,
                                    Restrictions = "ovo, leite"
                    },
                    new UserModel { UserId = 333,
                                    Face = "556546-80fe-4bad-bc2a-e4f4asec834e",
                                    AreRestrictionsSet = true,
                                    Restrictions = "Aipim"
                    },
                    new UserModel { UserId = 444,
                                    Face = "142ffwe7jfr-80fe-4bad-bc2a-e4f3dfgc834e",
                                    AreRestrictionsSet = true,
                                    Restrictions = "amendoim"
                    },
                    new UserModel { UserId = 555,
                                    Face = "53ff234234-80fe-4bad-bc2a-e4fgrddf44e",
                                    AreRestrictionsSet = false
                    }

        });

            context.SaveChanges();            
        }
    }
}
