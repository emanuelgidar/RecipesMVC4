using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Raven.Client.Embedded;
using RecipesMVC4.Controllers;
using RecipesMVC4.Models;
using System.Linq;
using System.Web.Security;

namespace RecipesMVC4.Tests.ControllerTests
{

    [TestClass]
    public class AccountControllerTests
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
            var user1 = new User
            {
                FullName = "FullName1",
                Username = "Username1",
                Password = "Password1",
            };
            session.Store(user1);

            var user2 = new User
            {
                FullName = "FullName2",
                Username = "Username2",
                Password = "Password2",
            };
            session.Store(user2);
            session.SaveChanges();
        }

        [TestCleanup]
        public void CleanUp()
        {
            session.Dispose();
        }


        [TestMethod]
        public void Register_Action_Adds_A_User_Into_the_Database()
        {
            //Arrange
            var accountController = new AccountController(session);
            var user2 = session.Query<User>().SingleOrDefault(x => x.Username == "Username2");
            //Act
            accountController.Register(user2);
            var expected = user2;
            var actual = session.Query<User>().SingleOrDefault(x => x.Username == user2.Username);
            //Assert
            Assert.AreEqual(actual, expected);
                }
            }
        }
