using Microsoft.AspNetCore.Mvc;
using APISubscriber.Models;

namespace APISubscriber.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriberController : ControllerBase
    {

        [HttpGet("{subNumber}")]
        public IActionResult SelectSubscriberBySubNumber(string subNumber)
        {
            SubscriberMethods subscriberMethods = new SubscriberMethods();
            SubscriberDetails subscriber = subscriberMethods.GetSubscriberBySubNumber(subNumber);

            return Ok(subscriber);
        }

        [HttpPut("{subNumber}")]
        public IActionResult UpdateSubscriber(string subNumber,[FromBody] SubscriberDetails editSubscriber)
        {
            editSubscriber.SubscriptionNumber = subNumber;
            SubscriberMethods subscriberMethods = new SubscriberMethods();
            SubscriberDetails updatedSubscriber = subscriberMethods.EditSubscriberBySubDetails(editSubscriber);

            return Ok(updatedSubscriber);
        }
    }
}