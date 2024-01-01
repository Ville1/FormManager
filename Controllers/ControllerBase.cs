using FormManager.Data.HttpData.Response;
using FormManager.Data.Models;
using FormManager.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FormManager.Controllers
{
    public class ControllerBase : Controller
    {
        public readonly DatabaseService DB;

        public ControllerBase(DatabaseService service)
        {
            DB = service;
        }

        public bool IsAnonymous
        {
            get {
                return HttpContext.User.Identity == null || !HttpContext.User.Identity.IsAuthenticated || !(HttpContext.User.Identity is ClaimsIdentity) ||
                    !((ClaimsIdentity)HttpContext.User.Identity).Claims.Any(claim => claim.Type == "UserId");
            }
        }

        new public User User
        {
            get {
                if(IsAnonymous) {
                    //No user logged in
                    throw new Exception("Can't access User, not authenticated");
                }

                //Get user id from claims      v this is never null because IsAnonymous == true
                Guid id = HttpContext.User.Identity == null ? Guid.Empty : Guid.Parse(((ClaimsIdentity)HttpContext.User.Identity).Claims.First(claim => claim.Type == "UserId").Value);

                //Get user data from database
                User? user = DB.Users.Get(id);
                if(user == null) {
                    //User has been deleted after login
                    throw new NotImplementedException("User has been deleted after login. TODO: Logout user");
                }
                return user;
            }
        }

        protected ActionResult ErrorResult(ValidationErrorResponse errors, int statusCode = 403)
        {
            Response.StatusCode = statusCode;
            return Json(errors);
        }
    }

    public class AuthPolicy
    {
        public const string Basic = "Basic";
    }
}
