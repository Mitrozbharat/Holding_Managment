using HoardingManagement.Interface;
using OfficeOpenXml;

namespace Hoarding_managment.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboard _context;
        private readonly db_hoarding_managementContext _dbContext;
        private readonly AutocompleteService _autocompleteService;
        private readonly IQuotation _Quoatations_context;
        IWebHostEnvironment hostingenvironment;
        private readonly ICustomer _customer;

        public DashboardController(IDashboard dashboard, IWebHostEnvironment hc, IQuotation quotation, db_hoarding_managementContext dbContext,ICustomer customer, AutocompleteService autocompleteService)
        {
            _context = dashboard;
            hostingenvironment = hc;
            _Quoatations_context = quotation;
            _dbContext = dbContext;
            _customer = customer;
            _autocompleteService = autocompleteService;

        }
        public IActionResult Index()
        {
            return View();
        }
        
    

       [HttpGet]
        public async Task<IActionResult> HoardingInventory(string searchQuery = "", int pageSize = 10, int pageNumber = 1)
        {
            var inventory = await _context.GetAllHoarldingInvenrotyAsync(searchQuery, pageNumber, pageSize);
            var totalItems = await _context.GetAllHoarldingInvenrotyCountAsync(searchQuery);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            InventoryPagedViewModel? viewModel = new InventoryPagedViewModel
            {
                InventoryViewModel = inventory,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize,
                SearchQuery = searchQuery
            };

            return View(viewModel);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByInventoryName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name cannot be empty.");
            }

            var customers = await _context.SearchByInventoryNameAsync(name);
            return Ok(customers);
        }



        [HttpDelete]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var inventory = await _context.GetInvetroyByIdAsync(id);
            if (inventory == null)
            {
                return Json(new { success = false, message = "Inventory not found." });
            }

            try
            {
                await _context.DeleteInventryAsync(id);
                return Json(new
                {
                    success = true,
                    message = "Deleted Successfully",
                    id = id
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateInventoryItems(int Id, string Image, string Area, string City, string width, string height, string Rate,int FkVendorId)
        {
            if (ModelState.IsValid)
            {
                //var existingItem =  _dbContext.GetInventryItemsByIdAsync(Id); // Use await to get the result
                //if (existingItem == null)
                //{

                    var inventoryItem = new TblInventoryitem
                    {
                        FkInventoryId = Id,
                        Image = Image,
                        Area = Area,
                        City = City,
                        Width = width,
                        Height = height,
                        Rate = Rate,
                        BookingStatus = 0,
                        FkVendorId = FkVendorId,
                        IsDelete = 0,
                        Type = 0,
                        Fkcustomer = 1
                    };

                    _dbContext.TblInventoryitems.Add(inventoryItem);
                _dbContext.SaveChanges();

                    return Json(new { success = true, Message = "Create Quotation successfully." });
                //}
                //else
                //{
                //    return Json(new { success = false, Message = "Please select another item." });
                //}
            }
            return Json(new { success = false, Message = "Creating error." });
        }

        public JsonResult GetVendorName(string query)
        {
            var subproduct = _autocompleteService.Getvendorname(query);

            var result = subproduct
              .Where(f => f.VendorName.ToLower().Contains(query.ToLower())).Select(s => new { Name = s.VendorName, Id = s.Id })
              .ToList();
            return Json(result);
        }
        public JsonResult GetBusinessName(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<object>()); // Return an empty list if the query is null or empty
            }

            // Fetch business names from the service
            var subproduct = _autocompleteService.Getbusinessname(query);
            if (subproduct == null)
            {
                return Json(new List<object>()); // Return an empty list if the service returned null
            }

            // Filter and project the result into a list of anonymous objects
            var result = subproduct
              .Where(f => f.BusinessName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
              .Select(s => new { Name = s.BusinessName, Id = s.Id })
              .ToList();

            return Json(result);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateInventoryItems(int id, string city, string area, string width, string height, string rate, string VendorName,int vendorid,string Image,string location,string vendoramt)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Model state is invalid." });
            }

            var existingItem = await _context.GetInventryByIdAsync(id);
            if (existingItem == null)
            {
                return Json(new { success = false, message = "Item not found." });
            }

            // Update the properties of the existing item
            existingItem.Id = id;
            existingItem.City = city;
            existingItem.Area = area;
            existingItem.Width = width;
            existingItem.Height = height;
            existingItem.Rate = rate;
            existingItem.FkVendorId = vendorid;
            existingItem.Image = Image;
            existingItem.Location = location;
            existingItem.VendorAmt = vendoramt;

            // Save changes to the database
            this._dbContext.SaveChanges();

            return Json(new { success = true, message = "Inventory item updated successfully." });
        }



        [HttpDelete]
        public async Task<IActionResult> DeletedSelectedInventryHoarding(int id)
        {

            var existingItem = await _context.GetInventryItemsByIdAsync(id);
            if (existingItem == null)
            {
                return Json(new { success = false, Message = "Deleteing Error seleced hoarding", model = 0 });
            }
            var deletecnf = await _context.DeleteInventryItemsAsync(id);

            Console.WriteLine("deleted id" + existingItem);

            return Json(new { success = true, Message = "Delete seleced hoarding", model = deletecnf });


        }

        [HttpGet]
        public async Task<IActionResult> getallCount()
        {
            try
            {
                int itemCount = await _context.InventryItemscountAsync();
                return Json(new { success = true, count = itemCount });
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return Json(new { success = false, message = "An error occurred while fetching the count." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> InventoryallCount()
        {
            try
            {
            
                int itemCount = await _context.InventoryCountAsync();

                return Json(new { success = true, count = itemCount });
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                return Json(new { success = false, message = "An error occurred while fetching the count." });
            }
        }

        public async Task<IActionResult> getSelectedHoarding()
        {
            var data = _context.GetInventryItemsAsync();

            if (data == null)
            {
                return Json(new { success = false, Message = " Data not Found " });

            }

            return Json(new { success = true, Message = "send ", model = data });
        }

        public async Task<IActionResult> GetAllInventoryItems()
        {
            var model = await _context.GetInventryItemsAsync();
            return View();
        }

        [HttpGet("SelectedHoarding")]
        public async Task<IActionResult> SelectedHoarding()
        {
            var model = await _context.GetInventryItemsAsync();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> SelectedHoardingJ()
        {
            var quotations = await _context.GetInventryItemsAsync();
            return Json(new { success = true, message = "Success", model = quotations });
        }
        [HttpPost]
        public  IActionResult GetHordingdata(int id)
        {
            if (id != null)
            {
                var getHordingItemsAgainstId = this._dbContext.TblQuotationitems.Where(x => x.FkQuotationId == id && x.IsDelete == 0).Select(item => new InventoryitemViewmodel
                {
                    Id = item.Id,
                    Image = item.Image,
                    City = item.City,
                    Area = item.Area,
                    Width = item.Width,
                    Height = item.Height,
                    Rate = item.Rate,
                    BookingStatus = item.BookingStatus,
                    type = (int?)item.Type,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                    FkInventoryId = 1,
                    Fkcustomer = item.FkCustomerId,
                    FkVendorId = (int)item.FkVendorId,
                    VendorName = _dbContext.TblVendors
                               .Where(v => v.Id == item.FkVendorId && v.IsDelete == 0)
                               .Select(v => v.VendorName)
                               .FirstOrDefault(),
                    CustomerName = _dbContext.TblCustomers
                               .Where(c => c.Id == item.FkCustomerId && c.IsDelete == 0)
                               .Select(c => c.CustomerName)
                               .FirstOrDefault(),
                    BusinessName = _dbContext.TblCustomers
                               .Where(c => c.Id == item.FkCustomerId && c.IsDelete == 0)
                               .Select(c => c.BusinessName)
                               .FirstOrDefault(),
                })
               .ToList();
                return Json(new { success = true, message = "Success", model = getHordingItemsAgainstId });
            }

            return Json(new { success = false, Message = " Data not Found " });
        }
        [HttpGet]
        public async Task<IActionResult> SelectedHoarding(int pageNumber = 1, int pageSize = 5)
        {
            var quotations = await _context.GetInventryItemsAsync();

            return Json(new { success = true, Message = "", model = quotations });
        }
        [HttpPost]
        public async Task<IActionResult> DeletedSelectInventoryHoarding(int id)
        {
            var existingItem = await _context.GetInventryItemsByIdAsync(id);
            if (existingItem == null)
            {
                return Json(new { success = false, message = "Error Deleting Hoarding", model = 0 });
            }

            var deleteCnf = await _context.DeleteInventryItemsAsync(id);
            Console.WriteLine("Deleted ID: " + existingItem.Id);

            return Json(new { success = true, message = "Hoarding Deleted successfully", model = deleteCnf });
        }

        [HttpGet]
        public async Task<IActionResult> getSelectInventoryHoarding()
        {

            var model = _context.get();
            return Json(new { success = true, message = "Selected hoarding deleted successfully", model = model });
        }


        [HttpPost]
        public async Task<IActionResult> SaveSelectedHoardings([FromBody] QuotationItemListViewModel model)
        {
            if (model == null || model.SelectedItems == null )
            {
                return BadRequest(new { success = false, message = "No items selected." });
            }

            try
            {

             var id =  await  _context.AddQuatationsAsync(model);

                return Ok(new { success = true, message = "Selected hoardings saved successfully." ,id =id});
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                return StatusCode(500, new { success = false, message = "An error occurred while saving the selected hoardings." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> addCampaign([FromBody] QuotationItemListViewModel model)
        {
            if (model == null || model.SelectedItems == null || !model.SelectedItems.Any())
            {
                return BadRequest(new { success = false, message = "No items selected." });
            }

            try
            {

                await _context.addCampaign(model);


                return Ok(new { success = true, message = "Campaign Created saved successfully." });
            }
            catch (Exception ex)
            {
                // Log the error (optional)
                return StatusCode(500, new { success = false, message = "Campaign saved error." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please upload a valid Excel file.");
            }

            try
            {
               
                // Set EPPlus license context
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        var records = new List<TblInventory>();

                        for (int row = 2; row <= rowCount; row++)
                        {
                            DateTime? date = null;
                            var dateString = worksheet.Cells[row, 4].Value?.ToString().Trim();
                            if (!string.IsNullOrEmpty(dateString) && DateTime.TryParse(dateString, out DateTime parsedDate))
                            {
                                date = parsedDate;
                            }
                            var record = new TblInventory
                            {
                                Image = worksheet.Cells[row, 2].Value?.ToString().Trim(),
                                City = worksheet.Cells[row, 3].Value?.ToString().Trim(),
                                Area = worksheet.Cells[row, 4].Value?.ToString().Trim(),
                                Location = worksheet.Cells[row, 5].Value?.ToString().Trim(),
                                Width = worksheet.Cells[row, 6].Value?.ToString().Trim(),
                                Height = worksheet.Cells[row, 7].Value?.ToString().Trim(),
                                Rate = worksheet.Cells[row, 8].Value?.ToString().Trim(),
                                VendorAmt = worksheet.Cells[row, 9].Value?.ToString().Trim(),
                                IsDelete =0,
                                CreatedAt = DateTime.Now,
                                CreatedBy = "Admin",
                                // Map other columns as needed
                            };

                            records.Add(record);
                        }

                        

                        await _dbContext.TblInventories.AddRangeAsync(records);
                        await _dbContext.SaveChangesAsync();
                    }
                }

                return Ok(new { message = "File uploaded and data saved." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error. " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewInventory(string city, string area, string location, string width, string height, string rate, string vendoramt, string Image,int vendorid)
        {
            
                var data = new TblInventory
                {
                    City = city,
                    Area = area,
                    Location = location,
                    Width = width,
                    Height = height,
                    Rate = rate,
                    VendorAmt = vendoramt,
                    Image = Image,
                    FkVendorId= vendorid,
                    IsDelete = 0,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin",
                };

                await _context.AddNewInventoryAsync(data);
                return Ok(new { Success = true, Message = "Customer  Add Successfully. " });
            
            return Json(new { Success = false, Message = "Invalid data." });

        }

        [HttpGet]
        public IActionResult SearchCustomersByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name cannot be empty.");
            }

            var customers = _customer.SearchCustomersByNameAsync(name);
            return Json(new {success=true,data= customers });
        }
    }
}

