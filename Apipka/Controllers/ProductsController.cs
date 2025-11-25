using Apipka.DATA;
using Apipka.DTO;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace Apipka.Controllers
{
    public class ProductsController : ApiController
    {
        private Entities db = new Entities();

        // GET api/products
        public IHttpActionResult GetProducts()
        {
            try
            {
                var products = db.Products
                    .Include(p => p.ProductTypes)
                    .Include(p => p.MaterialTypes)
                    .Select(p => new ProductDto
                    {
                        ProductID = p.ProductID,
                        ProductName = p.ProductName,
                        ArticleNumber = p.ArticleNumber,
                        MinPartnerPrice = p.MinPartnerPrice,
                        ProductType = p.ProductTypes.ProductTypeName,
                        MaterialType = p.MaterialTypes.MaterialTypeName,
                        RawMaterialLossPercent = p.MaterialTypes.RawMaterialLossPercent,
                        ProductTypeID = p.ProductTypeID,
                        MaterialTypeID = p.MainMaterialTypeID
                    })
                    .ToList();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/products/5
        public IHttpActionResult GetProduct(int id)
        {
            try
            {
                var product = db.Products
                    .Include(p => p.ProductTypes)
                    .Include(p => p.MaterialTypes)
                    .Where(p => p.ProductID == id)
                    .Select(p => new ProductDto
                    {
                        ProductID = p.ProductID,
                        ProductName = p.ProductName,
                        ArticleNumber = p.ArticleNumber,
                        MinPartnerPrice = p.MinPartnerPrice,
                        ProductType = p.ProductTypes.ProductTypeName,
                        MaterialType = p.MaterialTypes.MaterialTypeName,
                        RawMaterialLossPercent = p.MaterialTypes.RawMaterialLossPercent,
                        ProductTypeID = p.ProductTypeID,
                        MaterialTypeID = p.MainMaterialTypeID
                    })
                    .FirstOrDefault();

                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST api/products
        [HttpPost]
        public IHttpActionResult CreateProduct([FromBody] ProductCreateDto productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var product = new Products
                {
                    ProductName = productDto.ProductName,
                    ArticleNumber = productDto.ArticleNumber,
                    MinPartnerPrice = productDto.MinPartnerPrice,
                    ProductTypeID = productDto.ProductTypeID,
                    MainMaterialTypeID = productDto.MaterialTypeID
                };

                db.Products.Add(product);
                db.SaveChanges();

                return Ok(new { message = "Product created successfully", productId = product.ProductID });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT api/products/5
        [HttpPut]
        public IHttpActionResult UpdateProduct(int id, [FromBody] ProductCreateDto productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingProduct = db.Products.Find(id);
                if (existingProduct == null)
                    return NotFound();

                existingProduct.ProductName = productDto.ProductName;
                existingProduct.ArticleNumber = productDto.ArticleNumber;
                existingProduct.MinPartnerPrice = productDto.MinPartnerPrice;
                existingProduct.ProductTypeID = productDto.ProductTypeID;
                existingProduct.MainMaterialTypeID = productDto.MaterialTypeID;

                db.SaveChanges();

                return Ok(new { message = "Product updated successfully" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE api/products/5
        [HttpDelete]
        public IHttpActionResult DeleteProduct(int id)
        {
            try
            {
                var product = db.Products.Find(id);
                if (product == null)
                    return NotFound();

                db.Products.Remove(product);
                db.SaveChanges();

                return Ok(new { message = "Product deleted successfully" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}