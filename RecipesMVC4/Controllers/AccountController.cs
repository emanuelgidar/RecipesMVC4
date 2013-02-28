using Raven.Client;
using RecipesMVC4.Models;
using System.Linq;
using System.Web.Mvc;
using System;
using System.Web.Security;


namespace RecipesMVC4.Controllers
{
    public class AccountController : BootstrapBaseController
    {
        private readonly IDocumentSession _documentSession;
        public AccountController(IDocumentSession documentSession)
        {
                _documentSession = documentSession;
        }

        public ActionResult Index()
        {
            return View(_documentSession.Query<User>().ToList());
        }

        public ActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SignIn(User user)
        {
            if (ValidateUser(user.Username,user.Password))
            {
                return RedirectToAction("Index", "Admin");
            }
            TempData["Error"] = string.Format("Invalid username or password !");
            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            TempData["Success"] = string.Format("Signed-out successfully !");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        // POST: /Home/Create
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (user == null)
                return View();

            if (_documentSession.Query<User>().FirstOrDefault(users => users.Username == user.Username) != null)
            {
                TempData["Error"] = string.Format("Username {0} already exists. Please choose another one! ", user.Username);
                return View();
            }

            user.ID = Guid.NewGuid();
            
            _documentSession.Store(user);
            _documentSession.SaveChanges();
            TempData["Success"] = string.Format("New user: {0}", user.FullName);

            return Redirect("/Home");
        }

        private bool ValidateUser(string username, string password)
        {
            var user = _documentSession.Query<User>().SingleOrDefault( userQuery => userQuery.Username == username && userQuery.Password == password);
            if( user != null)
            {
                _documentSession.SaveChanges();
                FormsAuthentication.SetAuthCookie(user.Username, false);
                return true;
            }
            return false;
        }
    }
}