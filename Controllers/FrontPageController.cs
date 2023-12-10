using FormManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormManager.Controllers
{
    public class FrontPageController : ControllerBase
    {
        public FrontPageController(DatabaseService service) : base(service) { }

        /// <summary>
        /// Front page
        /// </summary>
        /// <returns></returns>
        [Route("/Main")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Index()
        {
            return View();
        }
    }
}
