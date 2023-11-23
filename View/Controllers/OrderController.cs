using Microsoft.AspNetCore.Mvc;

namespace View.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult AddProductToCart()
        {

            return PartialView("_Cart");
        }
    }
}
