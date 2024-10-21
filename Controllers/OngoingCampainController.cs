using Hoarding_management.Data;

namespace Hoarding_managment.Controllers
{
    public class OngoingCampainController : Controller
    {
        private readonly IOngoingCampain _context;
        private readonly db_hoarding_managementContext _dbContext;
        public OngoingCampainController(db_hoarding_managementContext dbContext, IOngoingCampain ongoingCampain)
        {
            _context = ongoingCampain;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery = "", int pageSize = 10, int pageNumber = 1)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            ViewBag.sessionUserId = sessionUserId;
            ViewBag.sessionUserName = sessionUserName;


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            var campaign = await _context.GetCampaignAsync(searchQuery, pageNumber, pageSize);
            var totalItems = await _context.GetOngoingCampaignCountAsync(searchQuery);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            CampaignPagedViewModel? viewModel = new CampaignPagedViewModel
            {
                CampaignsViewModel = campaign,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize,
                SearchQuery = searchQuery
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CompletedCampain(string searchQuery = "", int pageSize = 10, int pageNumber = 1)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            ViewBag.sessionUserId = sessionUserId;
            ViewBag.sessionUserName = sessionUserName;


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            var campaign = await _context.CompletedOngoingcampaignAsync(searchQuery, pageNumber, pageSize);
            var totalItems = await _context.GetCompletedCampaignCountAsync(searchQuery);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            CampaignPagedViewModel? viewModel = new CampaignPagedViewModel
            {
                CampaignsViewModel = campaign,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize,
                SearchQuery = searchQuery
            };

            return View(viewModel);
        }

        //[HttpGet]
        //public async Task<IActionResult> Index(string searchQuery = "", int pageSize = 10, int pageNumber = 1)
        //{
        //    var campaign = await _context.GetallOngoingCampaignAsync(searchQuery, pageNumber, pageSize);
        //    var totalItems = await _context.GetOngoingCampaignCountAsync(searchQuery);
        //    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        //    CampaignPagedViewModel? viewModel = new CampaignPagedViewModel
        //    {
        //        CampaignsViewModel = campaign,
        //        CurrentPage = pageNumber,
        //        TotalPages = totalPages,
        //        PageSize = pageSize,
        //        SearchQuery = searchQuery
        //    };

        //    return View(viewModel);
        //}

        //[HttpGet]
        //public async Task<IActionResult> CompletedCampain(string searchQuery = "", int pageSize = 10, int pageNumber = 1)
        //{
        //    var campaign = await _context.GetallCompletedCampaignAsync(searchQuery, pageNumber, pageSize);
        //    var totalItems = await _context.GetCompletedCampaignCountAsync(searchQuery);
        //    var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        //    CampaignPagedViewModel? viewModel = new CampaignPagedViewModel
        //    {
        //        CampaignsViewModel = campaign,
        //        CurrentPage = pageNumber,
        //        TotalPages = totalPages,
        //        PageSize = pageSize,
        //        SearchQuery = searchQuery
        //    };

        //    return View(viewModel);
        //}



        [HttpGet("search")]
        public async Task<IActionResult> SearchByCampaignName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name cannot be empty.");
            }

