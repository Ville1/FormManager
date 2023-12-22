using FormManager.Data.HttpData.Request;
using FormManager.Data.HttpData.Response;
using FormManager.Data.Models.Forms;
using FormManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormManager.Controllers
{
    public class VideoGameController : ControllerBase
    {
        public VideoGameController(DatabaseService service) : base(service) { }

        /// <summary>
        /// List page
        /// </summary>
        /// <returns></returns>
        [Route("/VideoGame/Index")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/VideoGame/Search")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Search(SearchRequest search)
        {
            ListResponse<VideoGame> games = new ListResponse<VideoGame>();

            return Json(games);
        }
    }
}
