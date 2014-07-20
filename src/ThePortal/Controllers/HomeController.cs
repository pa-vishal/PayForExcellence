using System.Web.Mvc;

namespace ThePortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "job portal";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "the portal.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact us on";

            return View();
        }
    }
}
