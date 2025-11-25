using Apipka.DATA;
using Apipka.DTO;
using System;
using System.Linq;
using System.Web.Http;

namespace Apipka.Controllers
{
    public class WorkshopsController : ApiController
    {
        private Entities db = new Entities();

        // GET api/workshops
        public IHttpActionResult GetWorkshops()
        {
            try
            {
                var workshops = db.Workshops
                    .Select(w => new WorkshopDto
                    {
                        WorkshopID = w.WorkshopID,
                        WorkshopName = w.WorkshopName,
                        WorkshopType = w.WorkshopTypes.WorkshopTypeName,
                        WorkersCount = w.WorkersCount,
                        WorkshopTypeID = w.WorkshopTypeID
                    })
                    .ToList();

                return Ok(workshops);
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