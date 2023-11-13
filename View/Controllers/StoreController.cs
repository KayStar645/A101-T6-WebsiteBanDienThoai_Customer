using Microsoft.AspNetCore.Mvc;

namespace View.Controllers
{
    public class StoreController : Controller
    {
        private readonly ILogger<StoreController> _logger;

        public StoreController(ILogger<StoreController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return PartialView("_Layout");
        }

        public IActionResult Home()
        {
            return PartialView("_Home");
        }
    }
}