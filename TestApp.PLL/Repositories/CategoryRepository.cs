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
    public class CategoryRepository : GenericRepository<Categories> , ICategoryRepository
    {
        private readonly StoreContext _storeContext;
        public CategoryRepository( StoreContext storeContext) : base(storeContext) { 
            _storeContext = storeContext;
        
        }

        public async Task<IEnumerable<Categories>> GetAllCategories()
        {
            return await _storeContext.Categories.Include(p => p.Products).ToListAsync();
        }

        public  async Task<Categories> GetCategoriesById(int id)
        {
            return await _storeContext.Categories.Include(p => p.Products).FirstOrDefaultAsync(P => P.ID == id);
        }
    }
}
