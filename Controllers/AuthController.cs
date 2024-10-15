using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Hoarding_management.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuth _auth;
        private readonly db_hoarding_managementContext _context;
        private readonly ILogger<AuthController> _logger;

        private const string SessionUserIdKey="Id";

        public AuthController(IAuth auth, db_hoarding_managementContext context, ILogger<AuthController> logger)
        {
            _auth = auth;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the user exists
                var user = await _context.TblUsers
                    .FirstOrDefaultAsync(x => x.UserName.ToLower() == model.Username.ToLower() && x.IsDelete == 0);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Username is incorrect");
                }
                else
                {
                    // Validate the password
                    if (user.Password.ToLower() == model.Password.ToLower())
                    {
                        // Store user ID in session
                        HttpContext.Session.SetInt32(SessionUserIdKey, user.Id);
                        HttpContext.Session.SetString("SessionUsername", user.UserName);

                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Password is incorrect");
                    }
                }

                // Log warning if login attempt failed
                _logger.LogWarning("Login attempt failed for username: {Username}", model.Username);
            }

            // If we got this far, something failed; redisplay form
            return View(model);
        }



        public async Task<IActionResult> LogOut()
        {
            // Clear the user session
            HttpContext.Session.Remove("Id");
            HttpContext.Session.Remove("SessionUsername");
            // Optionally, you can sign out from any authentication provider here if applicable

            return RedirectToAction("Index", "Auth");
        }

    }
}
