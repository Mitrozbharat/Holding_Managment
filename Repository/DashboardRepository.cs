using Hoarding_managment.Data;
using Hoarding_managment.Models;
using Microsoft.EntityFrameworkCore;

namespace Hoarding_managment.Repository
{
    public class DashboardRepository : IDashboard
    {
        private readonly db_hoarding_managementContext _context;
        public DashboardRepository(db_hoarding_managementContext db_hoarding_managementContext)
        {
            _context = db_hoarding_managementContext;
        }

        // get all InventryHoardings
        public async Task<List<InventoryViewModel>> GetAllHoarldingInvenrotyAsync(int pageNumber, int pageSize)
        {
            List<InventoryViewModel> inventory = new List<InventoryViewModel>();

            var lst = _context.TblInventories
                     .Where(x => x.IsDelete == 0)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            foreach (var item in lst)
            {
                InventoryViewModel model = new InventoryViewModel();

                model.Id = item.Id;
                model.Image = item.Image;
                model.City = item.City;
                model.Area = item.Area;
                model.Rate = item.Rate;
                model.Width = item.Width;
                model.Height = item.Height;
                model.BookingStatus = item.BookingStatus;
                model.CreatedAt = item.CreatedAt;
                model.UpdatedAt = item.UpdatedAt;
                model.VendorName = _context.TblVendors.FirstOrDefault(x => x.Id == item.FkVendorId)?.VendorName;
                model.FkVendorId = item.FkVendorId;
                inventory.Add(model);
            }
            return inventory;

        }
        public async Task<TblInventory> GetInvetroyByIdAsync(int id)
        {
            return await _context.TblInventories.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == 0);
        }
        public async Task<int> DeleteInventryAsync(int id)
        {
            var existcustomer = _context.TblInventories.FirstOrDefault(x => x.Id == id);
            if (existcustomer != null)
            {

                existcustomer.IsDelete = 1;
                await _context.SaveChangesAsync();
                return 0;
            }
            return 1;
        }
        public async Task<TblInventoryitem> CreateInventryItemsAsync(TblInventoryitem model)
        {
            _context.TblInventoryitems.Add(model);
            await _context.SaveChangesAsync();

            return model;
        }
        public async Task<TblInventory> UpdateInventoryItemAsync(TblInventory model)
        {
            try
            {
                //_context.TblInventoryitems.Update();
                await _context.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                // Optionally, log the exception and handle it as needed
                throw new Exception("An error occurred while updating the inventory item.", ex);
            }
        }
        public async Task<TblInventory> GetInventryItemsByIdAsync(int id)
        {
            var inventory = await _context.TblInventories.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == 0);
            if (inventory != null)
            {

                return inventory;

            }
            return inventory;



        }


        public async Task<int> InventryItemscountAsync()
        {
            return await _context.TblInventoryitems
                                 .Where(x => x.IsDelete == 0)
                                 .CountAsync();
        }


        public async Task<int> InventoryCountAsync()
        {
            return await _context.TblInventories
                                 .Where(x => x.IsDelete == 0)
                                 .CountAsync();
        }
        public async Task<int> DeleteInventryItemsAsync(int id)
        {
            var existcustomer = _context.TblInventoryitems.FirstOrDefault(x => x.Id == id);
            if (existcustomer != null)
            {

                existcustomer.IsDelete = 1;
                await _context.SaveChangesAsync();
                return 0;
            }
            return 1;
        }
        public async Task<TblInventory> AddInvenrotyAsync(TblInventory model)
        {
            _context.TblInventories.AddAsync(model);
            await _context.SaveChangesAsync();

            return model;
        }
        public async Task<TblInventory> UpdateInvenrotyAsync(int id, TblInventory model)
        {
            _context.TblInventories.Update(model);
            await _context.SaveChangesAsync();

            return model;
        }
        public async Task<TblInventory> GetInventoryByIdAsync(int id)
        {
            return await _context.TblInventories.FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == 0);
        }
        public async Task<int> GetAllHoarldingInvenrotyCountAsync()
        {
            return await _context.TblInventories.CountAsync();
        }
        public async Task<int> DeleteInvenrotyAsync(int id)
        {
            var exitinventory = await _context.TblInventories
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDelete == 0);

            if (exitinventory == null)
            {
                return 0;
            }
            // Mark the campaign as deleted
            exitinventory.IsDelete = 1;

            // Update the campaign in the database
            _context.TblInventories.Update(exitinventory);
            await _context.SaveChangesAsync();

            // Return 1 (indicating a successful deletion)
            return exitinventory.Id;
        }

        public async Task<List<InventoryitemViewmodel>> GetInventryItemsAsync()
        {
            var inventoryItems = await _context.TblInventoryitems
         .Where(x => x.IsDelete == 0)
         .Select(item => new InventoryitemViewmodel
         {
             Id = item.Id,
             Image = item.Image,
             City = item.City,
             Area = item.Area,
             Width = item.Width,
             Height = item.Height,
             Rate = item.Rate,
             BookingStatus = item.BookingStatus,
             IsLight = item.IsLight,
             CreatedAt = item.CreatedAt,
             UpdatedAt = item.UpdatedAt,
             FkInventoryId = item.FkInventoryId,
             Fkcustomer = item.Fkcustomer,
             FkVendorId = (int)item.FkVendorId,
             VendorName = _context.TblVendors
                 .Where(v => v.Id == item.FkVendorId && v.IsDelete == 0)
                 .Select(v => v.VendorName)
                 .FirstOrDefault(),
             CustomerName = _context.TblCustomers
                 .Where(c => c.Id == item.Fkcustomer && c.IsDelete == 0)
                 .Select(c => c.CustomerName)
                 .FirstOrDefault(),
             BusinessName = _context.TblCustomers
                 .Where(c => c.Id == item.Fkcustomer && c.IsDelete == 0)
                 .Select(c => c.BusinessName)
                 .FirstOrDefault(),
         })
         .ToListAsync();


            return inventoryItems;
        }
        public Task<List<InventoryitemViewmodel>> get()
        {
            var selectedInvert = _context.TblInventoryitems
                .Where(x => x.IsDelete == 0)  // Filtering only non-deleted items
                .Select(x => new InventoryitemViewmodel
                {
                    Id = x.Id,
                    Image = x.Image,
                    Area = x.Area,
                    City = x.City,
                    Width = x.Width,
                    Height = x.Height,
                    Rate = x.Rate,
                    BookingStatus = x.BookingStatus,
                    IsLight = x.IsLight,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    IsDelete = x.IsDelete,
                    FkInventoryId = x.FkInventoryId,
                    Fkcustomer = x.Fkcustomer,
                    FkVendorId = (int)x.FkVendorId
                })
                .ToListAsync(); // Use ToListAsync to return a Task<List<InventoryitemViewmodel>>

            return selectedInvert;
        }
        public async Task<QuotationItemListViewModel> AddQuatationsAsync(QuotationItemListViewModel selectedItems)
        {
            var lastQuotation = await _context.TblQuotations
                .Where(x => x.IsDelete == 0)
                .OrderByDescending(x => x.QuotationNumber)
                .FirstOrDefaultAsync();

            string lastNumberPart = lastQuotation != null ? lastQuotation.QuotationNumber.Replace("Q", "") : "000";
            int nextNumber = int.Parse(lastNumberPart) + 1;
            string nextQuotationNumber = $"Q{nextNumber:D3}";

            var data = new TblQuotation
            {
                QuotationNumber = nextQuotationNumber,
                CreatedAt = DateTime.Now,
                CreatedBy = "admin",
                FkCustomerId = selectedItems.CustomerId,
                IsDelete = 0,
               
            };

            await _context.TblQuotations.AddAsync(data);
            await _context.SaveChangesAsync();

            int qid = data.Id;
            if (qid != 0)
            {
                foreach (var item in selectedItems.SelectedItems)
                {
                    var inventoryitems = _context.TblInventoryitems.Where(x => x.Id == item.Id).FirstOrDefault();
                    var inventory = _context.TblInventories.Where(x => x.Id == inventoryitems.FkInventoryId).FirstOrDefault();

                    var newdata = new TblQuotationitem
                    {
                        FkCustomerId = selectedItems.CustomerId,
                        Rate = item.Rate,
                        FkQuotationId = qid,
                        City = inventoryitems.City,
                        Area = inventoryitems.Area,
                        Location = inventory.Location,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDelete = 0,
                        Height = inventoryitems.Height,
                        Width = inventoryitems.Width,
                        BookingStatus = inventoryitems.BookingStatus,
                        IsLight = inventory.IsLight,
                        VendorAmt = inventory.VendorAmt,
                        Image = inventoryitems.Image,
                        LocationDescription = inventory.Location,
                        FkVendorId = inventoryitems.FkVendorId,
                        UpdatedBy = "admin",
                        CreatedBy = "Admin",
                        FkInventory = item.FkInventoryId
                    };

                    _context.TblQuotationitems.Add(newdata);
                    var recor = await _context.SaveChangesAsync();

                    if (recor > 0)
                    {
                        inventoryitems.IsDelete = 1;
                        _context.Update(inventoryitems);
                        _context.SaveChanges();
                    }



                }
            }

            return null;
        }

        public async Task<QuotationItemListViewModel> addCampaign(QuotationItemListViewModel selectedItems)
        {


            if (selectedItems.CustomerId != null && selectedItems.SelectedItems != null)
            {
                foreach (var item in selectedItems.SelectedItems)
                {
                    var inventoryitems = _context.TblInventoryitems.Where(x => x.Id == item.Id).FirstOrDefault();
                    var inventory = _context.TblInventories.Where(x => x.Id == inventoryitems.FkInventoryId).FirstOrDefault();

                    var newdata = new TblCampaign
                    {
                        FkCustomerId = selectedItems.CustomerId,
                        FkInventoryId = item.FkInventoryId,
                        FromDate = DateTime.Now,
                        ToDate = DateTime.Now,
                        BookingAmt = item.Rate,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDelete = 0,
                        UpdatedBy = "admin",
                        CreatedBy = "Admin"
                    };

                    _context.TblCampaigns.Add(newdata);
                    await _context.SaveChangesAsync();

                }
            }

            return null;
        }

        public async Task<TblInventory> UploadExcelAsync(TblInventory inventory)
        {
            return inventory;
        }
        public async Task<TblInventory> AddNewInventoryAsync(TblInventory model)
        {
            _context.TblInventories.Add(model);
            await _context.SaveChangesAsync();
            return model;

        }
        //    public async Task<TblInventory> UploadExcelAsync(TblInventory inventory)
        //    {
        //        // Set EPPlus license context
        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //        using (var stream = new MemoryStream())
        //        {
        //            await file.CopyToAsync(stream);
        //            using (var package = new ExcelPackage(stream))
        //            {
        //                var worksheet = package.Workbook.Worksheets[0];
        //                var rowCount = worksheet.Dimension.Rows;

        //                var records = new List<TblInventory>();

        //                for (int row = 2; row <= rowCount; row++)
        //                {
        //                    DateTime? date = null;
        //                    var dateString = worksheet.Cells[row, 4].Value?.ToString().Trim();
        //                    if (!string.IsNullOrEmpty(dateString) && DateTime.TryParse(dateString, out DateTime parsedDate))
        //                    {
        //                        date = parsedDate;
        //                    }
        //                    var record = new TblInventory
        //                    {
        //                        Image = worksheet.Cells[row, 2].Value?.ToString().Trim(),
        //                        City = worksheet.Cells[row, 3].Value?.ToString().Trim(),
        //                        Area = worksheet.Cells[row, 3].Value?.ToString().Trim(),
        //                        Location = worksheet.Cells[row, 3].Value?.ToString().Trim(),
        //                        Width = worksheet.Cells[row, 3].Value?.ToString().Trim(),
        //                        Height = worksheet.Cells[row, 3].Value?.ToString().Trim(),
        //                        Rate = worksheet.Cells[row, 3].Value?.ToString().Trim(),
        //                        VendorAmt = worksheet.Cells[row, 3].Value?.ToString().Trim(),
        //                        // Map other columns as needed
        //                    };

        //                    records.Add(record);
        //                }





        //            }
        //            return inventory;
        //    }
    }

}
