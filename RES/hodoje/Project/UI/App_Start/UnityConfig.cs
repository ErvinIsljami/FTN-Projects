using System;
using DataAccess;
using FileReader;
using FileReader.Interfaces;
using FileReader.Loggers;
using FileReader.Readers;
using Unity;
using System.IO;
using FileReader.Writers;
using UI.Controllers;
using Unity.Injection;
using DataProxy;
using Entities.Models;

namespace UI
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            string solutionPath = Path.GetDirectoryName(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory));
            string pathToWatch = Path.Combine(solutionPath, "Readings");
            string logDirectory = Path.Combine(solutionPath, "Log");

            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IReader, XmlReader>();
            container.RegisterType<ILogger, TxtLogger>();
            DatabaseWriter writer = new DatabaseWriter(new UnitOfWork(new DatabaseContext()));
            //Watcher watcher = new Watcher(new XmlReader(), new TxtLogger(), writer, pathToWatch, logDirectory);
            container.RegisterType<IWatcher, Watcher>(new InjectionConstructor(new XmlReader(), new TxtLogger(), writer, pathToWatch, logDirectory));
            container.RegisterType<IPowerConsumptionCachedData, PowerConsumptionCachedData>(new InjectionConstructor(new CacheManager<PowerConsumptionData>(), new UnitOfWork(new DatabaseContext())));
            container.RegisterType<PowerConsumptionController>(new InjectionConstructor(new PowerConsumptionCachedData(new CacheManager<PowerConsumptionData>(), new UnitOfWork(new DatabaseContext()))));
        }
    }
}