
using System.Text;

namespace Hoarding_managment.Controllers
{
    public class QuatationController : Controller
    {
        private readonly IQuotation _context;
        private readonly IDashboard _Dashcontext;
        public QuatationController(IQuotation quotationRepository, IDashboard dashboard)
        {
            _context = quotationRepository;
            _Dashcontext = dashboard;
        }
       

        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery = "", int pageSize = 10, int pageNumber = 1)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            ViewBag.sessionUserId = sessionUserId;
            ViewBag.sessionUserName = sessionUserName;



            if (sessionUserId == null || sessionUserId == 0)
            {
                return RedirectToAction("Index", "Auth");
            }
            List<QuatationViewModel>? quotations = await _context.GetAllQuotationsListAsync(searchQuery, pageNumber, pageSize);
            var totalItems = await _context.GetQuotationCountAsync(searchQuery);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            QuatationPagedViewModel? viewModel = new QuatationPagedViewModel
            {
                QuatationViewModel = quotations,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize,
                SearchQuery = searchQuery
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetQuataionsJ(int pageNumber = 1, int pageSize = 10)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");



            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
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
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
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
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");



            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            if (id <= 0)
            {
                return BadRequest();
            }

            var quotation = await _context.GetQuotationByIdDetailAsync(id);


         

            if (quotation == null)
            {
                return NotFound();
            }

            return View(quotation);
        }


        [HttpGet]
        public async Task<IActionResult> LastQuotation(int id)
        {

            var viewmodel = _context.GetLatestQuotationById(id);

            return View(viewmodel);
        }



        [HttpGet("search")]
        public async Task<IActionResult> SearchByCustomerName(string name)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name cannot be empty.");
            }

            var customers = await _context.SearchByCustomerNameAsync(name);
            return Ok(customers);
        }



        [HttpPost]
        public async Task<IActionResult> DeleteQuatationitem(int id)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");


            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            var qitemId = await _context.findQuotationitemByIdAsync(id);
            if (qitemId != null)
            {
                var deleteCampaign = await _context.DeleteQuotationitemByIdAsync(qitemId);
                return Json(new
                {
                    success = true,
                    Message = "Delete Inventory Successfully",
                    id = deleteCampaign
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
        public IActionResult GenerateExcel([FromBody] QuotationData model)
        {
            var sessionUserId = HttpContext.Session.GetInt32("SessionUserIdKey");

            var sessionUserName = HttpContext.Session.GetString("SessionUsername");

            if (sessionUserId == null)
            {
                return RedirectToAction("Index", "Auth");
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Quotation Data");

                // Add header row for quotation details
                worksheet.Cells[1, 1].Value = "Quotation Number";
                worksheet.Cells[1, 2].Value = model.QuotationNumber;

                worksheet.Cells[2, 1].Value = "Date";
                worksheet.Cells[2, 2].Value = model.QuotationDate;

                worksheet.Cells[3, 1].Value = "Business Name";
                worksheet.Cells[3, 2].Value = model.BusinessName;

                //worksheet.Cells[4, 1].Value = "Address";
                //worksheet.Cells[4, 2].Value = model.Address;
                string truncatedAddress = model.Address.Length > 30 ? model.Address.Substring(0, 30) + "..." : model.Address;
                worksheet.Cells[4, 1].Value = "Address";
                worksheet.Cells[4, 2].Value = model.Address;

                worksheet.Cells[5, 1].Value = "Total Amount";
                worksheet.Cells[5, 2].Value = model.TotalAmount;

                // Leave a gap, then start the table for items
                int startRow = 7;

                // Add header row for items
                worksheet.Cells[startRow, 1].Value = "City";
                worksheet.Cells[startRow, 2].Value = "Area";
                worksheet.Cells[startRow, 3].Value = "Location";
                worksheet.Cells[startRow, 4].Value = "Width";
                worksheet.Cells[startRow, 5].Value = "Height";
                worksheet.Cells[startRow, 6].Value = "Type";
                worksheet.Cells[startRow, 7].Value = "Price";

                // Add data rows for items
                int row = startRow + 1;
                foreach (var item in model.Items)
                {
                    worksheet.Cells[row, 1].Value = item.City;
                    worksheet.Cells[row, 2].Value = item.Area;
                    worksheet.Cells[row, 3].Value = item.Location;
                    worksheet.Cells[row, 4].Value = item.Width;
                    worksheet.Cells[row, 5].Value = item.Height;

                    string type = item.type switch
                    {
                        "0" => "FL",
                        "1" => "LIT",
                        "2" => "NL",
                        _ => "NL" // Default value
                    };

                    worksheet.Cells[row, 6].Value = type;
                    worksheet.Cells[row, 7].Value = item.Price;
                    row++;
                }


                // AutoFit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var excelFile = package.GetAsByteArray();
                var fileName = "Quotation_" + model.QuotationNumber + ".xlsx";

                // Return the file as an Excel download
                return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }






        [HttpGet("Quatation/GenerateLink/{id}")]
        public async Task<IActionResult> GenerateLink(string id)
        { // Decode the Base64-encoded ID directly in the action
            byte[] data;
            try
            {
                data = Convert.FromBase64String(id);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid.");
            }

            string decodedString = Encoding.UTF8.GetString(data);

            if (!int.TryParse(decodedString, out int decodedId) || decodedId <= 0)
            {
                return BadRequest("Invalid ID format.");
            }

            // Fetch the quotation by the decoded ID
            var quotation = await _context.GetQuotationByIdDetailAsync(decodedId);

            if (quotation == null)
            {
                return NotFound();
            }

            ViewBag.ItemsJson = JsonConvert.SerializeObject(quotation.Items);

            return View(quotation);
        }

     
    }
}
