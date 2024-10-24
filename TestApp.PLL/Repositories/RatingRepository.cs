using Microsoft.EntityFrameworkCore;
using TestApp.DAL.Context;
using TestApp.DAL.Entities;
using TestApp.PLL.Interfaces;

public class RatingRepository : IRatingRepository
{
    private readonly StoreContext _storeContext;
    public RatingRepository(StoreContext storeContext)
    {
        _storeContext = storeContext;
    }

    public async Task<int> AddRatings(Rating rate)
    {
        // Add the rating
        await _storeContext.Set<Rating>().AddAsync(rate);
        await _storeContext.SaveChangesAsync();

        // Recalculate product's average rating
        var product = await _storeContext.Set<Products>().Include(p => p.Ratings)
                               .FirstOrDefaultAsync(p => p.ID == rate.ProductId);
        if (product != null)
        {
            product.CalculateAverageRating();
            _storeContext.Set<Products>().Update(product);
        }

        return await _storeContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Rating>> GetAllRatings()
    {
        return await _storeContext.Set<Rating>()
                                  .Include(r => r.Product)
                                  .Include(r => r.User)
                                  .ToListAsync();
    }

    public async Task<List<Rating>> GetRatingsId(int id)
    {
        return await _storeContext.Set<Rating>()
                                  .Include(r => r.Product)
                                  .Include(r => r.User)
                                  .Where(r => r.ProductId == id)
                                  .ToListAsync();
    }
}
