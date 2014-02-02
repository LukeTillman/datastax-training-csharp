using System;
using System.Web.Mvc;
using Playlist.ActionFilters;
using Playlist.Data;
using Playlist.Data.Dtos;
using Playlist.Models.Login;

namespace Playlist.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsersDao _usersDao;
        private readonly IStatisticsDao _statsDao;

        public LoginController(IUsersDao usersDao, IStatisticsDao statsDao)
        {
            if (usersDao == null) throw new ArgumentNullException("usersDao");
            _usersDao = usersDao;
            _statsDao = statsDao;
        }

        /// <summary>
        /// Shows the Login screen.
        /// </summary>
        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult Index()
        {
            // Check if we were redirected because user was not logged in and add error message if so
            if (TempData.ContainsKey(RequiresLoggedInUserAttribute.TempDataKey))
                ModelState.AddModelError("", "Not logged in.");

            _statsDao.IncrementCounter("page hits: login");

            return View(new LoginModel());
        }

        /// <summary>
        /// Handles submit of the Login form and tries to log the user in or create the new user.
        /// </summary>
        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult LoginSubmit(LoginModel model)
        {
            // We do both login and create from this form, so look for the button that was pressed and call the appropriate action
            if (model.Button == "login")
                return DoLogin(model);

            if (model.Button == "newAccount")
                return CreateUser(model);

            return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Tries to log the user in.
        /// </summary>
        private ActionResult DoLogin(LoginModel model)
        {
            if (ModelState.IsValid == false)
                return RedirectToAction("Index", "Login");

            // Do some manual validation
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("", "Username and Password cannot be blank.");
                return RedirectToAction("Index", "Login");
            }

            UserDto user = _usersDao.ValidateLogin(model.Username, model.Password);
            if (user == null)
            {
                _statsDao.IncrementCounter("failed login attempts");

                // Return to the login screen with an error
                ModelState.AddModelError("", "Username or Password is invalid.");
                return RedirectToAction("Index", "Login");
            }

            // In a real app, you wouldn't use Session, but this is just an example app, so cheat and use session
            Session["user"] = user;
            _statsDao.IncrementCounter("valid login attempts");
            return RedirectToAction("Index", "Playlists");
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        private ActionResult CreateUser(LoginModel model)
        {
            if (ModelState.IsValid == false)
                return RedirectToAction("Index", "Login");

            // Do some manual validation
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("", "Username and Password cannot be blank.");
                return RedirectToAction("Index", "Login");
            }

            UserDto user = _usersDao.AddUser(model.Username, model.Password);
            if (user == null)
            {
                // Go back to the user screen with an error
                ModelState.AddModelError("", "User Already Exists.");
                return RedirectToAction("Index", "Login");
            }

            // In a real app, you wouldn't use Session, but this is just an example app, so cheat and use session
            Session["user"] = user;
            _statsDao.IncrementCounter("users");
            return RedirectToAction("Index", "Playlists");
        }

        /// <summary>
        /// Logs the current user out and redirects to the login page.
        /// </summary>
        [HttpGet]
        public ActionResult Logout()
        {
            // Just clear the user object in session
            Session.Remove("user");
            return RedirectToAction("Index", "Login");
        }
    }
}
