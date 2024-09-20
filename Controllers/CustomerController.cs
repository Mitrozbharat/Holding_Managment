
using Microsoft.EntityFrameworkCore;

namespace Hoarding_managment.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomer _context;

        public CustomerController(ICustomer customer)
        {
            _context = customer;
        }



        [HttpGet]

        public async Task<IActionResult> Index(int pageSize = 5, int pageNumber = 1)
        {
            var customers = await _context.GetallCustomerAsync(pageNumber, pageSize);
            var totalVendors = await _context.GetCustomerCountAsync();
            var totalPages = (int)Math.Ceiling(totalVendors / (double)pageSize);

            var viewModels = customers.Select(v => CustomerMapper.ToCustomerViewModel(v)).ToList();
            ViewData["CurrentPage"] = pageNumber;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = totalPages; // Pass total pages to the view

            return View(viewModels);

        }


        [HttpGet]
        public IActionResult AddNewCustomer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddNewCustomer(string businessName, string customerName, string email, string gstn, string contactNumber, string alternateNumber, string address, string state)
        {
            if (ModelState.IsValid)
            {
                var data = new TblCustomer
                {
                    BusinessName = businessName,
                    CustomerName = customerName,
                    Email = email,
                    GstNo = gstn,
                    ContactNo = contactNumber,
                    AlternateNumber = alternateNumber,
                    Address = address,
                    State = state,
                    IsDelete = 0,
                    CreatedAt = DateTime.Now,
                    CreatedBy = customerName,
                };

                await _context.AddNewCustomerasAsync(data);
                return Ok(new { Success = true, Message = "Customer  Add Successfully. " });
            }
            return Json(new { Success = false, Message = "Invalid data." });

        }

        [HttpPut]
        [HttpPut]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerViewModel model)
        {
            if (model == null)
            {
                return Json(new { success = false, Message = "Invalid data." });
            }

            var customer = await _context.GetCustomerById(model.Id); // Ensure this is async
            if (customer == null)
            {
                return Json(new { success = false, Message = "Customer not found." });
            }

            // Update customer properties
            customer.BusinessName = model.BusinessName;
            customer.CustomerName = model.CustomerName;
            customer.Email = model.Email;
            customer.GstNo = model.GstNo;
            customer.ContactNo = model.ContactNo;
            customer.AlternateNumber = model.AlternateNumber;
            customer.Address = model.Address;
            customer.State = model.State;
            customer.UpdatedAt = DateTime.Now;

            await _context.UpdateCustomer(customer); // Ensure the update method is async

            return Json(new { success = true, Message = "Customer updated successfully.", model = customer });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = _context.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            try
            {
                await _context.DeleteCustomer(id);
                return Json(new
                {
                    success = true,
                    Message = "Delete Successfully",
                    id = id
                });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }


        [HttpGet("GetDetails")]
        public IActionResult Details(int id)
        {
            var customer = _context.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);

        }


        public IActionResult GetCustomerinfoById(int id)
        {
            var customer = _context.GetCustomerById(id); // Assuming _context is your database context
            if (customer == null)
            {
                return Json(new { success = false, message = "Customer not found." });
            }

            return Json(new { success = true, message = "Successfully.", model = customer });
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerinfo()
        {
            var customer = await _context.GetCustomerinfo();

            if (customer != null)
            {
                return Json(new { success = true, Message = "Successfully retrieved Customer.", model = customer });
            }
            return Json(new { success = false, Message = "Customer not found." });
        }



        [HttpGet]
        public async Task<IActionResult> GetCountCustomer()
        {
            try
            {

                int itemCount = await _context.CustomerCountAsync();

                return Json(new { success = true, count = itemCount });
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                return Json(new { success = false, message = "An error occurred while fetching the count." });
            }
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchCustomersByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Name cannot be empty.");
            }

            var customers = await _context.SearchCustomersByNameAsync(name);
            return Ok(customers);
        }
    }
}