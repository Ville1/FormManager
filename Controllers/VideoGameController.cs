using FormManager.Data;
using FormManager.Data.HttpData.Request;
using FormManager.Data.HttpData.Response;
using FormManager.Data.Models.Forms;
using FormManager.Services;
using FormManager.Utils;
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

        /// <summary>
        /// Form page
        /// </summary>
        /// <returns></returns>
        [Route("/VideoGame/Form")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// Search video games
        /// </summary>
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

            return Json(new ListResponse<VideoGameResponseDataShort>(paginatedResults.Select(x => x.ToResponseShort()), games.Count(), search.PageSize));
        }

        /// <summary>
        /// Get video game data
        /// </summary>
        [HttpGet]
        [Route("/VideoGame")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Get(Guid id)
        {
            if (!DB.VideoGames.Has(id)) {
                //Video game with this id does not exist
                return StatusCode(404);
            }
            return Json(DB.VideoGames.Get(id).ToResponse());
        }

        /// <summary>
        /// Save video game
        /// </summary>
        [HttpPost]
        [Route("/VideoGame")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Save([FromBody] VideoGameRequestData data)
        {
            //Validate
            ValidationErrorResponse validationErrors = Validate(data);
            if(validationErrors.Errors.Count != 0) {
                //Validation failed
                return ErrorResult(validationErrors);
            }

            VideoGame newData = VideoGame.FromRequestData(data);
            if (data.Id == Guid.Empty) {
                //New video game
                data.Id = DB.VideoGames.Add(newData);
            } else {
                //Modify a pre-existing video game
                //Get old data
                if (!DB.VideoGames.Has(data.Id)) {
                    //Video game with this id does not exist
                    return StatusCode(404);
                }
                VideoGame oldData = DB.VideoGames.Get(data.Id);
                oldData.Name = newData.Name;
                DB.SaveChanges();
            }

            return Json(data.Id);
        }

        private ValidationErrorResponse Validate(VideoGameRequestData data)
        {
            FormValidator validator = new FormValidator(data);

            //Name
            validator.IsRequired<string>("Name");

            return validator.Response;
        }
    }
}
