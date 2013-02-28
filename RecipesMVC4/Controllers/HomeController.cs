using Raven.Client;
using RecipesMVC4.Models;
using System.Linq;
using System.Web.Mvc;
using System;

namespace RecipesMVC4.Controllers
{
    public class HomeController : BootstrapBaseController
    {
        private readonly IDocumentSession _documentSession;

        public HomeController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ActionResult Index()
        {
            return View(_documentSession.Query<Recipe>().OrderBy(x => x.ID).ToList());
        }

        public ActionResult Details(Guid id)
        {
            var recipe = _documentSession.Query<Recipe>().SingleOrDefault(recipeQuery => recipeQuery.ID == id);

            if (recipe == null)
            {
                TempData["Error"] = string.Format("Recipe {0} not found", id);
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        // GET: /Home/Edit/5
        public ActionResult Edit(Guid id)
        {
            var recipe = _documentSession.Query<Recipe>().SingleOrDefault(recipeQuery => recipeQuery.ID == id);
            if (recipe == null)
            {
                TempData["Error"] = string.Format("Recipe {0} not found", id);
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        // POST: /Home/Edit/
        [HttpPost]
        public ActionResult Edit(Recipe recipe)
        {
            var doc = _documentSession.Query<Recipe>().SingleOrDefault(recipeQuery => recipeQuery.ID == recipe.ID);
            doc.isIncorrect = recipe.isIncorrect;
            _documentSession.SaveChanges();
            TempData["Success"] = string.Format("Saved changes to Recipe {0}", recipe.Title);

            return RedirectToAction("Index");
        }

    }
}





    