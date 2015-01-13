using System.Web.Mvc;
using FileService;

namespace AllocatorShare2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

       

        public ActionResult Auth()
        {
            return View("Index");
        }

        /// <summary>
        /// This is a shared reource for the Global Navigation bar.  It loads the current User
        /// To display information like UserName.  Will display information like a list of Plans
        /// and other details specific to User.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Header()
        {
            return PartialView("_Header");
        }

        /// <summary>
        /// This is a shared resource for displaying the footer. There are elements of the footer
        /// that should only display when someone is logged in, thus it needs the current user.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Footer()
        {
            return PartialView("_Footer");
        }
    }
}
