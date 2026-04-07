using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace APISubscriber.Models
{
    public class SubscriberDetails
    {
        //Konstruktor
        public SubscriberDetails(int subscriberId, string subsciptionNumber, string subscriberSocialSecutityNumber, string subscriberFirstName, string subscriberLastName, string subscriberDeliveryAddress, string subscriberPostalCode)
        {
            SubscriberId = subscriberId;
            SubsciptionNumber = subsciptionNumber;
            SubscriberSocialSecutityNumber = subscriberSocialSecutityNumber;
            SubscriberFirstName = subscriberFirstName;
            SubscriberLastName = subscriberLastName;
            SubscriberDeliveryAddress = subscriberDeliveryAddress;
            SubscriberPostalCode = subscriberPostalCode;
        }

        //Tom konstruktor
        public SubscriberDetails(){}

        public int SubscriberId { get; set; }
        public string SubsciptionNumber { get; set; }
        
        public string SubscriberSocialSecutityNumber { get; set; }
        public string SubscriberFirstName { get; set; }
        public string SubscriberLastName { get; set; }
        public string SubscriberDeliveryAddress { get; set; }
        public string SubscriberPostalCode { get; set; }
    }
}