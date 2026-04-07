namespace MVCAdvertisment.Models
{
    public class AdsDetails
    {
        public AdsDetails(int adId, string adTitle, string adDescription, decimal adPrice)
        {
            AdId = adId;
            AdTitle = adTitle;
            AdDescription = adDescription;
            AdPrice = adPrice;
        }

        public AdsDetails() { }
        
        public int AdId { get; set; }
        public string AdTitle { get; set; }
        public string AdDescription { get; set; }
        public decimal AdPrice { get; set; }
    }
}