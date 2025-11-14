namespace Hoarding_managment.Data
{
    public class PaymentQRViewModel
    {
        public int CampaignId { get; set; }
        public int itemid { get; set; }
        public string Customer { get; set; }
        public decimal Amount { get; set; }
        public string UpiId { get; set; } = "7420077075@ybl";
        public string QRString { get; internal set; }
    }

    public class PaymentDto
    {
        public string CampaignId { get; set; }
        public string itemid { get; set; }
        // public string VendorName { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; }
        public string MobileNumber { get; set; }
        public string UpiId { get; set; }
    }

    public class FinalPaymentDto
    {
        public int CampaignId { get; set; }
        public int IndexId { get; set; }
        public string Customer { get; set; }
        public decimal Amount { get; set; }
        public string UpiId { get; set; }
    }



    public class DateRangeRequest
    {
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Status { get; set; }
    }
}
