using Raven.Client;
using RecipesMVC4.Models;
using System.Linq;
using System.Web.Mvc;


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
            if (_documentSession.Query<User>().SingleOrDefault(userQuery => userQuery.Username == user.Username && userQuery.Password == user.Password) != null)
            {
                user.IsLoggedIn = true;
                _documentSession.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            return View();
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
                ViewBag.ErrorMessage = string.Format("Username {0} already exists. Please choose another one! ", user.Username);
                return View();
            }
            user.ID = _documentSession.Query<User>().Count() + 1;
            _documentSession.Store(user);
            _documentSession.SaveChanges();
            TempData["Message"] = string.Format("New user: {0}", user.FullName);

            return Redirect("/Home");
        }

    }
}