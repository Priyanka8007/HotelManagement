using HotelManagement.Data;
using HotelManagement.Models;
using HotelManagement.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Drawing.Printing;

namespace HotelManagement.Controllers
{
    public class SuperAdmin1Controller : Controller
    {

        private readonly LoginDbContext _context;

        public SuperAdmin1Controller(LoginDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        //[HttpGet]
        //public async Task<IActionResult> GetHotels()
        //{
        //    try
        //    {
        //        // Fetch data from the database
        //        var hotels = await _context.Hotels.ToListAsync();

        //        // Return the data as JSON
        //        return Json(new { success = true, data = hotels });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions and return an error response
        //        return Json(new { success = false, message = "An error occurred: " + ex.Message });
        //    }
        //}
        [HttpGet]
        public async Task<IActionResult> GetHotels(int page = 1, int pageSize = 3)
        {
            try
            {
                // Validate inputs
                if (page < 1 || pageSize < 1)
                {
                    return Json(new { success = false, message = "Invalid page or pageSize parameters." });
                }

                // Calculate starting and ending row numbers
                int startRow = (page - 1) * pageSize + 1;
                int endRow = page * pageSize;

                // Raw SQL query to use ROW_NUMBER() for pagination
                var query = @$"
            SELECT *
            FROM (
                SELECT 
                    ROW_NUMBER() OVER (ORDER BY Id ASC) AS RowNum, 
                    *
                FROM Hotels
            ) AS RowConstrainedResult
            WHERE RowNum BETWEEN {startRow} AND {endRow}
            ORDER BY RowNum";
              //  var hotels = await _context.Hotels.ToListAsync();

                // Execute raw SQL query
                var paginatedHotels = await _context.Hotels.FromSqlRaw(query).ToListAsync();

                // Fetch total hotel count
                var totalHotels = await _context.Hotels.CountAsync();
                var totalActiveHotels = _context.Hotels.Count(static h => h.IsActive);

                // Calculate total pages
                var totalPages = (int)Math.Ceiling((double)totalHotels / pageSize);

                // Return the data as JSON
                return Json(new
                {
                    success = true,
                    data = paginatedHotels,
                    totalHotels = totalHotels,
                    totalActiveHotels= totalActiveHotels,
                    totalPages = totalPages,
                    currentPage = page,
                    pageSize = pageSize
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }



        //[HttpGet]
        //public async Task<IActionResult> GetHotels()
        //{
        //    int page = 1;
        //    const int pageSize = 3; // Number of records to display per page
        //    try
        //    {
        //        // Fetch data from the database
        //        var hotels = await _context.Hotels.ToListAsync();

        //        //total hotel count
        //        var totalHotels = await _context.Hotels.CountAsync();


        //        var paginatedhotels = hotels.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //        // Return the data as JSON
        //        return Json(new { success = true, data = paginatedhotels });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions and return an error response
        //        return Json(new { success = false, message = "An error occurred: " + ex.Message });
        //    }
        //}



        [HttpGet]
        public async Task<IActionResult> GetHotelById(int id)
        {
            // Find the hotel record by its ID.
            var hotel = await _context.Hotels
                                      .Where(h => h.Id == id)
                                      .Select(h => new
                                      {
                                          h.Id,
                                          h.HotelName,
                                          h.OwnerName,
                                          h.MobileNo,
                                          h.Address,
                                          h.GSTNumber,
                                          h.LicenseNumber,
                                          h.IsActive,
                                          RenewDate = h.RenewDate.HasValue ? h.RenewDate.Value.ToString("yyyy-MM-dd") : null
                                      })
                                      .FirstOrDefaultAsync();

            // If no record is found, return a 404 response.
            if (hotel == null)
            {
                return NotFound();
            }

            // Return the hotel data as JSON.
            return Json(hotel);
        }

        [HttpPut]
        public async Task<IActionResult> EditHotel([FromBody] Hotel hotelDto)
        {
            //if (hotelDto == null || !ModelState.IsValid)
            //{
            //    return BadRequest("Invalid data.");
            //}

            var existingHotel = await _context.Hotels.FindAsync(hotelDto.Id);
            if (existingHotel == null)
            {
                return NotFound("Hotel not found.");
            }

            // Update hotel fields
            existingHotel.HotelName = hotelDto.HotelName;
            existingHotel.OwnerName = hotelDto.OwnerName;
            existingHotel.MobileNo = hotelDto.MobileNo;
            existingHotel.Address = hotelDto.Address;
            existingHotel.GSTNumber = hotelDto.GSTNumber;
            existingHotel.LicenseNumber = hotelDto.LicenseNumber;
            existingHotel.RenewDate = hotelDto.RenewDate;
            existingHotel.IsActive = hotelDto.IsActive;

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Hotel updated successfully." });
        }

        // Controller Action for Cancel (Delete) Hotel
        [HttpDelete]
        public IActionResult CancelHotel(int hotelId)
        {
            try
            {
                var hotel = _context.Hotels.Find(hotelId); // Assuming you're using Entity Framework

                if (hotel == null)
                {
                    return Json(new { success = false, message = "Hotel not found" });
                }

                _context.Hotels.Remove(hotel);
                _context.SaveChanges();

                return Json(new { success = true, message = "Hotel deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        //[HttpPost]
        //public async Task<IActionResult> AddHotel([FromBody] Hotel hoteldto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // Add the hotel to the database
        //            _context.Hotels.Add(hoteldto);
        //            await _context.SaveChangesAsync();

        //            // Return a success response
        //            //return Json(new { success = true, message = "Hotel added successfully!" });
        //            return Json(new
        //            {
        //                success = true,
        //                data = hoteldto,
        //                message = "Hotel added successfully!",
        //                // Return the inserted hotel data
        //            });
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle exceptions and return an error response
        //            return Json(new { success = false, message = "An error occurred: " + ex.Message });
        //        }
        //    }

        //    // Return validation error messages if the model state is invalid
        //    return Json(new { success = false, message = "Invalid data", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        //}


        [HttpPost]
        public async Task<IActionResult> AddHotel([FromBody] Hotel hoteldto)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Json(new { success = false, message = "Invalid data", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            //}

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Step 1: Generate Username and Password
                string username = hoteldto.HotelName.Replace(" ", "").ToLower() + "@hotel.com";
                string password = "Hotel@" + new Random().Next(1000, 9999); // Example: Hotel@1234

                // Step 2: Hash the Password
                var passwordHasher = new PasswordHasher<IdentityUser>();
                var hashedPassword = passwordHasher.HashPassword(null, password);

                // Step 3: Insert Hotel Data (Including Username and Password)
                hoteldto.UserNamee = username;
               // hoteldto.Password1 = hashedPassword; // Store the hashed password in the hotel table
                _context.Hotels.Add(hoteldto);
                await _context.SaveChangesAsync();

                // Step 4: Create a New User in AspNetUsers
                var user = new IdentityUser
                {
                    Id = Guid.NewGuid().ToString(), // Generate unique ID for the user
                    UserName = username,
                    NormalizedUserName = username.ToUpper(),
                    Email = username,
                    NormalizedEmail = username.ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = hashedPassword,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Step 5: Assign the 'Admin' Role to the User
                var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
                if (adminRole == null)
                {
                    // Create the Admin role if it doesn't exist
                    var newAdminRole = new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    };
                    _context.Roles.Add(newAdminRole);
                    await _context.SaveChangesAsync();
                    adminRole = newAdminRole;
                }

                var userRole = new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = adminRole.Id
                };
                _context.UserRoles.Add(userRole);
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();

                // Return success response with entire hoteldto and password
                return Json(new
                {
                    success = true,
                    message = "Hotel added successfully! Admin user created.",
                    username = username,
                    password = password,
                    data = hoteldto
                });
            }
            catch (Exception ex)
            {
                // Rollback transaction in case of failure
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }







    }
}
