﻿using FormManager.Data.HttpData.Request;
using FormManager.Data.HttpData.Response;
using FormManager.Data;
using FormManager.Services;
using FormManager.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FormManager.Data.Models.Forms;

namespace FormManager.Controllers
{
    public class DeveloperController : ControllerBase
    {
        public DeveloperController(DatabaseService service) : base(service) { }

        /// <summary>
        /// List page
        /// </summary>
        /// <returns></returns>
        [Route("/Developer/Index")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Form page
        /// </summary>
        /// <returns></returns>
        [Route("/Developer/Form")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Form()
        {
            SetViewBagCurrentUser();
            return View();
        }

        /// <summary>
        /// Search developers
        /// </summary>
        [HttpPost]
        [Route("/Developer/Search")]
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
            IEnumerable<Developer> developers = DB.Developers.Search(filters.IsMatch);
            List<Developer> paginatedResults = search.Paginate(developers);

            return Json(new ListResponse<DeveloperResponseData>(paginatedResults.Select(x => x.ToResponse(DB)), developers.Count(), search.PageSize));
        }

        /// <summary>
        /// Get developer data
        /// </summary>
        [HttpGet]
        [Route("/Developer")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Get(Guid id)
        {
            if (!DB.Developers.Has(id)) {
                //Developer with this id does not exist
                return StatusCode(404);
            }
            return Json(DB.Developers.Get(id).ToResponse(DB));
        }

        /// <summary>
        /// Save developer
        /// </summary>
        [HttpPost]
        [Route("/Developer")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Save([FromBody] DeveloperRequestData data)
        {
            //Validate
            ValidationErrorResponse validationErrors = Validate(data);
            if (validationErrors.Errors.Count != 0) {
                //Validation failed
                return ErrorResult(validationErrors);
            }

            Developer newData = Developer.FromRequestData(data);
            if (data.Id == Guid.Empty) {
                //New developer
                data.Id = DB.Developers.Add(newData);
            } else {
                //Modify a pre-existing developer
                //Get old data
                if (!DB.Developers.Has(data.Id)) {
                    //Developer with this id does not exist
                    return StatusCode(404);
                }
                Developer oldData = DB.Developers.Get(data.Id);

                oldData.Name = newData.Name;

                DB.SaveChanges();
            }

            return Json(data.Id);
        }

        /// <summary>
        /// Check if developer can be deleted
        /// </summary>
        [HttpGet]
        [Route("/Developer/DeleteCheck")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult DeleteCheck(Guid id)
        {
            if (!DB.Developers.Has(id)) {
                //Developer with this id does not exist
                return StatusCode(410);
            }
            Developer developer = DB.Developers.Get(id);
            List<string> deleteErrors;
            if (!developer.CanBeDeleted(out deleteErrors)) {
                //Can't be deleted
                return Json(new DeleteErrorsResponse() {
                    Errors = deleteErrors
                });
            }
            return Json(new DeleteErrorsResponse());
        }

        /// <summary>
        /// Delete developer
        /// </summary>
        [HttpDelete]
        [Route("/Developer")]
        [Authorize(AuthPolicy.Basic)]
        public ActionResult Delete(Guid id)
        {
            if (!DB.Developers.Has(id)) {
                //Developer with this id does not exist
                return StatusCode(404);
            }

            //Get developer data
            Developer developer = DB.Developers.Get(id);

            //Check if it can be deleted
            List<string> deleteErrors;
            if (!developer.CanBeDeleted(out deleteErrors)) {
                //Can't be deleted
                return ErrorResult(new DeleteErrorsResponse() {
                    Errors = deleteErrors
                });
            }

            //Delete
            DB.Developers.Delete(developer);

            return StatusCode(200);
        }

        private ValidationErrorResponse Validate(DeveloperRequestData data)
        {
            FormValidator validator = new FormValidator(data);

            //Name
            validator.IsRequired<string>("Name");

            return validator.Response;
        }
    }
}
