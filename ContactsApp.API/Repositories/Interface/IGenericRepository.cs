using ContactsApp.API.Models.Domain;

namespace ContactsApp.API.Repositories.Interface
{
    public interface IGenericRepository<T> where T: class
    {
        Task CreateEntityAsync(T entity);
        Task<IEnumerable<T>> GetAllEntitiesAsync();
        Task<T?> GetEntityByIdAsync(int id);
        Task<T?> UpdateEntityAsync(T updatedEntity);
        Task<T?> DeleteEntityAsync(int id);

    }
}
