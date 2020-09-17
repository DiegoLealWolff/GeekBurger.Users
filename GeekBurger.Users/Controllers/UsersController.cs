using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeekBurger.Users.Contract;
using GeekBurger.Users.Services;
using GeekBurger.Users.Model;
using GeekBurger.Users.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace GeekBurger.Users.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersRepository _usersRepository;
        private IConfiguration _configuration;

        public UsersController(IUsersRepository usersRepository, IConfiguration configuration)
        {
            _usersRepository = usersRepository;
            _configuration = configuration;
        }

        [HttpPost("user")]
        public async Task<IActionResult> PostValidadeUser(string faceInput)
        {            
            var apiFacial = new FacialServices(_usersRepository);
            var resultModel = apiFacial.StartValidade(faceInput);
                        
            var serviceBusService = new ServiceBusService(_configuration);
            var result = serviceBusService.CreateTopic();            

            var userRetrieved = new UserRetrieved()
            {
                AreRestrictionsSet = true,
                UserId = resultModel.UserId
            };

            result = await serviceBusService.SendMessagesAsync(userRetrieved);

            if (result == false)
            {
                return NotFound();
            }

            return StatusCode(200);
        }

        [HttpPost("foodRestriction")]
        public async void PostPublishFoodRestriction(FoodRestriction foodRestriction)
        {      
            var userRetrieved = new UserRetrieved()
            {
                AreRestrictionsSet = true,
                UserId = foodRestriction.UserId
            };
      
            var serviceBusService = new ServiceBusService(_configuration);
            serviceBusService.CreateTopic();
            await serviceBusService.SendMessagesAsync(userRetrieved);
        }
    }
}
