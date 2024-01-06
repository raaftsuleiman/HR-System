using Microsoft.AspNetCore.Mvc;

namespace HR.Controllers
{
    public class LeaveRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetRequests()
        {
            return View();
        }
    }
}
