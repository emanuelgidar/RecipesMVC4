using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client.Embedded;
using RecipesMVC4.Controllers;
using RecipesMVC4.Models;
using System.Linq;

namespace RecipesMVC4.Tests.ControllerTests
{

    [TestClass]
    public class AccountControllerTests
    {

        [TestMethod]
        public void SignIn_Action_Changes_The_State_Of_A_User()
        {
            using (var documentStore = new EmbeddableDocumentStore
            {
                RunInMemory = true
            })
            {
                documentStore.Initialize();

                using (var session = documentStore.OpenSession())
                {
                    var user1 = new User
                    {
                        ID = 1,
                        FullName = "FullName1",
                        Username = "Username1",
                        Password = "Password1",
                        IsLoggedIn = false
                    };
                    session.Store(user1);

                    var user2 = new User
                    {
                        ID = 2,
                        FullName = "FullName2",
                        Username = "Username2",
                        Password = "Password2",
                        IsLoggedIn = false
                    };
                    session.Store(user2);
                    session.SaveChanges();

                    var accountController = new AccountController(session);
                    accountController.SignIn(user2);
                    Assert.IsTrue(user2.IsLoggedIn);
                    session.Dispose();          
                }
            }
        }


        [TestMethod]
        public void Register_Action_Adds_A_User_Into_the_Database()
        {
            using (var documentStore = new EmbeddableDocumentStore
            {
                RunInMemory = true
            })
            {
                documentStore.Initialize();

                using (var session = documentStore.OpenSession())
                {
                    var user1 = new User
                    {
                        ID = 1,
                        FullName = "FullName1",
                        Username = "Username1",
                        Password = "Password1",
                        IsLoggedIn = false
                    };
                    session.Store(user1);

                    var user2 = new User
                    {
                        ID = 2,
                        FullName = "FullName2",
                        Username = "Username2",
                        Password = "Password2",
                        IsLoggedIn = false
                    };
                    session.Store(user2);
                    session.SaveChanges();

                    var accountController = new AccountController(session);

                    var user3 = new User
                    {
                        FullName = "FullName3",
                        Username = "Username3",
                        Password = "Password3",
                       };
                    
                    accountController.Register(user3);
                    var expected = user3;
                    var actual = session.Query<User>().SingleOrDefault( x => x.Username == user3.Username);
                    Assert.AreEqual(actual, expected);
                    session.Dispose(); 
                }
            }
        }
    }
}
