using Cassandra;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Playlist.Data;

namespace Playlist.WindsorInstallers
{
    /// <summary>
    /// Registers all Cassandra components with Windsor, including all the DAO classes.
    /// </summary>
    public class CassandraWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Use the builder to create an instance of Session connected to localhost
            Session sessionInstance = Cluster.Builder().AddContactPoint("localhost").Build().Connect();

            // Do container registrations
            container.Register(
                // The Java code uses a Singleton instance for Session (and the C# driver docs indicate the same applies for C#), 
                // so register a single instance.  In the Java sample, this is done in the CassandraData base class that all
                // DAO objects inherit from, but we don't really need that base class
                Component.For<Session>().Instance(sessionInstance),

                // Register all the DAO objects in the Playlist.Data project
                Classes.FromAssemblyContaining<ICassandraInfo>().Pick().WithServiceFirstInterface().LifestyleTransient()
            );
        }
    }
}