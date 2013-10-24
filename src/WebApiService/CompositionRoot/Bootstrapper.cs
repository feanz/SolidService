using Client.Code;
using SimpleInjector.Extensions;

namespace WebApiService.CompositionRoot
{
    using System.Reflection;
    using System.Security.Principal;
    using System.Threading;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;
    using BusinessLayer;
    using Contract;
    using SimpleInjector;
    using WebApiService.Code;

    public static class Bootstrapper
    {
        public static void Bootstrap()
        {
            // Did you know the container can diagnose your configuration? Go to: http://bit.ly/YE8OJj.
            var container = new Container();

            BusinessLayerBootstrapper.Bootstrap(container);

			//if you need the api to call back to services then just remove business layer ref and uncomment these
			//container.RegisterOpenGeneric(typeof(IQueryHandler<,>), typeof(WcfServiceQueryHandlerProxy<,>));
			//container.RegisterOpenGeneric(typeof(ICommandHandler<>), typeof(WcfServiceCommandHandlerProxy<>));

            RegisterWebApiSpecificDependencies(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.RegisterMvcAttributeFilterProvider();

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = 
                new SimpleInjectorWebApiDependencyResolver(container);

            System.Web.Mvc.DependencyResolver.SetResolver(
                new SimpleInjector.Integration.Web.Mvc.SimpleInjectorDependencyResolver(container));
        }

        private static void RegisterWebApiSpecificDependencies(Container container)
        {
            container.Register<IPrincipal>(() => HttpContext.Current.User ?? Thread.CurrentPrincipal);
            container.RegisterSingle<ILogger, DebugLogger>();
            container.RegisterSingle<IQueryProcessor, DynamicQueryProcessor>();

            // This provider builds the list of commands and queries.
            var provider = new CommandControllerDescriptorProvider(typeof(ICommandHandler<>).Assembly);

            container.RegisterSingle<CommandControllerDescriptorProvider>(provider);
           
            container.RegisterSingle<IHttpControllerSelector, CommandHttpControllerSelector>();
            container.RegisterSingle<IHttpActionSelector, CommandHttpActionSelector>();

            // This line is optional, but by registering all controllers explicitly, they will be
            // verified when calling Verify().
            foreach (var commandDescriptor in provider.GetDescriptors())
            {
                container.Register(commandDescriptor.ControllerDescriptor.ControllerType);
            }
        }
    }
}