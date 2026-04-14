namespace MVCAdvertisment.Models
{
    public class CreateAdvertismentViewModel
    {
        public SubscriberDetails? Subscriber { get; set; }
        public AdsDetails Ad { get; set; }
        public AnnonsorerDetails? Annonsorer { get; set; }

        public bool ShowSubscriberForm { get; set; }
        public bool ShowCompanyForm { get; set; }
        public bool ShowAdForm { get; set; }

    }
}