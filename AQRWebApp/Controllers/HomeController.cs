using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AQRWebApp.BL;

namespace AQRWebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetListClients()
        {
            ClientService clientService = new ClientService();

            return Json(clientService.GetAllExposeDto().OrderBy(m => m.ClientName), JsonRequestBehavior.AllowGet);
        }
    }
}