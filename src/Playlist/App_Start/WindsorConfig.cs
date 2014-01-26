using Castle.Windsor;
using Playlist.WindsorInstallers;

namespace Playlist
{
    /// <summary>
    /// Bootstrapper for the Castle Windsor container.
    /// </summary>
    public static class WindsorConfig
    {
        /// <summary>
        /// Creates the container for the application and registers all the necessary components.
        /// </summary>
        public static IWindsorContainer CreateContainer()
        {
            var container = new WindsorContainer();
            container.Install(new CassandraWindsorInstaller(), new MvcControllersInstaller());
            return container;
        }
    }
}