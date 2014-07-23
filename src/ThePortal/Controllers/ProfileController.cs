using System.Web.Mvc;

namespace ThePortal.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ActionResult Dashboard()
        {
            return View("Index");
        }

    }
}
