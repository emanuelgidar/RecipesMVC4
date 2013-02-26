using Raven.Client;
using RecipesMVC4.Models;
using System.Linq;
using System.Web.Mvc;

namespace RecipesMVC4.Controllers
{
    public class AdminController : BootstrapBaseController
    {
        private readonly IDocumentSession _documentSession;
      
        public AdminController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        //
        // GET: /Home/
        //[ActionName("Index")]
        public ActionResult Index()
        {
            return View(_documentSession.Query<Recipe>().OrderBy(x => x.ID).ToList());
        }

        public ActionResult Details(int id)
        {
            var recipe = _documentSession.Query<Recipe>().SingleOrDefault(recipeQuery => recipeQuery.ID == id);
            if (recipe == null)
            {
                TempData["Message"] = string.Format("Recipe {0} not found", id);
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: /Home/Create
        [HttpPost]
        public ActionResult Create(Recipe recipe)
        {
            //if (!ModelState.IsValid)
            //    return View();
            recipe.ID = _documentSession.Query<Recipe>().Count() + 1;
            _documentSession.Store(recipe);

            TempData["Message"] = string.Format("Created Recipe {0}", recipe.Title);

            return RedirectToAction("Index");
        }

        // GET: /Home/Edit/5
        public ActionResult Edit(int id)
        {
            var recipe = _documentSession.Query<Recipe>().SingleOrDefault(recipeQuery => recipeQuery.ID == id);
            if (recipe == null)
            {
                TempData["Message"] = string.Format("Recipe {0} not found", id);
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        // POST: /Home/Edit/
        [HttpPost]
        public ActionResult Edit(Recipe recipe)
        {
            var doc = _documentSession.Query<Recipe>().SingleOrDefault(recipeQuery => recipeQuery.ID == recipe.ID);
            doc.Title = recipe.Title;
            doc.Description = recipe.Description;
            doc.isIncorrect = recipe.isIncorrect;
            _documentSession.SaveChanges();
            TempData["Message"] = string.Format("Saved changes to Movie {0}", recipe.Title);

            return RedirectToAction("Index");
        }

        // GET: /Home/Delete/5
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            var recipe = _documentSession.Query<Recipe>().SingleOrDefault(recipeQuery => recipeQuery.ID == id);

            if (recipe == null)
            {
                TempData["Message"] = string.Format("Recipe {0} not found", id);
                return RedirectToAction("Index");
            }

            return View(recipe);
        }

        // POST: /Home/Delete/
        [HttpPost]
        public ActionResult Delete(int id)
        {
            _documentSession.Delete(_documentSession.Query<Recipe>().SingleOrDefault(recipeQuery => recipeQuery.ID == id));
            TempData["Message"] = string.Format("Deleted Recipe with the Id {0}", id);
            return RedirectToAction("Index");
        }
    }
}

