using System.Web.Mvc;

namespace ThePortal.Controllers
{
    public partial class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            ViewBag.Message = "job portal";

            return View();
        }

        public virtual ActionResult About()
        {
            ViewBag.Message = "the portal.";

            return View();
        }

        public virtual ActionResult Contact()
        {
            ViewBag.Message = "Contact us on";

            return View();
        }
    }
}
