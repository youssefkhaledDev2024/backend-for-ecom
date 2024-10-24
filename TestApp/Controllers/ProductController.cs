using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class ProductController : ControllerBase
    {

        private readonly IProductRepository _productRepository;
        private readonly StoreContext _storeContext;
        private readonly IMapper _mapper;
        public ProductController(IProductRepository productRepository , StoreContext storeContext , IMapper mapper)
        {
            _productRepository = productRepository;
            _storeContext = storeContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsModel>>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProducts();

            // Calculate the average rating for each product if not already set
            foreach (var product in products)
            {
                product.Rating = product.Ratings.Any()
                    ? product.Ratings.Average(r => r.Score)
                    : 0;
            }

            var productsModel = _mapper.Map<IEnumerable<ProductsModel>>(products);
            return Ok(productsModel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ProductsModel>>> GetProductsById( int id)
        {
           var  product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            // Calculate the average rating
            product.Rating = product.Ratings.Any()
                ? product.Ratings.Average(r => r.Score)
                : 0;

            var productModel = _mapper.Map<ProductsModel>(product);
            return Ok(productModel);
        }


        [HttpPost]
        public async Task<ActionResult<List<ProductsModel>>> AddNewProducts([FromBody] ProductsModel product)
        {


            var category = await _storeContext.Categories.FindAsync(product.CategoryId);

            if (category == null)
            {
                return BadRequest($"Category with ID {product.CategoryId} does not exist.");
            }



            var ProductEntity = _mapper.Map<Products>(product);
            await _productRepository.Add(ProductEntity);


            return Ok(new List<ProductsModel>());
        }

        [HttpPut]
        public async Task<ActionResult<List<ProductsModel>>> UpdateProduct([FromBody] ProductsModel product , int id)
        {

            if (id != product.ID)
            {
                return BadRequest("ID mismatch");
            }

            var existingProduct = await _productRepository.GetById(id);
            if (existingProduct == null)
            {
                return NotFound("Product not found");
            }

            // Update the existing product properties as necessary
            existingProduct.ProductTittle = product.ProductTittle;
            existingProduct.ProductPrice = product.ProductPrice;
            existingProduct.ProductDescription = product.ProductDescription;
            existingProduct.ImageName = product.ImageName;

            // Add other properties as needed

            await _productRepository.Update(existingProduct);
            return Ok();
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult<List<ProductsModel>>> DeleteProduct(int id)
        {
            var result = await _productRepository.DeleteById(id);
            if (result == 0)
            {
                return NotFound("Product not found");
            }

            return NoContent();
        }

    }
}
