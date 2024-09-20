
namespace Hoarding_managment.Repository
{
    public class OngoingCampainRepository : IOngoingCampain
    {
        private readonly db_hoarding_managementContext _context;

        public OngoingCampainRepository( db_hoarding_managementContext db_Hoarding_Management)
        {
            _context = db_Hoarding_Management;  
        }

        public async Task<List<CampaignViewModel>> GetallOngoingCampaignAsync(int pageNumber, int pageSize)
        {
            var campaigns = await _context.TblCampaigns.Where(x=>x.IsDelete==0)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(item => new CampaignViewModel
                {
                    Id = item.Id,
                    ToDate = item.ToDate,
                    FromDate = item.FromDate,
                    BookingAmt = item.BookingAmt,
                    UpdatedBy = item.UpdatedBy,
                    CreatedAt = item.CreatedAt,
                    IsDelete = item.IsDelete,
                    CustomerName = _context.TblCustomers
                        .Where(c => c.Id == item.FkCustomerId)
                        .Select(c => c.CustomerName)
                        .FirstOrDefault(),
                    City = _context.TblInventories
                        .Where(v => v.Id == item.FkInventoryId)
                        .Select(v => v.City)
                        .FirstOrDefault(),
                    BusinessName = _context.TblVendors
                        .Where(v => v.Id == item.FkInventoryId)
                        .Select(v => v.BusinessName)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return campaigns;
        }
     
        public async Task<CampaignViewModel> UpdateCampaignAsync(CampaignViewModel model)
        {

            var campaign = new TblCampaign
            {
                Id=model.Id,
                //FkCustomer=model.FkCustomerId,
                //FkInventory=model.FkInventoryId,
                FromDate=model.FromDate,
                ToDate=model.ToDate,
                BookingAmt=model.BookingAmt,

            };

            _context.TblCampaigns.Update(campaign);
            await _context.SaveChangesAsync();

            return model;
        }
        public async Task<TblCampaign> GetCampaingnByIdAsync(int id)
        {
            return await _context.TblCampaigns.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == 0);
        }
        public async Task<int> DeleteCampaignAsync(int id)
        {
            var campaign = await _context.TblCampaigns
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == 0);

            if (campaign == null)
            {
                return 0;
            }
            // Mark the campaign as deleted
            campaign.IsDelete = 1;

            // Update the campaign in the database
            _context.TblCampaigns.Update(campaign);
            await _context.SaveChangesAsync();

            // Return 1 (indicating a successful deletion)
            return 1;
        }
        public async Task<int> GetOngoingCampaignCountAsync()
        {
            return await _context.TblCampaigns
                                 .CountAsync(c => c.FromDate >= DateTime.Today && c.IsDelete == 0);
        }



        public async Task<QuotationItemListViewModel> addCampaign(QuotationItemListViewModel selectedItems)
        {
            if (selectedItems.CustomerId != null && selectedItems.SelectedItems != null)
            {
                foreach (var item in selectedItems.SelectedItems)
                {
                    var newdata = new TblCampaign
                    {
                        FkCustomerId = selectedItems.CustomerId,
                        FkInventoryId = item.FkInventoryId,
                        FromDate = item.FromDate,
                        ToDate = item.ToDate,
                        BookingAmt = item.Rate,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDelete = 0,
                        UpdatedBy = "admin",
                        CreatedBy = "Admin"
                    };

                    _context.TblCampaigns.Add(newdata);
                    var recordsAffected = await _context.SaveChangesAsync();

                    if (recordsAffected > 0)
                    {
                        // Remove associated inventory items if the quotation item was saved successfully.
                        var itemsToDelete = _context.TblInventoryitems
                            .Where(x => x.FkInventoryId == item.FkInventoryId);
                        _context.TblInventoryitems.RemoveRange(itemsToDelete);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return null;
        }

       
    }
}
