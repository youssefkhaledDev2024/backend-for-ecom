using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApp.DAL.Context;
using TestApp.DAL.Entities;
using TestApp.ModelClasses;
using TestApp.PLL.Interfaces;
using TestApp.PLL.Repositories;

namespace TestApp.Controllers
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<Users> _userManager;
        private readonly StoreContext _StoreContext;
        public RatingController(IRatingRepository ratingRepository , IMapper mapper , UserManager<Users> userManager , StoreContext storeContext)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
            _userManager = userManager;
            _StoreContext = storeContext;

            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rating>>> GetAllRatings()
        {
            var ratings = await _ratingRepository.GetAllRatings();
            var ratingViewModels = _mapper.Map<IEnumerable<RatingModel>>(ratings);
            return Ok(ratingViewModels);
        }

        [HttpPost]
        public async Task<ActionResult<RatingModel>> AddRating([FromBody] RatingModel ratingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the current logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Map the view model to the entity
            var ratingEntity = _mapper.Map<Rating>(ratingModel);
            ratingEntity.UserId = user.Id; // Set the UserId

            await _ratingRepository.AddRatings(ratingEntity);

            // Calculate the new average rating for the product
            var product = await _StoreContext.Products
                .Include(p => p.Ratings)
                .FirstOrDefaultAsync(p => p.ID == ratingEntity.ProductId);

            if (product != null)
            {
                product.Rating = product.Ratings.Any()
                    ? product.Ratings.Average(r => r.Score)
                    : 0;

                await _StoreContext.SaveChangesAsync(); // Update product rating
            }

            var addedRating = await _StoreContext.Ratings
                .Include(r => r.Product)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == ratingEntity.Id);

            return Ok(_mapper.Map<RatingModel>(addedRating));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatingById(int id)
        {
            var ratings = await _ratingRepository.GetRatingsId(id);
            var ratingViewModels = _mapper.Map<IEnumerable<RatingModel>>(ratings);
            return Ok(ratingViewModels);
        }



    }
}
