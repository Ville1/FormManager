using FormManager.Data.Models;
using FormManager.Data.Models.Log;
using FormManager.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FormManager.Controllers
{
    public class LoginController : ControllerBase
    {
        public LoginController(DatabaseService service) : base(service) { }

        /// <summary>
        /// Login page
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("/Login")]
        public ActionResult Index()
        {
            if (!IsAnonymous) {
                //Already logged in
                return Redirect("/Main");
            }
            return View();
        }

        /// <summary>
        /// Logout current user
        /// </summary>
        /// <returns></returns>
        [Route("/Logout")]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/Login");
        }

        [AllowAnonymous]
        [Route("/Login/Authenticate")]
        public ActionResult Login(string email, string password)
        {
            if (!DB.Users.CanLogIn(email, password)) {
                //Invalid credentials
                return StatusCode(401);
            }

            //Log user in
            User user = DB.Users.GetLoginUser(email, password);
            HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() {
                new Claim("UserId", user.Id.ToString())
            }, "Custom"))).Wait();
            DB.Log.Write(new LoginEvent(user.Id));

            return StatusCode(200);
        }

        /*
        // GET: LoginController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try {
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try {
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try {
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }*/
    }
}
