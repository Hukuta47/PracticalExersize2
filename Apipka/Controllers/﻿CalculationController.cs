using Apipka.DATA;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace Apipka.Controllers
{
    [RoutePrefix("api/calculation")]
    public class CalculationController : ApiController
    {
        private Entities db = new Entities();

        // GET api/calculation/rawmaterial
        [HttpGet]
        [Route("rawmaterial")]
        public IHttpActionResult CalculateRawMaterial(int productTypeId, int materialTypeId, int quantity, double param1, double param2)
        {
            try
            {
                // Получаем коэффициенты из базы данных
                var productType = db.ProductTypes.Find(productTypeId);
                var materialType = db.MaterialTypes.Find(materialTypeId);

                if (productType == null || materialType == null)
                {
                    return BadRequest("Тип продукции или материал не найден");
                }

                // Формула расчета сырья:
                // Количество_сырья = (Параметр1 × Параметр2) × Количество_продукции × Коэффициент_типа_продукции × (1 + Процент_потерь/100)

                double baseMaterial = param1 * param2; // Базовая потребность в сырье на единицу
                double productTypeCoefficient = (double)productType.ProductTypeCoefficient;
                double lossCoefficient = 1 + ((double)materialType.RawMaterialLossPercent / 100);

                // Расчет общего количества сырья
                double rawMaterialNeeded = baseMaterial * quantity * productTypeCoefficient * lossCoefficient;

                // Округляем до целого в большую сторону
                int result = (int)Math.Ceiling(rawMaterialNeeded);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/calculation/productiontime
        [HttpGet]
        [Route("productiontime")]
        public IHttpActionResult CalculateProductionTime(int productId, int quantity)
        {
            try
            {
                // Используем прямой SQL запрос, так как ProductManufacturing нет в EDMX
                var query = @"
                    SELECT SUM(pm.ManufacturingTimeHours) as TotalTimePerUnit
                    FROM ProductManufacturing pm
                    WHERE pm.ProductID = @productId";

                var totalTimePerUnit = db.Database.SqlQuery<decimal?>(
                    query, new SqlParameter("@productId", productId)).FirstOrDefault();

                if (totalTimePerUnit == null || totalTimePerUnit == 0)
                {
                    return BadRequest("Нет данных о производстве для этого продукта");
                }

                // Общее время = время на единицу × количество
                decimal totalTime = totalTimePerUnit.Value * quantity;

                return Ok(new { TotalTime = totalTime });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/calculation/productcost
        [HttpGet]
        [Route("productcost")]
        public IHttpActionResult CalculateProductCost(int productId, int quantity)
        {
            try
            {
                // Расчет стоимости производства продукта
                var query = @"
                    SELECT 
                        p.MinPartnerPrice,
                        SUM(pm.ManufacturingTimeHours * w.WorkersCount * 500) as LaborCost, -- 500 руб/час стоимость рабочего часа
                        p.MinPartnerPrice * 0.3 as MaterialCost -- 30% от цены - стоимость материалов
                    FROM Products p
                    LEFT JOIN ProductManufacturing pm ON p.ProductID = pm.ProductID
                    LEFT JOIN Workshops w ON pm.WorkshopID = w.WorkshopID
                    WHERE p.ProductID = @productId
                    GROUP BY p.ProductID, p.MinPartnerPrice";

                var costData = db.Database.SqlQuery<CostCalculationDto>(
                    query, new SqlParameter("@productId", productId)).FirstOrDefault();

                if (costData == null)
                {
                    return BadRequest("Продукт не найден");
                }

                var totalCost = costData.LaborCost + costData.MaterialCost;
                var profit = costData.MinPartnerPrice - totalCost;
                var totalForQuantity = totalCost * quantity;

                return Ok(new
                {
                    LaborCost = costData.LaborCost,
                    MaterialCost = costData.MaterialCost,
                    TotalCostPerUnit = totalCost,
                    ProfitPerUnit = profit,
                    TotalForQuantity = totalForQuantity
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DTO для расчета стоимости
        public class CostCalculationDto
        {
            public decimal MinPartnerPrice { get; set; }
            public decimal LaborCost { get; set; }
            public decimal MaterialCost { get; set; }
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