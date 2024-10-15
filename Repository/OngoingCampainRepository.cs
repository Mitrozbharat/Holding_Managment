
using Hoarding_management.Data;

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

        public async Task<CampaigneditViewModel> UpdateCampaignAsync(CampaigneditViewModel model)  
        {
            try
            {
                var campaigndetails =await _context.TblCampaigns.FirstOrDefaultAsync(x => x.Id == model.Id);
                if(campaigndetails != null)
                {
                    campaigndetails.FromDate = model.FromDate;
                    campaigndetails.ToDate = model.ToDate;
                    campaigndetails.BookingAmt = model.BookingAmt;
                    _context.TblCampaigns.UpdateRange();
                  await  _context.SaveChangesAsync();

                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        

            return model;
        }

        public async Task<TblCampaingitem> GetCampaingnByIdAsync(int id)
        {
            return await _context.TblCampaingitems.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == 0);
        }
        public async Task<int> DeleteCampaignAsync(int id)
        {
            // Find the campaign that is not marked as deleted
            var campaignToDelete = await _context.TblCampaingitems
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == 0);

            // Return 0 if the campaign does not exist or is already marked as deleted
            if (campaignToDelete == null)
            {
                return 0;
            }

             campaignToDelete.IsDelete = 1;
            _context.TblCampaingitems.Update(campaignToDelete);
            await _context.SaveChangesAsync();

            return 1;
        }

        public async Task<int> GetOngoingCampaignCountAsync()
        {
            return await _context.TblCampaigns
                                 .CountAsync(c => c.FromDate >= DateTime.Today && c.IsDelete == 0);
        }
        //public async Task<QuotationItemListViewModel> addCampaign(QuotationItemListViewModel selectedItems)
        //{
        //    if (selectedItems.CustomerId != null && selectedItems.SelectedItems != null)
        //    {
        //        foreach (var item in selectedItems.SelectedItems)
        //        {
        //            var newdata = new TblCampaign
        //            {
        //                FkCustomerId = selectedItems.CustomerId,
        //                FkInventoryId = item.Id,
                        
        //                FromDate = item.FromDate,
        //                ToDate = item.ToDate,
        //                BookingAmt = item.Rate,
        //                CreatedAt = DateTime.Now,
        //                UpdatedAt = DateTime.Now,
        //                IsDelete = 0,
        //                UpdatedBy = "admin",
        //                CreatedBy = "Admin"
        //            };

        //            _context.TblCampaigns.Add(newdata);
        //            var recordsAffected = await _context.SaveChangesAsync();

        //            if (recordsAffected > 0)
        //            {
        //                // Remove associated inventory items if the quotation item was saved successfully.
        //                var itemsToDelete = _context.TblInventoryitems
        //                    .Where(x => x.FkInventoryId == item.FkInventoryId);
        //                _context.TblInventoryitems.RemoveRange(itemsToDelete);
        //                await _context.SaveChangesAsync();
        //            }
        //        }
        //    }

        //    return null;
        //}
        public async Task<List<CampaignViewModel>> GetallOngoingCampaignAsync(string searchQuery, int pageNumber, int pageSize)
        {
            // Retrieve all campaigns that are not deleted
            var campaigns = await _context.TblCampaigns
                .Where(x => x.IsDelete == 0 && x.ToDate >= DateTime.Today)
                .ToListAsync();

            // Filter by CustomerName, City, or BusinessName if a search query is provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                campaigns = campaigns.Where(x =>
                    _context.TblCustomers.Any(c => c.Id == x.FkCustomerId && c.CustomerName.Contains(searchQuery)) ||
                    _context.TblInventories.Any(v => v.Id == x.FkInventoryId && v.City.Contains(searchQuery)) ||
                    _context.TblVendors.Any(v => v.Id == x.FkInventoryId && v.BusinessName.Contains(searchQuery))
                ).ToList();
            }

            // Group campaigns by CustomerId and VendorId, selecting the last entry for each group
            var groupedCampaigns = campaigns
                .GroupBy(c => new { c.FkCustomerId, c.FkInventoryId })
                .Select(g => g.OrderBy(c => c.CreatedAt).FirstOrDefault())
                .ToList();

            // Apply pagination (Skip and Take)
            var pagedCampaigns = groupedCampaigns
                .OrderByDescending(x=>x.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Map the paged campaigns to the CampaignViewModel
            var result = pagedCampaigns.Select(item => new CampaignViewModel
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
                BusinessName = _context.TblVendors
                    .Where(v => v.Id == item.FkInventoryId)
                    .Select(v => v.BusinessName)
                    .FirstOrDefault(),
                VendorName = _context.TblVendors
                    .Where(v => v.Id == item.FkInventoryId)
                    .Select(v => v.VendorName)
                    .FirstOrDefault(),
                City = _context.TblInventories
                    .Where(v => v.Id == item.FkInventoryId)
                    .Select(v => v.City)
                    .FirstOrDefault(),
                Location = _context.TblInventories
                    .Where(v => v.Id == item.FkInventoryId)
                    .Select(v => v.Location)
                    .FirstOrDefault(),
                Image = _context.TblInventories
                    .Where(v => v.Id == item.FkInventoryId)
                    .Select(v => v.Image)
                    .FirstOrDefault(),
            }).ToList();

            return result;
        } 
        public async Task<List<CampaignViewModel>> GetallCompletedCampaignAsync(string searchQuery, int pageNumber, int pageSize)
        {
            // Retrieve all campaigns that are not deleted
            var campaigns = await _context.TblCampaigns
                .Where(x => x.IsDelete == 0 && x.ToDate < DateTime.Today)
                .ToListAsync();

            // Filter by CustomerName, City, or BusinessName if a search query is provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                campaigns = campaigns.Where(x =>
                    _context.TblCustomers.Any(c => c.Id == x.FkCustomerId && c.CustomerName.Contains(searchQuery)) ||
                    _context.TblInventories.Any(v => v.Id == x.FkInventoryId && v.City.Contains(searchQuery)) ||
                    _context.TblVendors.Any(v => v.Id == x.FkInventoryId && v.BusinessName.Contains(searchQuery))
                ).ToList();
            }

            // Group campaigns by CustomerId and VendorId, selecting the last entry for each group
            var groupedCampaigns = campaigns
                .GroupBy(c => new { c.FkCustomerId, c.FkInventoryId })
                .Select(g => g.OrderBy(c => c.CreatedAt).FirstOrDefault())
                .ToList();

            // Apply pagination (Skip and Take)
            var pagedCampaigns = groupedCampaigns
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Map the paged campaigns to the CampaignViewModel
            var result = pagedCampaigns.Select(item => new CampaignViewModel
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
                BusinessName = _context.TblVendors
                    .Where(v => v.Id == item.FkInventoryId)
                    .Select(v => v.BusinessName)
                    .FirstOrDefault(),
                VendorName = _context.TblVendors
                    .Where(v => v.Id == item.FkInventoryId)
                    .Select(v => v.VendorName)
                    .FirstOrDefault(),
                City = _context.TblInventories
                    .Where(v => v.Id == item.FkInventoryId)
                    .Select(v => v.City)
                    .FirstOrDefault(),
                Location = _context.TblInventories
                    .Where(v => v.Id == item.FkInventoryId)
                    .Select(v => v.Location)
                    .FirstOrDefault(),
                Image = _context.TblInventories
                    .Where(v => v.Id == item.FkInventoryId)
                    .Select(v => v.Image)
                    .FirstOrDefault(),
            }).ToList();

            return result;
        }

        public async Task<int> GetOngoingCampaignCountAsync(string searchQuery)
        {
            // Initialize the query for campaigns
            var query = _context.TblCampaigns.Where(x => x.IsDelete == 0 && x.ToDate >= DateTime.Today).AsQueryable();

            // If a search query is provided, filter by CustomerName, City, or BusinessName
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(x =>
                    _context.TblCustomers.Any(c => c.Id == x.FkCustomerId && c.CustomerName.Contains(searchQuery)) ||
                    _context.TblInventories.Any(v => v.Id == x.FkInventoryId && v.City.Contains(searchQuery)) ||
                    _context.TblVendors.Any(v => v.Id == x.FkInventoryId && v.BusinessName.Contains(searchQuery)));
            }

            // Return the count of campaigns that match the query
            return await query.CountAsync();
        } 
        
        public async Task<int> GetCompletedCampaignCountAsync(string searchQuery)
        {
            // Initialize the query for campaigns
            var query = _context.TblCampaigns.Where(x => x.IsDelete == 0 && x.ToDate < DateTime.Today).AsQueryable();

            // If a search query is provided, filter by CustomerName, City, or BusinessName
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(x =>
                    _context.TblCustomers.Any(c => c.Id == x.FkCustomerId && c.CustomerName.Contains(searchQuery)) ||
                    _context.TblInventories.Any(v => v.Id == x.FkInventoryId && v.City.Contains(searchQuery)) ||
                    _context.TblVendors.Any(v => v.Id == x.FkInventoryId && v.BusinessName.Contains(searchQuery)));
            }

            // Return the count of campaigns that match the query
            return await query.CountAsync();
        }

        public async Task<CampaignViewModel> SearchByCampaignNameAsync(string name)
        {
            var lowerCaseName = name.ToLower();

            // Find the first campaign that matches the name based on the customer, inventory, or vendor name
            var campaign = await _context.TblCampaigns
                .Where(x => x.IsDelete == 0 &&
                       (_context.TblCustomers.Any(c => c.Id == x.FkCustomerId && c.CustomerName.ToLower().Contains(lowerCaseName)) ||
                        _context.TblInventories.Any(v => v.Id == x.FkInventoryId && v.City.ToLower().Contains(lowerCaseName)) ||
                        _context.TblVendors.Any(v => v.Id == x.FkInventoryId && v.BusinessName.ToLower().Contains(lowerCaseName))))
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
                .FirstOrDefaultAsync();

            return campaign;
        }
        
        public async Task<TblCampaingitem> GetCampaingnItemByIdAsync(int id)
        {
            var existingCampaign = await _context.TblCampaingitems
                .Where(c => c.FkInventoryId == id )
                .FirstOrDefaultAsync();

            // Return the campaign if found, null otherwise
            return existingCampaign;
        }

        public async Task<List<CampaignViewModel>> GetCampaignAsync(string searchQuery, int pageNumber, int pageSize)
        {
            // Retrieve all campaigns that are not deleted and join with tbl_campaingitem
            var campaigns = await (from campaign in _context.TblCampaignnews
                                   join campaignItem in _context.TblCampaingitems
                                   on campaign.Id equals campaignItem.FkCampaignId
                                   where campaign.IsDelete == 0 && campaignItem.IsDelete == 0 && campaignItem.ToDate >= DateTime.Today
                                   select new { campaign, campaignItem })
                                   .ToListAsync();

            // Filter by CustomerName, City, or BusinessName if a search query is provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                campaigns = campaigns.Where(x =>
                    _context.TblCustomers.Any(c => c.Id == x.campaign.FkCustomerId && c.CustomerName.Contains(searchQuery)) ||
                    _context.TblInventories.Any(v => v.Id == x.campaignItem.FkInventoryId && v.City.Contains(searchQuery)) ||
                    _context.TblVendors.Any(v => v.Id == x.campaignItem.FkInventoryId && v.BusinessName.Contains(searchQuery))
                ).ToList();
            }

            // Apply pagination (Skip and Take)
            var pagedCampaigns = campaigns
                .OrderByDescending(x => x.campaign.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Map the paged campaigns to the CampaignViewModel
            var result = pagedCampaigns.Select(item => new CampaignViewModel
            {
                Id = item.campaign.Id,
                ToDate = item.campaignItem.ToDate,
                FromDate = item.campaignItem.FromDate,
                BookingAmt = item.campaignItem.BookingAmt,
                UpdatedBy = item.campaign.UpdatedBy,
                CreatedAt = item.campaign.CreatedAt,
                IsDelete = item.campaign.IsDelete,

                CustomerName = _context.TblCustomers
                    .Where(c => c.Id == item.campaign.FkCustomerId)
                    .Select(c => c.CustomerName)
                    .FirstOrDefault(),
                BusinessName = _context.TblVendors
                    .Where(v => v.Id == item.campaignItem.FkInventoryId)
                    .Select(v => v.BusinessName)
                    .FirstOrDefault(),
                VendorName = _context.TblVendors
                    .Where(v => v.Id == item.campaignItem.FkInventoryId)
                    .Select(v => v.VendorName)
                    .FirstOrDefault(),
                City = _context.TblInventories
                    .Where(v => v.Id == item.campaignItem.FkInventoryId)
                    .Select(v => v.City)
                    .FirstOrDefault(),
                Location = _context.TblInventories
                    .Where(v => v.Id == item.campaignItem.FkInventoryId)
                    .Select(v => v.Location)
                    .FirstOrDefault(),
                Image = _context.TblInventories
                    .Where(v => v.Id == item.campaignItem.FkInventoryId)
                    .Select(v => v.Image)
                    .FirstOrDefault(),
            }).ToList();

            return result;
        }


        public async Task<List<CampaignViewModel>> CompletedOngoingcampaignAsync(string searchQuery, int pageNumber, int pageSize)
        {
            // Join tbl_campaignnew and tbl_campaingitem to retrieve completed campaigns
            var campaigns = await (from campaign in _context.TblCampaignnews
                                   join campaignItem in _context.TblCampaingitems
                                   on campaign.Id equals campaignItem.FkCampaignId
                                   where campaign.IsDelete == 0 && campaignItem.ToDate < DateTime.Today
                                   select new { campaign, campaignItem })
                                   .ToListAsync();

            // Filter by CustomerName, City, or BusinessName if a search query is provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                campaigns = campaigns.Where(x =>
                    _context.TblCustomers.Any(c => c.Id == x.campaign.FkCustomerId && c.CustomerName.Contains(searchQuery)) ||
                    _context.TblInventories.Any(v => v.Id == x.campaignItem.FkInventoryId && v.City.Contains(searchQuery)) ||
                    _context.TblVendors.Any(v => v.Id == x.campaignItem.FkInventoryId && v.BusinessName.Contains(searchQuery))
                ).ToList();
            }

            // Group campaigns by CustomerId and InventoryId, selecting the last entry for each group
            var groupedCampaigns = campaigns
                .GroupBy(c => new { c.campaign.FkCustomerId, c.campaignItem.FkInventoryId })
                .Select(g => g.OrderBy(c => c.campaign.CreatedAt).FirstOrDefault())
                .ToList();

            // Apply pagination (Skip and Take)
            var pagedCampaigns = groupedCampaigns
                .OrderByDescending(x => x.campaign.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Map the paged campaigns to the CampaignViewModel
            var result = pagedCampaigns.Select(item => new CampaignViewModel
            {
                Id = item.campaign.Id,
                ToDate = item.campaignItem.ToDate,
                FromDate = item.campaignItem.FromDate,
                BookingAmt = item.campaignItem.BookingAmt,
                UpdatedBy = item.campaign.UpdatedBy,
                CreatedAt = item.campaign.CreatedAt,
                IsDelete = item.campaign.IsDelete,

                CustomerName = _context.TblCustomers
                    .Where(c => c.Id == item.campaign.FkCustomerId)
                    .Select(c => c.CustomerName)
                    .FirstOrDefault(),
                BusinessName = _context.TblVendors
                    .Where(v => v.Id == item.campaignItem.FkInventoryId)
                    .Select(v => v.BusinessName)
                    .FirstOrDefault(),
                VendorName = _context.TblVendors
                    .Where(v => v.Id == item.campaignItem.FkInventoryId)
                    .Select(v => v.VendorName)
                    .FirstOrDefault(),
                City = _context.TblInventories
                    .Where(v => v.Id == item.campaignItem.FkInventoryId)
                    .Select(v => v.City)
                    .FirstOrDefault(),
                Location = _context.TblInventories
                    .Where(v => v.Id == item.campaignItem.FkInventoryId)
                    .Select(v => v.Location)
                    .FirstOrDefault(),
                Image = _context.TblInventories
                    .Where(v => v.Id == item.campaignItem.FkInventoryId)
                    .Select(v => v.Image)
                    .FirstOrDefault(),
            }).ToList();

            return result;
        }


        public async Task<QuotationItemListViewModel> addCampaign(QuotationItemListViewModel selectedItems)
        {


            if (selectedItems.CustomerId != null && selectedItems.SelectedItems != null)
            {


                int currentYear = DateTime.Now.Year;
                int nextYear = DateTime.Now.Month >= 4 ? currentYear + 1 : currentYear;
                string financialYearCode = $"{currentYear % 100}{nextYear % 100}"; // For example: "2425"

                // Get the last quotation number
                var lastQuotation = await _context.TblCampaignnews
                    .Where(x => x.IsDelete == 0 && x.CampaignNumber.Contains(financialYearCode))
                    .OrderByDescending(x => x.CampaignNumber)
                    .FirstOrDefaultAsync();

                // Extract the sequence number
                string lastNumberPart = lastQuotation != null
                    ? lastQuotation.CampaignNumber.Substring(6) // Extracts the last three digits (sequence)
                    : "000";

                int nextNumber = int.Parse(lastNumberPart) + 1;

                // Generate the next quotation number
                string nextNumberString = $"C{financialYearCode}{nextNumber:D3}"; // Example: "Q2425001"


                // Create and add the new quotation
                var data = new TblCampaignnew
                {
                    CampaignNumber = nextNumberString,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "admin",
                    FkCustomerId = selectedItems.CustomerId,
                    IsDelete = 0
                };

                await _context.AddRangeAsync(data);
                await _context.SaveChangesAsync();

                var lastCampId = data.Id;


                foreach (var item in selectedItems.SelectedItems)
                {

                    var inventory = _context.TblInventories.Where(x => x.Id == item.Id).FirstOrDefault();
                    var newdata = new TblCampaingitem();
                    if (item.status == 0)
                    {
                        newdata = new TblCampaingitem
                        {
                            FkCampaignId = lastCampId,
                            FkInventoryId = item.FkInventoryId,
                            FromDate = item.FromDate,
                            ToDate = item.ToDate,
                            BookingAmt = item.Rate,
                            CreatedAt = DateTime.Now,
                            CreatedBy = "Admin",
                            IsDelete = 0,
                        };
                    }
                    else
                    {
                        newdata = new TblCampaingitem
                        {
                            FkCampaignId = lastCampId,
                            FkInventoryId = _context.TblQuotationitems.Where(x => x.Id == item.Id).Select(x => x.FkInventory).FirstOrDefault(),
                            FromDate = item.FromDate,
                            ToDate = item.ToDate,
                            BookingAmt = item.Rate,
                            CreatedAt = DateTime.Now,
                            CreatedBy = "Admin",
                            IsDelete = 0,
                        };
                    }

                    _context.TblCampaingitems.Add(newdata);
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


        public async Task<TblCampaingitem> IsCampaignBooked(int id, DateTime requestedFromDate, DateTime requestedToDate)
        {


            var FkId = await _context.TblInventoryitems
                .Where(x => x.Id == id) // Replace 'id' with your actual Id value
                .Select(x => x.FkInventoryId)
                .FirstOrDefaultAsync(); // Execute the query and return the first matching result or null


            var campaignItems = await _context.TblCampaingitems
                .Where(c => c.FkInventoryId == FkId && c.IsDelete == 0 && c.ToDate >= DateTime.Today) // Ensure it's not marked as deleted and ToDate is in the future
                .ToListAsync();

            // Check if the requested date range is outside all existing campaign date ranges
            foreach (var campaignItem in campaignItems)
            {
                // Check if the requested dates are outside the existing campaign's dates
                bool isOutside = (requestedFromDate > campaignItem.ToDate) || (requestedToDate < campaignItem.FromDate);

                // If requested dates are outside any campaign's date range, return false (no conflict)
                if (!isOutside)
                {
                    return campaignItem; // No overlap found
                }
            }

            // No overlap found, return false
            return null;
        }


    }
}
