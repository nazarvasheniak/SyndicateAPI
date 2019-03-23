using NHibernate;

namespace SyndicateAPI.Storage.Interfaces
{
    public interface ISessionStorage
    {
        ISession Session { get; }
    }
}
