using HotelManagement.Data;
using HotelManagement.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Controllers
{
    public class HotelItemsController : Controller
    {
        private readonly LoginDbContext _context;

        public HotelItemsController(LoginDbContext context)
        {
            _context = context;
        }

        [HttpGet]
            public IActionResult Index(string hotelName)
            {
                var result = _context.HotelItemDetails
                    .FromSqlRaw("EXEC HotelItemsDetails1 @HotelName = {0}", hotelName)
                    .ToList();

                return View(result);
            }

        [HttpPost]
        public IActionResult Update([FromBody] Item updatedItem)
        {
            // Find the item in the database using the ID
            var item = _context.HotelItemDetails.Find(updatedItem.Id);
            if (item == null)
            {
                return NotFound();
            }

            // Update the item details
            item.ItemName = updatedItem.ItemName;
            item.Quantity = updatedItem.Quantity;
            item.UnitPrice = updatedItem.UnitPrice;
            item.TotalPrice = updatedItem.Quantity * updatedItem.UnitPrice;

            // Save changes
            _context.SaveChanges();

            return Ok(new { success = true, message = "Item updated successfully!" });
        }
        //update the hotel items

        //[HttpPost]
        //public IActionResult UpdateItem(int HotelId, string ItemName, int Quantity, decimal UnitPrice)
        //{
        //    var item = _context.Items.Find(HotelId);
        //    if (item != null)
        //    {
        //        item.ItemName = ItemName;
        //        item.Quantity = Quantity;
        //        item.UnitPrice = UnitPrice;
        //        _context.SaveChanges();
        //        return Json(new { success = true });
        //    }
        //    return Json(new { success = false });
        //}



    }
}
