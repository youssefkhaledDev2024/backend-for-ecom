using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.DAL.Entities;

namespace TestApp.PLL.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Categories>
    {

        Task<IEnumerable<Categories>> GetAllCategories();

        Task<Categories> GetCategoriesById(int id);

    }
}
