using Apipka.DATA;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace Apipka.Controllers
{
    public class ManufacturingController : ApiController
    {
        private Entities db = new Entities();

        // GET api/manufacturing
        public IHttpActionResult GetManufacturingData()
        {
            try
            {
                // Прямой SQL запрос пока не обновите EDMX
                var query = @"
                    SELECT 
                        pm.ProductManufacturingID,
                        p.ProductName,
                        w.WorkshopName,
                        wt.WorkshopTypeName as WorkshopType,
                        pm.ManufacturingTimeHours,
                        w.WorkersCount,
                        pt.ProductTypeName as ProductType,
                        mt.MaterialTypeName as MaterialType,
                        p.MinPartnerPrice
                    FROM ProductManufacturing pm
                    INNER JOIN Products p ON pm.ProductID = p.ProductID
                    INNER JOIN Workshops w ON pm.WorkshopID = w.WorkshopID
                    INNER JOIN WorkshopTypes wt ON w.WorkshopTypeID = wt.WorkshopTypeID
                    INNER JOIN ProductTypes pt ON p.ProductTypeID = pt.ProductTypeID
                    INNER JOIN MaterialTypes mt ON p.MainMaterialTypeID = mt.MaterialTypeID";

                var manufacturingData = db.Database.SqlQuery<ManufacturingDataDto>(query).ToList();
                return Ok(manufacturingData);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/manufacturing/product/5
        [Route("api/manufacturing/product/{productId}")]
        public IHttpActionResult GetManufacturingByProduct(int productId)
        {
            try
            {
                var query = @"
                    SELECT 
                        pm.ProductManufacturingID,
                        w.WorkshopName,
                        wt.WorkshopTypeName as WorkshopType,
                        pm.ManufacturingTimeHours,
                        w.WorkersCount
                    FROM ProductManufacturing pm
                    INNER JOIN Workshops w ON pm.WorkshopID = w.WorkshopID
                    INNER JOIN WorkshopTypes wt ON w.WorkshopTypeID = wt.WorkshopTypeID
                    WHERE pm.ProductID = @productId";

                var manufacturing = db.Database.SqlQuery<ManufacturingByProductDto>(query,
                    new SqlParameter("@productId", productId)).ToList();

                return Ok(manufacturing);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DTO классы для SQL результатов
        public class ManufacturingDataDto
        {
            public int ProductManufacturingID { get; set; }
            public string ProductName { get; set; }
            public string WorkshopName { get; set; }
            public string WorkshopType { get; set; }
            public decimal ManufacturingTimeHours { get; set; }
            public int WorkersCount { get; set; }
            public string ProductType { get; set; }
            public string MaterialType { get; set; }
            public decimal MinPartnerPrice { get; set; }
        }

        public class ManufacturingByProductDto
        {
            public int ProductManufacturingID { get; set; }
            public string WorkshopName { get; set; }
            public string WorkshopType { get; set; }
            public decimal ManufacturingTimeHours { get; set; }
            public int WorkersCount { get; set; }
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