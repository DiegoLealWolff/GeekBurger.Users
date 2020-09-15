using GeekBurger.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private UsersDbContext _context;

        public UsersRepository(UsersDbContext context)
        {
            _context = context;
        }

        public bool Add(UserModel userModel)
        {
            _context.Users.Add(userModel);
            return true;
        }

        public void Delete(UserModel userModel)
        {
            _context.Users.Remove(userModel);      
        }

        public UserModel GetUserByFace(string faceUser)
        {
            var user = _context.Users?.Where(x => x.Face.Equals(faceUser, StringComparison.InvariantCultureIgnoreCase))
                       .FirstOrDefault();

            return user;
        }

        public UserModel GetUserById(int idUser)
        {
            var user = _context.Users?.Where(x => x.UserId == idUser).FirstOrDefault();

            return user;
        }

        public List<UserModel> GetUsers()
        {
            var user = _context.Users?.ToList();

            return user;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool Update(UserModel userModel)
        {
            _context.Users.Update(userModel);
            return true;
        }
    }
}
