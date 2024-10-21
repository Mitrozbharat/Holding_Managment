using DocumentFormat.OpenXml.Office2010.Excel;

namespace Hoarding_managment.Interface
{
    public interface IOngoingCampain
    {
        public Task<List<CampaignViewModel>> GetallOngoingCampaignAsync(int pageNumber, int pageSize);
        public Task<CampaigneditViewModel> UpdateCampaignAsync(CampaigneditViewModel model);
        public Task<TblCampaingitem> GetCampaingnByIdAsync(int id);
        public Task<int> DeleteCampaignAsync(int id);
        public Task<int> GetOngoingCampaignCountAsync();
      
        public  Task<QuotationItemListViewModel> addCampaign(QuotationItemListViewModel selectedItems);


        public Task<List<CampaignViewModel>> GetallOngoingCampaignAsync(string searchQuery,int pageNumber,int pageSize);
        public Task<List<CampaignViewModel>> GetallCompletedCampaignAsync(string searchQuery,int pageNumber,int pageSize);
        public Task<int>  GetOngoingCampaignCountAsync(string searchQuery);
        public Task<int> GetCompletedCampaignCountAsync(string searchQuery);
        public Task<CampaignViewModel> SearchByCampaignNameAsync(string name);
         public Task<List<CampaignViewModel>> GetCampaignAsync(string searchQuery, int pageNumber, int pageSize);
         public Task<List<CampaignViewModel>> CompletedOngoingcampaignAsync(string searchQuery, int pageNumber, int pageSize);
         public Task<TblCampaingitem>  GetCampaingnItemByIdAsync(int id);


        public  Task<TblCampaingitem> IsCampaignBooked(int id, DateTime requestedFromDate, DateTime requestedToDate, int status);


    }
}
