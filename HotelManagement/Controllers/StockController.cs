using Microsoft.AspNetCore.Mvc;
using HotelManagement.Models;
using HotelManagement.Data;
using HotelManagement.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Controllers
{
    public class StockController : Controller
    {
        private readonly LoginDbContext _context;

        public StockController(LoginDbContext context)
        {
            _context = context;
        }

        // GET: Stock/AddStockItem
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: Stock/AddStockItem
        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public IActionResult AddStockItem(Stocknew stock)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //        _context.Stocknews.Add(stock);
        //        _context.SaveChanges();
        //        TempData["SuccessMessage"] = "Stock item added successfully!";
        //       // return RedirectToAction("Index"); // Redirect to the stock list or another page
        //    return RedirectToAction("Index", "Admin");
        //    // }

        //    //return View(stock); // Return to the form if validation fails
        //}


        [HttpPost]
        public async Task<IActionResult> SaveStock([FromBody] Item items)
        {           
                try

                {
                     _context.Items.Add(items);
                   // _context.items.Add(Item);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Stock added successfully." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
       
        }
       



    }
}
