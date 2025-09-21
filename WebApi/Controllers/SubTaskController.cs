using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class SubTaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
