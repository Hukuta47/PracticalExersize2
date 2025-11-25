using Apipka.DATA;
using System;
using System.Linq;
using System.Web.Http;

namespace Apipka.Controllers
{
    public class ValuesController : ApiController
    {
        private Entities db = new Entities();

        // GET api/values
        public IHttpActionResult Get()
        {
            try
            {
                // Проверка подключения к базе
                var productCount = db.Products.Count();
                var workshopCount = db.Workshops.Count();

                return Ok(new
                {
                    message = "API работает корректно",
                    database = "Подключение к базе данных успешно",
                    productsCount = productCount,
                    workshopsCount = workshopCount,
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
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