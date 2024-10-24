using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.DAL.Context;
using TestApp.DAL.Entities;
using TestApp.PLL.Interfaces;

namespace TestApp.PLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly StoreContext _storeContext;
        public GenericRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        public async Task<int> Add(T item)
        {
            await _storeContext.Set<T>().AddAsync(item);
            return await _storeContext.SaveChangesAsync();
           
        }

        public async Task<int> DeleteById(int id)
        {
            var entity = await _storeContext.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return 0; // or throw an exception if you prefer
            }

            _storeContext.Set<T>().Remove(entity);
            return await _storeContext.SaveChangesAsync();
        }

        public  async Task<IEnumerable<T>> GetAll()
        {
            return await _storeContext.Set<T>().ToListAsync();
        }

        public  async Task<T> GetById(int id)
        {
            return await _storeContext.Set<T>().FindAsync(id);

        }

        public async Task<int> Update(T item)
        {
            var existingEntity = await _storeContext.Set<T>().FindAsync(item.ID);
            if (existingEntity != null)
            {
                _storeContext.Entry(existingEntity).State = EntityState.Detached;
            }

            _storeContext.Set<T>().Attach(item);
            _storeContext.Entry(item).State = EntityState.Modified;

            return await _storeContext.SaveChangesAsync();
        }
    }
}
