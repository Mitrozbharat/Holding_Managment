using DocumentFormat.OpenXml.Office2010.Excel;
using Hoarding_management.Data;
using Hoarding_managment.Data;
using System.Text;
using System.Web;

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
            List<CampaignViewModel>? campaign = await _context.GetCampaignAsync(searchQuery, pageNumber, pageSize);
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
        public async Task<IActionResult> DeleteCampaign(int id, int fk_id)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            ViewBag.sessionUserId = sessionUserId;
            ViewBag.sessionUserName = sessionUserName;


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            var Campaign = await _context.GetCampaingndelByIdAsync(id);
            if (Campaign != null)
            {
                var deleteCampaign = await _context.DeleteCampaignAsync(Campaign, fk_id);


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


        [HttpPost]
        public async Task<IActionResult> UpdateCampaign(CampaigneditViewModel model)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");
            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }

            if (model == null)
            {
                return Json(new { success = false, Message = "Invalid data" });
            }

            var campaign = await _context.GetCampaingnByIdAsync(model.Id);
            if (campaign == null)
            {
                return Json(new { success = false, Message = "Campaign not found" });
            }

            int i = await _context.UpdateCampaignAsync(model);
            if (i >= 1)
            {
                return Json(new { success = true, Message = "Updated successfully" });

            }

            return Json(new { success = false, Message = "something wants to Wrong" });


        }


        [HttpPost]
        public async Task<IActionResult> checkValidatedate(CampaigneditViewModel model)
        {
            try
            {
                // Retrieve all campaigns with the same FkCampaignId and FkInventoryId, excluding deleted ones
                var campaignsToUpdate = await _dbContext.TblCampaingitems
                    .Where(x => x.FkCampaignId == model.Id && x.FkInventoryId == model.fk_id && x.IsDelete == 0)
                    .ToListAsync();

                // Check if the requested FromDate falls within any existing date range
                bool isOverlap = campaignsToUpdate.Any(campaign =>
                    model.FromDate >= campaign.FromDate && model.FromDate <= campaign.ToDate
                );

                if (isOverlap)
                {
                    return Json(new { success = false, message = "The requested FromDate overlaps with existing campaign dates." });
                }

                return Json(new { success = true, message = "Dates are valid." });
            }
            catch (Exception ex)
            {
                // Log the exception message
                Console.WriteLine(ex.Message);
                // Consider logging the error properly using a logging framework
                return Json(new { success = false, message = "An error occurred while validating the dates." });
            }
        }


        [HttpPost]
        public async Task<IActionResult> checkTodateValidatedate(CampaigneditViewModel model)
        {
            try
            {
                // Retrieve all campaigns with the same FkCampaignId and FkInventoryId, excluding deleted ones
                var campaignsToUpdate = await _dbContext.TblCampaingitems
                    .Where(x => x.FkCampaignId == model.Id && x.FkInventoryId == model.fk_id && x.IsDelete == 0)
                    .ToListAsync();

                // Check if the requested ToDate falls within any existing date range
                bool isOverlap = campaignsToUpdate.Any(campaign =>
                    model.ToDate >= campaign.FromDate && model.ToDate <= campaign.ToDate
                );

                if (isOverlap)
                {
                    return Json(new { success = false, message = "The requested ToDate overlaps with existing campaign dates." });
                }

                return Json(new { success = true, message = "validate dates." });
            }
            catch (Exception ex)
            {
                // Log the exception message
                Console.WriteLine(ex.Message);
                // Consider logging the error properly using a logging framework
                return Json(new { success = false, message = "An error occurred while validating the ToDate." });
            }
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
            var upcomingeventCount = await _dbContext.TblQuotations.Where(x => x.IsDelete == 0).Select(x => x.Id).CountAsync();
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
            var horingcount = await _dbContext.TblInventories.Where(x => x.IsDelete == 0).Select(x => x.Id).CountAsync();

            // int ids = await _dbContext.TblVendors.FirstOrDefaultAsync( x => x.IsDelete == 0 && x.VendorName == "Sahu Advertising").Id;

            var vendor = await _dbContext.TblVendors
           .FirstOrDefaultAsync(x => x.IsDelete == 0 && x.Id == 1);

            int ids = vendor?.Id ?? 0; // Use 0 or another default value if vendor is null

            var vencount = await _dbContext.TblInventories.Where(x => x.IsDelete == 0 && x.FkVendorId == ids).CountAsync();

            var bookingwonHordings = await _dbContext.TblInventories.Where(x => x.IsDelete == 0 && x.FkVendorId == ids && x.BookingStatus == 1).CountAsync();


            return Json(new { success = true, hcount = horingcount, vcount = vencount, bookingwonHordings = bookingwonHordings });
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

            if (existingCampaign == null)
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


        [HttpGet]
        public async Task<IActionResult> GetBookedRanges(int id, int status)
        {
            int? FkId = null;

            if (status == 0)
            {
                FkId = await _dbContext.TblInventoryitems
                    .Where(x => x.Id == id)
                    .Select(x => x.FkInventoryId)
                    .FirstOrDefaultAsync();
            }
            else if (status == 1)
            {
                FkId = await _dbContext.TblQuotationitems
                    .Where(x => x.Id == id)
                    .Select(x => x.FkInventory)
                    .FirstOrDefaultAsync();
            }

            // Return null if no FkId was found
            if (FkId == null) return Json(new { success = false, message = "No valid FkId found." });

            // Query tbl_campaignitems to check for any overlapping date ranges for the given inventory item
            List<DateRangeRequest> dateRanges = await _dbContext.TblCampaingitems
                .Where(c => c.FkInventoryId == FkId && c.IsDelete == 0)
                .Select(c => new DateRangeRequest
                {
                    Id = c.Id,
                    FromDate = (DateTime)c.FromDate,
                    ToDate = (DateTime)c.ToDate
                })
                .ToListAsync();

            return Json(new { success = true, model = dateRanges });
        }

        [HttpPost]
        public IActionResult SavePaymentCash([FromBody] PaymentDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.PaymentMode))
                return BadRequest("Invalid data");

            // 1️⃣ Save Payment
            var payment = new TblPayment
            {
                FkCampaignId = int.Parse(dto.CampaignId),
                Amount = dto.Amount,
                PaymentType = dto.PaymentMode,
                PaymentStatus = "Completed",
                CreatedAt = DateTime.Now,
                CreatedBy = dto.CustomerName
            };
            _dbContext.TblPayments.Add(payment);

            // 2️⃣ Update the campaign item IsPaid & Amount
            var campaignItem = _dbContext.TblCampaingitems
                .FirstOrDefault(c => c.Id.ToString() == dto.itemid && c.FkCampaignId.ToString() == dto.CampaignId);

            if (campaignItem != null)
            {
                campaignItem.Ispaid = true; // Mark as paid
                campaignItem.Amount = dto.Amount.ToString();
                _dbContext.TblCampaingitems.Update(campaignItem);
            }

            _dbContext.SaveChanges();

            return Ok(new { message = "Payment recorded successfully" });
        }

        [HttpGet]
        public async  Task<IActionResult> PaymentHostory(string searchQuery = "", int pageSize = 10, int pageNumber = 1)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            ViewBag.sessionUserId = sessionUserId;
            ViewBag.sessionUserName = sessionUserName;


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            List<CampaignViewModel>? campaign = await _context.GetCampaignAsync(searchQuery, pageNumber, pageSize);
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
        public IActionResult PaymentQR(string data)
        {
            if (string.IsNullOrEmpty(data))
                return RedirectToAction("Index");

            string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(data));

            var model = new PaymentQRViewModel
            {
                QRString = decoded
            };

            return View(model);
        }

      

        
        [HttpPost]
        public async Task<IActionResult> SavePayment(int cid,int itemid, string customer,string upi,decimal amount, string mode, string mobile, string rawQr)
        {
            if (cid == null)
                return BadRequest("Invalid data.");



            var payment = new TblPayment
            {
                FkCampaignId = cid,
                Amount = amount,
                PaymentType =mode,
                PaymentStatus = "Completed",
                CreatedAt = DateTime.Now,
                CreatedBy = customer
            };
            _dbContext.TblPayments.Add(payment);

            var campaignItem = _dbContext.TblCampaingitems
                           .FirstOrDefault(c => c.Id == itemid && c.FkCampaignId == cid);

            if (campaignItem != null)
            {
                campaignItem.Ispaid = true; // Mark as paid
                campaignItem.Amount = amount.ToString();
                _dbContext.TblCampaingitems.Update(campaignItem);
            }

            _dbContext.SaveChanges();

            return Ok(new { message = "Payment recorded successfully" });

            return Ok();
        }



    }
}
