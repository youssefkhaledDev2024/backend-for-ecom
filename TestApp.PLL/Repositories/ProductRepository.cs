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
    public class ProductRepository : GenericRepository<Products> , IProductRepository
    {
        private readonly StoreContext _storeContext;
        public ProductRepository( StoreContext storeContext) : base(storeContext )
        {
            _storeContext = storeContext;
        }

        public async Task  AddProductAsync(Products newProduct, int categoryId)
        {
            var category = _storeContext.Categories.FindAsync(categoryId);

            if (category == null) {
                throw new Exception("wrong process");
            }

            newProduct.CategoryId = categoryId;
            newProduct.Category = await category;

            _storeContext.Products.Add(newProduct);
            await _storeContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Products>> GetAllProducts()
        {
            return await _storeContext.Products.Include(p => p.Ratings).ToListAsync();
        }

        public async Task<Products> GetProductById(int id)
        {
            return await _storeContext.Products.Include(p => p.Ratings).FirstOrDefaultAsync(p => p.ID == id);
        }
    }
}
