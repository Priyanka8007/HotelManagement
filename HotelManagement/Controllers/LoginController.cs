using HotelManagement.Data;
using HotelManagement.Models.DTOs;
using HotelManagement.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace HotelManagement.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly LoginDbContext _context;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenRepository _tokenRepository;

        public LoginController(SignInManager<IdentityUser> signInManager,
                               UserManager<IdentityUser> userManager,
                               IConfiguration configuration,
                               ITokenRepository tokenRepository, LoginDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _tokenRepository = tokenRepository;
            _context = context;
        }
      
        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            ViewData["Title"] = "Register";
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDto registerdto)
        {
            Console.WriteLine("Role selected: " + registerdto.Roles);
            if (!ModelState.IsValid)
            {
                // If the model is invalid, return the view with the error messages
                return View(registerdto);
            }

            // Create a new IdentityUser
            var identityUser = new IdentityUser
            {
                UserName = registerdto.UserName,
                Email = registerdto.UserName  // Assuming the email is the same as the username
            };

            // Create the user with the provided password
            var identityResult = await _userManager.CreateAsync(identityUser, registerdto.Password);

            if (identityResult.Succeeded)
            {
                // If user creation succeeded, add roles if provided
                if (registerdto.Roles != null && registerdto.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerdto.Roles);
                  //  var roleResult = await _userManager.AddToRolesAsync(identityUser, registerdto.Roles);

                    if (identityResult.Succeeded)
                    {
                        TempData["Message"] = "User was registered! Please login.";
                        return RedirectToAction("Login", "Login"); // Redirect to the login page
                    }
                }
                else
                {
                    TempData["Error"] = "No roles assigned.";
                    return View(registerdto); // Return the view with the error message
                }
            }

            // If the user creation or role assignment failed, add errors to ModelState
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // If something went wrong, return the view with the errors
            TempData["Error"] = "Something went wrong. Please try again.";
            return View(registerdto);
        }


        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Title"] = "Login";  // Ensure the title is set
            return View();  // Return the Login View
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest model) // Use the same method name "Index"
        {
            if (!ModelState.IsValid)
            {
                return View(model);  // Return the login view if model is invalid
            }

            // Check if user exists
            var user = await _userManager.FindByEmailAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            // Check password
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            // Get roles for the user
            var roles = await _userManager.GetRolesAsync(user);
            // Retrieve hotel details based on username
            var hotel = _context.Hotels.FirstOrDefault(h => h.UserNamee == user.UserName);


            //// Store hotel info in session if hotel exists
            if (hotel != null)
            {
                HttpContext.Session.SetString("OwnerName", hotel.OwnerName);
                HttpContext.Session.SetString("HotelName", hotel.HotelName);
            }

            // Generate JWT Token if needed (this can be handled separately if you're using JWT)
            var token = _tokenRepository.CreateJWTToken(user, roles.ToList());

            // Check role and redirect accordingly
            if (roles.Contains("SuperAdmin"))
            {
                return RedirectToAction("Index", "Superadmin1");  // Redirect to SuperAdmin Dashboard
            }
            else if (roles.Contains("Admin"))
            {
                return RedirectToAction("Index", "Admin");  // Redirect to Admin Dashboard
            }
            else if (roles.Contains("User"))
            {
                return RedirectToAction("Index", "User");  // Redirect to User Dashboard
            }
            else if (roles.Contains("Kitchen"))
            {
                return RedirectToAction("Index", "Kitchen");  // Redirect to Kitchen Dashboard
            }

            // If no role matches, show error
            ModelState.AddModelError(string.Empty, "You are not authorized to access this page.");
            return View(model);  // Return the login view with an error message
        }
    }
}
