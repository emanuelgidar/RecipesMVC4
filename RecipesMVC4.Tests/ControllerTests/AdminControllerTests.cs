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
    public class AdminControllerTests
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
                    var adminController = new AdminController(session);
            //Act
                    ViewResult result = adminController.Index() as ViewResult;
            //Assert
                    Assert.IsNotNull(result);

            //Act
                    var actual = result.Model as List<Recipe>;
                    var expected = session.Query<Recipe>().OrderBy(x => x.ID).ToList();
            //Assert
                    CollectionAssert.AreEqual(actual, expected);
                }
            
        


        [TestMethod]
        public void Details_ReturnsARecipe_WhenTheRecipeIDIsVald()
        {
            //Arrange
                    var adminController = new AdminController(session);

                    var recipe1 = session.Query<Recipe>().SingleOrDefault(x => x.Title == "Title1");
            //Act
                    ViewResult result = adminController.Details(recipe1.ID) as ViewResult;
            //Assert
                    Assert.IsNotNull(result);
            //Act
                    var actual = result.Model as Recipe;
                    var expected = session.Query<Recipe>().SingleOrDefault(x => x.ID == recipe1.ID);
            //Assert
                    Assert.AreEqual(actual, expected);
        }


        [TestMethod]
        public void Edit_ReturnsARecipeAndCanUpdateARecipe_WhenTheRecipeIDIsValid()
        {
            //Arrange
                    var adminController = new AdminController(session);

                    var recipe1 = session.Query<Recipe>().SingleOrDefault(x => x.Title == "Title1");
            //Act         
                    ViewResult result = adminController.Edit(recipe1.ID) as ViewResult;

                    var actual = result.Model as Recipe;
                    var expected = session.Query<Recipe>().SingleOrDefault(x => x.ID == recipe1.ID);
            //Assert
                    Assert.AreEqual(actual, expected);

                    //update step
                    //in admin-mode all fields cand be updated
            //Arrange
                    var recipeToUpdate = new Recipe
                    {
                        Title = "UpdatedTitle",
                        Description = "UpdatedDescription",
                        ID = recipe1.ID,
                        isIncorrect = false
                    };
            //Act
                    ViewResult resultOnUpdate = adminController.Edit(recipe1.ID) as ViewResult;
                    var expectedAfterUpdate = recipeToUpdate;
                    adminController.Edit(recipeToUpdate);
                    var actualAfterUpdate = session.Query<Recipe>().SingleOrDefault(recipe => recipe.ID == recipeToUpdate.ID);
            //Assert
                    Assert.AreEqual(actualAfterUpdate, expectedAfterUpdate);
                }
            }
        }

