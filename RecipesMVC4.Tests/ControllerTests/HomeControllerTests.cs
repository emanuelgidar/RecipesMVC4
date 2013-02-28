using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client.Embedded;
using Raven.Client.Linq;
using RecipesMVC4.Controllers;
using RecipesMVC4.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;
using Raven.Client;

namespace RecipesMVC4.Tests.ControllerTests
{

    [TestClass]
    public class HomeControllerTests
    {
        IDocumentStore documentStore;
        IDocumentSession session;
             
        [TestInitialize]
        public void InitialiseTest()
        {
            documentStore = new EmbeddableDocumentStore
            {
                RunInMemory = true
            };
            documentStore.Initialize();

            session = documentStore.OpenSession();
                    var recipe1 = new Recipe
                    {
                        Title = "Title1",
                        Description = "Description1",
                        ID = Guid.NewGuid(),
                        isIncorrect = false
                    };
                    session.Store(recipe1);

                    var recipe2 = new Recipe
                    {
                        Title = "Title2",
                        Description = "Description2",
                        ID = Guid.NewGuid(),
                        isIncorrect = true
                    };
                    session.Store(recipe2);

                    var recipe3 = new Recipe
                    {
                        Title = "Title3",
                        Description = "Description3",
                        ID = Guid.NewGuid(),
                        isIncorrect = false
                    };
                    session.Store(recipe3);

                    var recipe4 = new Recipe
                    {
                        Title = "Title4",
                        Description = "Description4",
                        ID = Guid.NewGuid(),
                        isIncorrect = true
                    };
                    session.Store(recipe4);
                    session.SaveChanges();
        }
        
        
        [TestCleanup]
        public void CleanUp()
        {
            session.Dispose();
        }
            

        [TestMethod]
        public void Index_ReturnsListOfRecipes_WhenThereAreRecipesInDB()
        {
            //Arrange
                    var homeController = new HomeController(session);
            //Act
                    ViewResult result = homeController.Index() as ViewResult;
            //Assert
                    Assert.IsNotNull(result);

            //Act
                    var actual = result.Model as List<Recipe>;
                    var expected = session.Query<Recipe>().OrderBy(x => x.ID).ToList();
            //Assert
                    CollectionAssert.AreEqual(actual, expected);
        }


        [TestMethod]
        public void Details_ReturnsARecipe_WhenTheRecipeIDIsVali()
        {
            //Arrange
                    var homeController = new HomeController(session);

                    var recipe1 = session.Query<Recipe>().SingleOrDefault(x => x.Title == "Title1");
            //Act
                    ViewResult result = homeController.Details(recipe1.ID) as ViewResult;
            //Assert
                    Assert.IsNotNull(result);
            
            //Act
                    var actual = result.Model as Recipe;
                    var expected = session.Query<Recipe>().SingleOrDefault(x => x.ID == recipe1.ID);
            //Assert
                    Assert.AreEqual(actual, expected);
        }


        [TestMethod]
        public void Edit_ReturnsARecipeAndCanUpdateARecipe_WhenTheRecipeIDIsVali()
        {
            //Arrange
                    var homeController = new HomeController(session);

                    var recipe1 = session.Query<Recipe>().SingleOrDefault(x => x.Title == "Title1");
            //Act 
                    ViewResult result = homeController.Edit(recipe1.ID) as ViewResult;
            //Assert
                    Assert.IsNotNull(result);

            //Act
                    var actual = result.Model as Recipe;
                    var expected = session.Query<Recipe>().SingleOrDefault(x => x.ID == recipe1.ID);
            //Assert
                    Assert.AreEqual(actual, expected);
                    
                    //update step
            //Arrange
                    var updatedRecipe = new Recipe
                    {
                        Title = "Title1",
                        Description = "Description1",
                        ID = recipe1.ID,
                        //only the isIncorrect value cand be changet without being signed In
                        isIncorrect = false
                    };
            //Act
                    ViewResult resultOnUpdate = homeController.Edit(updatedRecipe) as ViewResult;

                    var expectedAfterUpdate = updatedRecipe;
                    homeController.Edit(updatedRecipe.ID);
                    var actualAfterUpdate = session.Query<Recipe>().SingleOrDefault(x => x.ID == updatedRecipe.ID);
             //Assert
                    Assert.AreEqual(actualAfterUpdate, expectedAfterUpdate);
        }
    }

}
