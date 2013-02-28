using Raven.Client;
using RecipesMVC4.Models;
using System.Linq;
using System.Web.Mvc;
using System.Security;
using System;

namespace RecipesMVC4.Controllers
{
    [Authorize]
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
            recipe.ID = Guid.NewGuid();
            _documentSession.Store(recipe);
            TempData["Success"] = string.Format("Recipe {0} added successfully !", recipe.Title);
            return RedirectToAction("Index");
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
            doc.Title = recipe.Title;
            doc.Description = recipe.Description;
            doc.isIncorrect = recipe.isIncorrect;
            _documentSession.SaveChanges();
            TempData["Success"] = string.Format("Saved changes to Recipe {0}", recipe.Title);

            return RedirectToAction("Index");
        }

        // GET: /Home/Delete/5
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(Guid id)
        {
            var recipe = _documentSession.Query<Recipe>().SingleOrDefault(recipeQuery => recipeQuery.ID == id);

            if (recipe == null)
            {
                TempData["Error"] = string.Format("Recipe {0} not found", id);
                return RedirectToAction("Index");
            }

            return View(recipe);
        }

        // POST: /Home/Delete/
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            _documentSession.Delete(_documentSession.Query<Recipe>().SingleOrDefault(recipeQuery => recipeQuery.ID == id));
            TempData["Success"] = string.Format("Deleted Recipe with the Id {0}", id);
            return RedirectToAction("Index");
        }
    }
}

