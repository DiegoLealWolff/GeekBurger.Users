using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeekBurger.Users.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;

namespace GeekBurger.Users.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IList<User> Users = new List<User>()
        {
            new User { Face = "XXXX", UserId = 1 },
            new User { Face = "YYYY", UserId = 2 },
            new User { Face = "YYYY", UserId = 3 }
        };

        [HttpPost("user")]
        public IActionResult PostValidadeUser(string face)
        {
            //AQUI PRECISO ACESSAR A API DE RECONHECIMENTO FACIAL   
            var result = Users.Where(x => x.Face == face).ToList();

            if (result.Count <= 0)
            {
                return NotFound();
            }

            return StatusCode(200);
        }

        [HttpPost("foodRestriction")]
        public async Task<IActionResult> PostPublishFoodRestriction(FoodRestriction foodRestriction)
        {
            string QueueConnectionString = "Endpoint=sb://geekburguerdiegowolffservicebus.servicebus.windows.net/;SharedAccessKeyName=ProductPolicy;SharedAccessKey=hyPvBASPC5yAtXSI/UFMNLDWETBJD46jXFrXbhNU880=";
            string QueuePath = "foodRestriction";

            IQueueClient queueClient = new QueueClient(QueueConnectionString, QueuePath);
            queueClient.OperationTimeout = TimeSpan.FromSeconds(10);

            var userRetrieved = new UserRetrieved()
            {
                AreRestrictionsSet = foodRestriction.Restrictions,
                UserId = foodRestriction.UserId
            };

            var messages = new List<Message>();
            messages.Add(new Message(Encoding.UTF8.GetBytes(userRetrieved.AreRestrictionsSet)));
            messages.Add(new Message(Encoding.UTF8.GetBytes(userRetrieved.UserId.ToString())));

            var sendTask = queueClient.SendAsync(messages);

            await sendTask;

            return StatusCode(200);
        }
    }
}
