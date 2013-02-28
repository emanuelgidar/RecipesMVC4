using Autofac;
using Autofac.Integration.Mvc;
using Raven.Client;
using Raven.Client.Document;
using System.Web.Mvc;


namespace RecipesMVC4
{
    public class RavenDBAutofacModule : Autofac.Module
    {

        public static IDocumentStore RavenDBDocumentStore { get; private set; }

        public void Resolve()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.Register(x =>
            {
                var documentStore = new DocumentStore
                {
                    ConnectionStringName = "ravenDB"
                }.Initialize();
                return documentStore;
            }).As<IDocumentStore>().SingleInstance();

            builder.Register(x => x.Resolve<IDocumentStore>().OpenSession())
                .As<IDocumentSession>()
                .InstancePerHttpRequest()
                .OnRelease(x =>
                {
                    x.SaveChanges();
                    x.Dispose();
                });

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}