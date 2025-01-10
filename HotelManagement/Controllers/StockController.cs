using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
