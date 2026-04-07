using Microsoft.AspNetCore.Mvc;
using APISubscriber.Models;

namespace APISubscriber.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriberController : ControllerBase
    {
        [HttpGet]
        public IActionResult SelectAllSubscribers()
        {
            SubscriberMethods subscriberMethods = new SubscriberMethods();
            List<SubscriberDetails> subscribers = subscriberMethods.GetAllSubscribers();

            return Ok(subscribers);
        }

        [HttpGet("{subNumber}")]
        public IActionResult SelectSubscriberBySubNumber(int subNumber)
        {
            SubscriberMethods subscriberMethods = new SubscriberMethods();
            SubscriberDetails subscriber = subscriberMethods.GetSubscriberBySubNumber(subNumber);

            return Ok(subscriber);
        }
    }
}