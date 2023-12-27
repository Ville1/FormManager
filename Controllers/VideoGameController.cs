using FormManager.Data;
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
        public ActionResult Search([FromBody]SearchRequest search)
        {
            //Validate parameters
            search ??= new SearchRequest();
            search.Validate();

            FilterContainer filters = search.ParseFilters(new List<Filter>() {
                new Filter(FilterType.Text, "name")
            });

            //Get results
            IEnumerable<VideoGame> games = DB.VideoGames.Search(filters.IsMatch);
            IEnumerable<VideoGame> paginatedResults = search.Paginate(games);

            return Json(new ListResponse<VideoGame>(paginatedResults, games.Count(), search.PageSize));
        }
    }
}
