namespace Hoarding_managment.Interface
{
    public interface IOngoingCampain
    {
        public Task<List<CampaignViewModel>> GetallOngoingCampaignAsync(int pageNumber, int pageSize);
        public Task<CampaigneditViewModel> UpdateCampaignAsync(CampaigneditViewModel model);
        public Task<TblCampaign> GetCampaingnByIdAsync(int id);
        public Task<int> DeleteCampaignAsync(int id);
        public Task<int> GetOngoingCampaignCountAsync();
        public  Task<QuotationItemListViewModel> addCampaign(QuotationItemListViewModel selectedItems);


        public Task<List<CampaignViewModel>> GetallOngoingCampaignAsync(string searchQuery,int pageNumber,int pageSize);
        public Task<int>  GetOngoingCampaignCountAsync(string searchQuery);

        public Task<CampaignViewModel> SearchByCampaignNameAsync(string name); 
    }
}
