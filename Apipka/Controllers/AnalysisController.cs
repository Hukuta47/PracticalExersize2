using Apipka.DATA;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace Apipka.Controllers
{
    public class AnalysisController : ApiController
    {
        private Entities db = new Entities();

        // GET api/analysis/summary
        [Route("api/analysis/summary")]
        public IHttpActionResult GetProductionSummary()
        {
            try
            {
                var query = @"
                    SELECT 
                        pt.ProductTypeName as ProductType,
                        w.WorkshopName as Workshop,
                        SUM(pm.ManufacturingTimeHours) as TotalTime,
                        AVG(pm.ManufacturingTimeHours) as AverageTime,
                        COUNT(*) as ProductCount
                    FROM ProductManufacturing pm
                    INNER JOIN Products p ON pm.ProductID = p.ProductID
                    INNER JOIN ProductTypes pt ON p.ProductTypeID = pt.ProductTypeID
                    INNER JOIN Workshops w ON pm.WorkshopID = w.WorkshopID
                    GROUP BY pt.ProductTypeName, w.WorkshopName";

                var summary = db.Database.SqlQuery<ProductionSummaryDto>(query).ToList();
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/analysis/products/totaltime
        [Route("api/analysis/products/totaltime")]
        public IHttpActionResult GetProductsTotalTime()
        {
            try
            {
                var query = @"
                    SELECT 
                        p.ProductID,
                        p.ProductName,
                        SUM(pm.ManufacturingTimeHours) as TotalManufacturingTime,
                        COUNT(DISTINCT pm.WorkshopID) as WorkshopCount,
                        SUM(pm.ManufacturingTimeHours * w.WorkersCount) as TotalLaborHours
                    FROM ProductManufacturing pm
                    INNER JOIN Products p ON pm.ProductID = p.ProductID
                    INNER JOIN Workshops w ON pm.WorkshopID = w.WorkshopID
                    GROUP BY p.ProductID, p.ProductName
                    ORDER BY TotalManufacturingTime DESC";

                var productsTotalTime = db.Database.SqlQuery<ProductTotalTimeDto>(query).ToList();
                return Ok(productsTotalTime);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DTO классы для SQL результатов
        public class ProductionSummaryDto
        {
            public string ProductType { get; set; }
            public string Workshop { get; set; }
            public decimal TotalTime { get; set; }
            public decimal AverageTime { get; set; }
            public int ProductCount { get; set; }
        }

        public class ProductTotalTimeDto
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public decimal TotalManufacturingTime { get; set; }
            public int WorkshopCount { get; set; }
            public decimal TotalLaborHours { get; set; }
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