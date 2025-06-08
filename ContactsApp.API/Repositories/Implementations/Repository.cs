using ContactsApp.API.Data;
using ContactsApp.API.Models.Entities;
using ContactsApp.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.API.Repositories.Implementations
{
    public class Repository<T>: IGenericRepository<T> where T: class, IEntity
    {
        private readonly AppDbContext dbContext;
        private readonly DbSet<T> dbSet;

        public Repository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
        }

        public async Task CreateEntityAsync(T entity)
        {
            await this.dbSet.AddAsync(entity);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<T?> DeleteEntityAsync(int id)
        {
            var contactToDelete = await this.GetEntityByIdAsync(id);

            if(contactToDelete == null)
            {
                return null;
            }

            dbSet.Remove(contactToDelete);
            await dbContext.SaveChangesAsync();
            
            return contactToDelete;
        }

        public async Task<IEnumerable<T>> GetAllEntitiesAsync()
        {
            return await this.dbSet.ToListAsync();
        }

        public async Task<T?> GetEntityByIdAsync(int id)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T?> UpdateEntityAsync(T updatedEntity)
        {
            var existingContact = await dbSet.FirstOrDefaultAsync(x => x.Id == updatedEntity.Id);
            
            if(existingContact!= null)
            {
                dbSet.Entry(existingContact).CurrentValues.SetValues(updatedEntity);
                await dbContext.SaveChangesAsync();
            }

            return existingContact;
        }
    }
}
