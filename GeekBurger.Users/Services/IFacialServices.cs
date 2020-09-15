using GeekBurger.Users.Contract;
using GeekBurger.Users.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.Users.Services
{
    public interface IFacialServices
    {
        Task<bool> UpsertFaceListAndCheckIfContainsFaceAsync();
    }
}
