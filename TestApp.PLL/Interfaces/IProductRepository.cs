using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.DAL.Entities;

namespace TestApp.PLL.Interfaces
{
    public interface IProductRepository : IGenericRepository<Products>
    {

        Task<IEnumerable<Products>> GetAllProducts();

        Task<Products> GetProductById(int id);


        Task AddProductAsync(Products newProduct, int categoryId);

    }
}
    