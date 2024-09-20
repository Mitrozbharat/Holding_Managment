
namespace Hoarding_managment.Controllers
{
    public class VendorController : Controller
    {
        private readonly IVendor _context;

        public VendorController(IVendor vendor)
        {
            _context = vendor;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
        {
            var vendors = await _context.GetAllVendorsAsync(pageNumber, pageSize);
            var totalVendors = await _context.GetVendorCountAsync();
            var totalPages = (int)Math.Ceiling(totalVendors / (double)pageSize);

            var viewModels = vendors.Select(v => VendorMapper.ToViewModel(v)).ToList();
            ViewData["CurrentPage"] = pageNumber;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = totalPages; // Pass total pages to the view

            return View(viewModels);
        }

        [HttpGet]   
        public async Task<IActionResult> Details(int id)
        {
            var vendor = await _context.GetVendorByIdAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }
            var viewModel = VendorMapper.ToViewModel(vendor);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AddVendor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddVendor(string businessName, string vendorName, string email, string gstNo, string contactNo, string alternateNumber, string address, string state)
        {
            if (ModelState.IsValid)
            {
                var data = new TblVendor
                {
                    BusinessName = businessName,
                    VendorName = vendorName,
                    Email = email,
                    GstNo = gstNo,
                    ContactNo = contactNo,
                    AlternateNumber = alternateNumber,
                    Address = address,
                    State = state,
                    IsDelete = 0,
                    CreatedBy = vendorName,
                    CreatedAt = DateTime.Now
                };

                await _context.AddVendorAsync(data);
                return Json(new { Success = true, Message = "Vendor added successfully." });
            }
            return Json(new { Success = false, Message = "Invalid data." });
        }

        [HttpGet]
        public async Task<IActionResult> EditVendor(int id)
        {
            var vendor = await _context.GetVendorByIdAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            var viewModel = VendorMapper.ToViewModel(vendor);
            return Json(viewModel);
        }

        [HttpPut]
        public async Task<IActionResult> EditVendor(int id, VendorViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var vendor = VendorMapper.ToModel(viewModel);
                vendor.Id = viewModel.Id;
                vendor.VendorName = viewModel.VendorName;
                vendor.BusinessName = viewModel.BusinessName;
                vendor.Address = viewModel.Address;
                vendor.State = viewModel.State;
                vendor.IsDelete = 0;
                vendor.Email = viewModel.Email;
                vendor.ContactNo = viewModel.ContactNo;
                vendor.AlternateNumber = viewModel.AlternateNumber;

                await _context.UpdateVendorAsync(id,vendor);
                return Json(new { success = true,  }); // Return success as JSON
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) }); // Return errors if any
        }

        [HttpDelete]
        public IActionResult DeleteVendor(int id)
        {
            var vendor = _context.DeleteVendor(id);
            if (vendor != null)
            {
                _context.DeleteVendor(id);
                return Json(new
                {
                    success = true,
                    Message = "Delete Successfully",
                    id = id
                });

            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Error"
                });
            }
        }
    }
}
