using GeekBurger.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Repository
{
    public interface IUsersRepository
    {
        UserModel GetUserById(int idUser);

        UserModel GetUserByFace(string faceUser);

        List<UserModel> GetUsers();

        bool Add(UserModel userModel);

        bool Update(UserModel userModel);

        void Delete(UserModel userModel);

        void Save();
    }
}
