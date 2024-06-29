using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_ProductsAPI.Data;
using Shop_ProductsAPI.Models;
using Shop_ProductsAPI.Models.Dto;

namespace Shop_ProductsAPI.Controllers
{
    [Route("api/ShopAPI")]
    [ApiController]
    public class ShopAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ShopAPIController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult <IEnumerable<ProductDTO>> GetProducts()
        {
            return Ok(_db.Products.ToList());
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ProductDTO> GetProduct(int id)
        {
            var product = _db.Products.FirstOrDefault(u => u.Id == id);
            if (id == 0)
            {
                return BadRequest();
            }
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        //POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ProductDTO> CreateProduct([FromBody]ProductDTO productDTO)
        {

            if (_db.Products.FirstOrDefault(U => U.Name.ToLower() == productDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Product already exists!");
                return BadRequest(ModelState);
            }
            if (productDTO == null)
            {
                return BadRequest(productDTO);
            }
            if (productDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Product model = new()
            {  
                Id = productDTO.Id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                ImageUrl = productDTO.ImageUrl
            };
            _db.Products.Add(model);
            _db.SaveChanges();

            return CreatedAtRoute("GetProduct", new {id = productDTO.Id }, productDTO);
        }

        //DELETE
        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteProduct(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var product = _db.Products.FirstOrDefault(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _db.Products.Remove(product);
            _db.SaveChanges();
            return NoContent();
        }

        //PUT
        [HttpPut("{id:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateProduct(int id, [FromBody]ProductDTO productDTO)
        {
            if (productDTO == null || id!= productDTO.Id)
            {
                return BadRequest();
            }

            Product model = new()
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                ImageUrl = productDTO.ImageUrl
            };
            _db.Products.Update(model);
            _db.SaveChanges();

            return NoContent();
        }

        //PATCH
        [HttpPatch("{id:int}", Name = "UpdateSpecidicInfoProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateSpecidicInfoProduct(int id, JsonPatchDocument<ProductDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var product = _db.Products.AsNoTracking().FirstOrDefault(u => u.Id == id);

            ProductDTO productDTO = new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
            };

            if (product == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(productDTO, ModelState);

            Product model = new Product()
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                ImageUrl = productDTO.ImageUrl
            };
            _db.Products.Update(model);
            _db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
