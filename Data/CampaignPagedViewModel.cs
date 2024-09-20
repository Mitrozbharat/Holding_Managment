namespace Hoarding_managment.Data
{
    public class CampaignPagedViewModel
    {
        public List<CampaignViewModel> ?Campaigns { get; set; }
        public int? CurrentPage { get; set; }
        public int ? TotalPages { get; set; }
    }
}
