namespace MVCAdvertisment.Models
{
    public class AnnonsorerDetails
    {
        public AnnonsorerDetails(int advId, string advName, int advNumber, int advPhoneNumber, string advDeliveryAddress, string advPostalCode, string advCity, string advBillingAddress)
        {
            AdvId = advId;
            AdvName = advName;
            AdvNumber = advNumber;
            AdvPhoneNumber = advPhoneNumber;
            AdvDeliveryAddress = advDeliveryAddress;
            AdvPostalCode = advPostalCode;
            AdvCity = advCity;
            AdvBillingAddress = advBillingAddress;
        }
    
        public AnnonsorerDetails() { }
        
        public int AdvId { get; set; }
        public string AdvName { get; set; }  
        public int AdvNumber { get; set; }
        public int AdvPhoneNumber { get; set; }
        public string AdvDeliveryAddress { get; set; }
        public string AdvPostalCode { get; set; }
        public string AdvCity { get; set; }
        public string AdvBillingAddress { get; set; }
        
    }
}