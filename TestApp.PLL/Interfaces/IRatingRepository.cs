using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.DAL.Entities;

namespace TestApp.PLL.Interfaces
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetAllRatings();
        Task<List<Rating>> GetRatingsId(int id);
        Task<int> AddRatings(Rating Rate);
    }
}
 