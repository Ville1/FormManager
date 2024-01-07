using FormManager.Data.HttpData.Request;
using FormManager.Data.HttpData.Response;
using FormManager.Data.Models.Forms;
using FormManager.Data;
using FormManager.Services;
using FormManager.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormManager.Controllers
{
    public class PublisherController : ControllerBase
    {
        public PublisherController(DatabaseService service) : base(service) { }

        /// <summary>
        /// List page
        /// </summary>
        /// <returns></returns>
        [Route("/Publisher/Index")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Form page
        /// </summary>
        /// <returns></returns>
        [Route("/Publisher/Form")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// Search publishers
        /// </summary>
        [HttpPost]
        [Route("/Publisher/Search")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Search([FromBody] SearchRequest search)
        {
            //Validate parameters
            search ??= new SearchRequest();
            search.Validate();

            FilterContainer filters = search.ParseFilters(new List<Filter>() {
                new Filter(FilterType.Text, "name")
            });

            //Get results
            IEnumerable<Publisher> publishers = DB.Publishers.Search(filters.IsMatch);
            IEnumerable<Publisher> paginatedResults = search.Paginate(publishers);

            return Json(new ListResponse<PublisherResponseData>(paginatedResults.Select(x => x.ToResponse()), publishers.Count(), search.PageSize));
        }

        /// <summary>
        /// Get publisher data
        /// </summary>
        [HttpGet]
        [Route("/Publisher")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Get(Guid id)
        {
            if (!DB.Publishers.Has(id)) {
                //Publisher with this id does not exist
                return StatusCode(404);
            }
            return Json(DB.Publishers.Get(id).ToResponse());
        }

        /// <summary>
        /// Save publisher
        /// </summary>
        [HttpPost]
        [Route("/Publisher")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Save([FromBody] PublisherRequestData data)
        {
            //Validate
            ValidationErrorResponse validationErrors = Validate(data);
            if (validationErrors.Errors.Count != 0) {
                //Validation failed
                return ErrorResult(validationErrors);
            }

            Publisher newData = Publisher.FromRequestData(data);
            if (data.Id == Guid.Empty) {
                //New publisher
                data.Id = DB.Publishers.Add(newData);
            } else {
                //Modify a pre-existing publisher
                //Get old data
                if (!DB.Publishers.Has(data.Id)) {
                    //Publisher with this id does not exist
                    return StatusCode(404);
                }
                Publisher oldData = DB.Publishers.Get(data.Id);

                oldData.Name = newData.Name;

                DB.SaveChanges();
            }

            return Json(data.Id);
        }

        private ValidationErrorResponse Validate(PublisherRequestData data)
        {
            FormValidator validator = new FormValidator(data);

            //Name
            validator.IsRequired<string>("Name");

            return validator.Response;
        }
    }
}
