namespace MVCAdvertisment.Models
{
    public class AdsDetails
    {
        public AdsDetails(int adId, string adTitle, string adDescription, int adItemPrice, int adPrice)
        {
            AdId = adId;
            AdTitle = adTitle;
            AdDescription = adDescription;
            AdItemPrice = adItemPrice;
            AdPrice = adPrice;
        }

        public AdsDetails() { }
        
        public int AdId { get; set; }
        public string AdTitle { get; set; }
        public string AdDescription { get; set; }
        public int AdItemPrice { get; set; }
        public int AdPrice { get; set; }
    }
}