namespace VideoApp
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;
    /// <summary>
    /// Autofac DI
    /// </summary>
    public static class AutoFacConfig
    {
        public static void Register()
        {
            #region Create the builder
            var builder = new ContainerBuilder();
            #endregion
            #region Setup a common pattern
            var assRepository = Assembly.Load("VideoApp.Repository").GetTypes().Where(s=> s.Name.EndsWith("Repository") ).ToArray();
            var assServices = Assembly.Load("VideoApp.Services").GetTypes().Where(s => s.Name.EndsWith("Service")).ToArray();
            builder.RegisterTypes(assRepository).AsImplementedInterfaces();
            builder.RegisterTypes(assServices).AsImplementedInterfaces();
            #endregion
            #region Register all controllers for the assembly
            builder.RegisterControllers(Assembly.Load("VideoApp.Biz"));
            #endregion
            #region Set the MVC dependency resolver to use Autofac
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            #endregion
        }
    }
}
