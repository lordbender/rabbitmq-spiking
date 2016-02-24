using System.Web.Mvc;

namespace RabbitMqSpike.ReceiverAOS.Controllers
{
    public class HomeController : Controller
    {
        public JsonResult Index()
        {
            ViewBag.Title = "Home Page";

            return Json(new
            {
                Status = "Running"
            }, JsonRequestBehavior.AllowGet);
        }
    }
}