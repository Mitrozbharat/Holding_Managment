namespace Hoarding_managment.Data
{
    public class CampaignViewModel
    {
        public int Id { get; set; }
        public int? FkInventoryId { get; set; }
        public int? FkCustomerId { get; set; }
        public string? BookingAmt { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public ulong? IsDelete { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public string CustomerName { get; set; }

        public string? VendorName { get; set; }
        public string? BusinessName { get; set; }
        public string? City { get; set; }
        public int? totalhording { get; set; }
    }
}
