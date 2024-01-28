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
            SetViewBagDropdownValues();
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
            SetViewBagDropdownValues();
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

            //Parse request data
            VideoGame newData = VideoGame.FromRequestData(data);

            if (data.Id == Guid.Empty) {
                //New video game
                data.Id = DB.VideoGames.Add(newData);
            } else {
                //Modify a pre-existing video game
                if (!DB.VideoGames.Save(newData, new List<string>() { "Name", "DeveloperId", "PublisherId" })) {
                    //Video game with this id does not exist
                    return StatusCode(404);
                }
            }

            return Json(data.Id);
        }

        /// <summary>
        /// Check if video game can be deleted
        /// </summary>
        [HttpGet]
        [Route("/VideoGame/DeleteCheck")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult DeleteCheck(Guid id)
        {
            if (!DB.VideoGames.Has(id)) {
                //Video game with this id does not exist
                return StatusCode(410);
            }
            VideoGame videoGame = DB.VideoGames.Get(id);
            List<string> deleteErrors;
            if (!videoGame.CanBeDeleted(out deleteErrors)) {
                //Can't be deleted
                return Json(new DeleteErrorsResponse() {
                    Errors = deleteErrors
                });
            }
            return Json(new DeleteErrorsResponse());
        }

        /// <summary>
        /// Delete video game
        /// </summary>
        [HttpDelete]
        [Route("/VideoGame")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Delete(Guid id)
        {
            if (!DB.VideoGames.Has(id)) {
                //Video game with this id does not exist
                return StatusCode(404);
            }

            //Get video game data
            VideoGame videoGame = DB.VideoGames.Get(id);

            //Check if it can be deleted
            List<string> deleteErrors;
            if(!videoGame.CanBeDeleted(out deleteErrors)) {
                //Can't be deleted
                return ErrorResult(new DeleteErrorsResponse() {
                    Errors = deleteErrors
                });
            }

            //Delete
            DB.VideoGames.Delete(videoGame);

            return StatusCode(200);
        }

        private ValidationErrorResponse Validate(VideoGameRequestData data)
        {
            FormValidator validator = new FormValidator(data);

            //Name
            validator.IsRequired<string>("Name");

            //Developer
            //TODO: Fix this, so Select(x => (Guid?)x) is not required?
            validator.InList("DeveloperId", DB.Developers.GetAllIds().Select(x => (Guid?)x));

            //Publisher
            validator.InList("PublisherId", DB.Publishers.GetAllIds().Select(x => (Guid?)x));

            return validator.Response;
        }

        private void SetViewBagDropdownValues()
        {
            ViewBag.DevelopersJson = string.Format("[{0}]", string.Join(", ", DB.Developers.GetAll().OrderBy(developer => developer.Name)
                .Select(developer => { return string.Format("{{ id: '{0}', text: '{1}' }}", developer.Id, developer.Name); })));
            ViewBag.PublishersJson = string.Format("[{0}]", string.Join(", ", DB.Publishers.GetAll().OrderBy(publisher => publisher.Name)
                .Select(publisher => { return string.Format("{{ id: '{0}', text: '{1}' }}", publisher.Id, publisher.Name); })));
        }
    }
}
