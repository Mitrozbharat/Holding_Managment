using static Hoarding_managment.Controllers.QuatationController;

namespace Hoarding_managment.Data
{
    public class QuotationData
    {
        public string QuotationNumber { get; set; }
        public string QuotationDate { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string TotalAmount { get; set; }

        public int fk_invertyid { get; set; }



        public List<QuotationItem> Items { get; set; }
    }
    public class QuotationItem
    {
        public string Area { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string Size { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Price { get; set; }
        public string type { get; set; }
    }
}
