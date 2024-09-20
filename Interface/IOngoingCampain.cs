namespace Hoarding_managment.Interface
{
    public interface IOngoingCampain
    {
        public Task<List<CampaignViewModel>> GetallOngoingCampaignAsync(int pageNumber, int pageSize);
        public Task<CampaignViewModel> UpdateCampaignAsync(CampaignViewModel model);
        public Task<TblCampaign> GetCampaingnByIdAsync(int id);
        public Task<int> DeleteCampaignAsync(int id);
        public Task<int> GetOngoingCampaignCountAsync();

        public  Task<QuotationItemListViewModel> addCampaign(QuotationItemListViewModel selectedItems);
    }
}