            var customers = await _context.SearchByCampaignNameAsync(name);
            return Ok(customers);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteCampaign(int id)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            ViewBag.sessionUserId = sessionUserId;
            ViewBag.sessionUserName = sessionUserName;


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            var Campaign = await _context.GetCampaingnByIdAsync(id);
            if (Campaign != null)
            {
                var deleteCampaign = await _context.DeleteCampaignAsync(id);


                return Json(new
                {
                    success = true,
                    Message = "Delete Campaign Successfully",
                    id = id
                });

            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Deleting Error..."
                });
            }
        }


        [ HttpPost]
        public async Task<IActionResult> UpdateCampaign(CampaigneditViewModel model)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            ViewBag.sessionUserId = sessionUserId;
            ViewBag.sessionUserName = sessionUserName;


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            if (model == null)
            {
                return Json(new { success = false, Meassage = "Invalid data" });
            }

            var Campaign = _context.GetCampaingnByIdAsync(model.Id);
            if (Campaign == null)
            {
                return Json(new { success = false, Meassage = "Campaign not found" });
            }


            await _context.UpdateCampaignAsync(model);

            return Json(new { success = true, Meassage = "Updated successfully" });
        }



      
        [HttpGet]
        public async Task<IActionResult> getallCampaignCount()
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            ViewBag.sessionUserId = sessionUserId;
            ViewBag.sessionUserName = sessionUserName;


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            // Assuming GetOngoingCampaignCountAsync() returns an integer count value
            var upcomingeventCount = await _context.GetOngoingCampaignCountAsync();
            return Json(new { success = true, count = upcomingeventCount });
        }
        
        [HttpGet]
        public async Task<IActionResult> getallQuotationCount()
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            ViewBag.sessionUserId = sessionUserId;
            ViewBag.sessionUserName = sessionUserName;


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            // Assuming GetOngoingCampaignCountAsync() returns an integer count value
            var upcomingeventCount = await _dbContext.TblQuotations.Where(x=>x.IsDelete==0).Select(x=>x.Id).CountAsync();
            return Json(new { success = true, count = upcomingeventCount });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOngoingCount()
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            ViewBag.sessionUserId = sessionUserId;
            ViewBag.sessionUserName = sessionUserName;


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            // Join TblCampaignnews with TblCampaignitems and count ongoing campaigns
            var ongoingEventCount = await (from campaignNews in _dbContext.TblCampaignnews
                                           join campaignItem in _dbContext.TblCampaingitems
                                           on campaignNews.Id equals campaignItem.FkCampaignId // Assuming FkCampaignId is the foreign key
                                           where campaignNews.IsDelete == 0
                                           && campaignItem.ToDate >= DateTime.Today
                                           select campaignNews).CountAsync(); // Use CountAsync for better performance

            return Json(new { success = true, count = ongoingEventCount });
        }


        [HttpGet]
        public async Task<IActionResult> getallongoingDuedateCount()
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            ViewBag.sessionUserId = sessionUserId;
            ViewBag.sessionUserName = sessionUserName;


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            // Define the date range for campaigns expiring in the next 14 days
            var twoWeeksFromToday = DateTime.Today.AddDays(14);

            // Count campaigns that are expiring within two weeks
            var expiringCampaignsCount = await (from campaignNews in _dbContext.TblCampaignnews
                                                join campaignItem in _dbContext.TblCampaingitems
                                                on campaignNews.Id equals campaignItem.FkCampaignId
                                                where campaignNews.IsDelete == 0
                                                && campaignItem.ToDate >= DateTime.Today
                                                && campaignItem.ToDate <= twoWeeksFromToday
                                                select campaignItem).CountAsync();

            // Count upcoming campaigns with a due date later than today
            var upcomingCampaignsCount = await (from campaignNews in _dbContext.TblCampaignnews
                                                join campaignItem in _dbContext.TblCampaingitems
                                                on campaignNews.Id equals campaignItem.FkCampaignId
                                                where campaignNews.IsDelete == 0
                                                && campaignItem.ToDate > DateTime.Today
                                                select campaignItem).CountAsync();

            // Return the counts in the response
            return Json(new { success = true, expiringCount = expiringCampaignsCount, upcomingCount = upcomingCampaignsCount });
        }


        [HttpGet]
        public async Task<IActionResult> getallhordingandVendorCount()
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            ViewBag.sessionUserId = sessionUserId;
            ViewBag.sessionUserName = sessionUserName;


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            // Assuming GetOngoingCampaignCountAsync() returns an integer count value
            //  var upcomingeventCount = await _context.GetOngoingCampaignCountAsync();
            var horingcount = await _dbContext.TblInventories.Where(x => x.IsDelete == 0).Select(x=>x.Id).CountAsync();
            var vendorcount = await _dbContext.TblVendors.Where(x => x.IsDelete == 0).Select(x=>x.Id).CountAsync();
            return Json(new { success = true, hcount = horingcount,vcount= vendorcount });
        }

        [HttpGet]
        public async Task<IActionResult> GetWeeklyQuotationData(DateTime startDate, DateTime endDate)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            ViewBag.sessionUserId = sessionUserId;
            ViewBag.sessionUserName = sessionUserName;


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            // Fetch the quotations that fall within the given date range
            var quotations = await _dbContext.TblQuotations
                .Where(q => q.IsDelete == 0 && q.CreatedAt >= startDate && q.CreatedAt <= endDate)
                .Select(q => new
                {
                    q.Id,
                    q.CreatedAt
                })
                .ToListAsync();

            return Json(quotations);
        }

        [HttpPost]
        public async Task<IActionResult> Addcampaingn([FromBody] QuotationItemListViewModel model)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            ViewBag.sessionUserId = sessionUserId;
            ViewBag.sessionUserName = sessionUserName;


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
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
        public async Task<IActionResult> ValidateDatesCampaign(int id, DateTime requestedFromDate, DateTime requestedToDate, int status)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

           
            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            var existingCampaign = await _context.IsCampaignBooked(id, requestedFromDate, requestedToDate, status);

            if (existingCampaign== null)
            {
                // Valid date range, return success message
                return Json(new
                {
                    success = true,
                    message = "Valid date."
                });
            }

            // Conflicting campaign found, return error message
            return Json(new
            {
                success = false,
                message = $"Select valid Dates .Already ongoing campaign {existingCampaign.FromDate:dd/MM/yyyy} To {existingCampaign.ToDate:dd/MM/yyyy}.",
                model = existingCampaign.ToDate
            });
        }


    }
}
