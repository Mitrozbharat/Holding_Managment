using Microsoft.AspNetCore.Mvc;

namespace Hoarding_managment.Controllers
{
    public class OngoingCampainController : Controller
    {
        private readonly IOngoingCampain _context;
        public OngoingCampainController(IOngoingCampain ongoingCampain)
        {
            _context = ongoingCampain;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 9)
        {
            var campaign = await _context.GetallOngoingCampaignAsync(pageNumber, pageSize);
            var totalItems = await _context.GetOngoingCampaignCountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var viewModel = new CampaignPagedViewModel
            {
                Campaigns = campaign,
                CurrentPage = pageNumber,
                TotalPages = totalPages,

            };


            return View(viewModel);
        }


        public async Task<IActionResult> DeleteOngoingCampaign(int id)
        {
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


        [HttpPut]
        public async Task<IActionResult> UpdateOngoingCampaign(CampaignViewModel? model)
        {
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



        [HttpPost]
        public async Task<IActionResult> Addcampaingn([FromBody] QuotationItemListViewModel model)
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

        [HttpGet]
        public async Task<IActionResult> getallCampaignCount()
        {
            // Assuming GetOngoingCampaignCountAsync() returns an integer count value
            var upcomingeventCount = await _context.GetOngoingCampaignCountAsync();
            return Json(new { success = true, count = upcomingeventCount });
        }

    }
}
