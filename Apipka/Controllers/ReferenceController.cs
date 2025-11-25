using Apipka.DATA;
using Apipka.DTO;
using System;
using System.Linq;
using System.Web.Http;

namespace Apipka.Controllers
{
    [RoutePrefix("api/reference")]
    public class ReferenceController : ApiController
    {
        private Entities db = new Entities();

        // GET api/reference/producttypes
        [HttpGet]
        [Route("producttypes")]
        public IHttpActionResult GetProductTypes()
        {
            try
            {
                var productTypes = db.ProductTypes
                    .Select(pt => new ProductTypeDto
                    {
                        ProductTypeID = pt.ProductTypeID,
                        ProductTypeName = pt.ProductTypeName,
                        ProductTypeCoefficient = pt.ProductTypeCoefficient
                    })
                    .ToList();

                return Ok(productTypes);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/reference/materialtypes
        [HttpGet]
        [Route("materialtypes")]
        public IHttpActionResult GetMaterialTypes()
        {
            try
            {
                var materialTypes = db.MaterialTypes
                    .Select(mt => new MaterialTypeDto
                    {
                        MaterialTypeID = mt.MaterialTypeID,
                        MaterialTypeName = mt.MaterialTypeName,
                        RawMaterialLossPercent = mt.RawMaterialLossPercent
                    })
                    .ToList();

                return Ok(materialTypes);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/reference/workshoptypes
        [HttpGet]
        [Route("workshoptypes")]
        public IHttpActionResult GetWorkshopTypes()
        {
            try
            {
                var workshopTypes = db.WorkshopTypes
                    .Select(wt => new WorkshopTypeDto
                    {
                        WorkshopTypeID = wt.WorkshopTypeID,
                        WorkshopTypeName = wt.WorkshopTypeName
                    })
                    .ToList();

                return Ok(workshopTypes);
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