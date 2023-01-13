using play.catalog.service.Entities;

namespace play.catalog.service.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetItemsAsync();
        Task<T> GetItemsAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(T entity);
    }
}