using HoardingManagement.Interface;
using Newtonsoft.Json;

namespace Hoarding_managment.Controllers
{
    public class QuatationController : Controller
    {
        private readonly IQuotation _context;
        private readonly IDashboard _Dashcontext;
        public QuatationController(IQuotation quotationRepository, IDashboard dashboard)
        {
            _context = quotationRepository;
            _Dashcontext= dashboard;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 9)
        {
            var quotations = await _context.GetAllQuotationsListAsync(pageNumber, pageSize);
            var totalItems = await _context.GetQuotationCountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var viewModel = new QuatationPagedViewModel
            {
                QuatationViewModel = quotations,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetQuataionsJ(int pageNumber = 1, int pageSize = 10)
        {
            var quotations = await _context.GetAllQuotationsListAsync(pageNumber, pageSize);


            if (quotations.Count > 0) 
            {
                return Json(new { success = true, message = "Success", model = quotations });

            }
            return Json(new { success = false, message = "Do not have any Quotations", model = 0 });


        }


        [HttpDelete]
        public async Task<IActionResult> DeleteQuotation(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var deleteQuotation = await _context.DeleteQuotationAsync(id);
            if (deleteQuotation == null)
            {
                return Json(new { success = false, message = "Error deleting quotation." });
            }

            return Json(new { success = true, message = "Delete Success", model = deleteQuotation });
        }

        [HttpGet]
        public async Task<IActionResult> ViewQuotation(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var quotation = await _context.GetQuotationByIdDetailAsync(id);


            ViewBag.ItemsJson = JsonConvert.SerializeObject(quotation.Items);


            if (quotation == null)
            {
                return NotFound();
            }

            return View(quotation);
        }





       


       

    }
}
