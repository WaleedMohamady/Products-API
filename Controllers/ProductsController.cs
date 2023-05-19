using Microsoft.AspNetCore.Mvc;
using nWeaveTask.BL;
using nWeaveTask.BL.DTOs;

namespace nWeaveTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsManager _productsManager;

        public ProductsController(IProductsManager productsManager)
        {
            _productsManager = productsManager;
        }

        // GET /api/products
        [HttpGet]
        public ActionResult<IEnumerable<ProductReadDTO>> GetProducts()
        {
            return _productsManager.GetAll();
        }

        // GET /api/products/{id}
        [HttpGet("{id}")]
        public ActionResult<ProductReadDTO> GetProduct(Guid id)
        {
         var product = _productsManager.GetById(id);
            if (product == null) { return NotFound(); }
            return product;
        }

        // PUT /api/products/{id}
        [HttpPut("{id}")]
        public IActionResult PutProduct(Guid id, ProductUpdateDTO product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
           var UpdatedProduct = _productsManager.Update(product);
            if (UpdatedProduct == true) {return NoContent();}

            return NotFound();
        }

        // POST /api/products
        [HttpPost]
        public ActionResult<ProductReadDTO> PostProduct(ProductAddDTO productAddDTO)
        {
            var productReadDTO = _productsManager.Add(productAddDTO);
            return CreatedAtAction("GetProduct", productReadDTO);
        }

        // DELETE /api/products/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(Guid id)
        {
            _productsManager.Delete(id);
            return NoContent();
        }

    }
}
