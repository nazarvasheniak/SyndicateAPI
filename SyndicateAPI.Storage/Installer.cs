using Microsoft.Extensions.DependencyInjection;
using SyndicateAPI.Storage.Interfaces;
using SyndicateAPI.Storage.Repositories;

namespace SyndicateAPI.Storage
{
    public static class Installer
    {
        public static void AddNHibernate(this IServiceCollection container, string connectionString)
        {
            container.AddSingleton(typeof(NHibernateConfigurator.ISessionFactory), new NHibernateConfigurator.NHibernateConfiguration(connectionString));
            container.AddScoped<ISessionStorage, SessionStorage>();
            container.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}
