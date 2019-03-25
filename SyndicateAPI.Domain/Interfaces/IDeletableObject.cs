
namespace SyndicateAPI.Domain.Interfaces
{
    public interface IDeletableObject
    {
        /// <summary>
        /// Архивирован
        /// </summary>
        bool Deleted { get; set; }
    }
}
