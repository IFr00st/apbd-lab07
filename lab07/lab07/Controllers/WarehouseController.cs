using lab07.Models;
using lab07.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace lab07.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly WarehouseRepository _warehouseRepository;

        public WarehouseController(WarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        [HttpPost]
        public async Task<IActionResult> InsertWarehouseProduct(Warehouse warehouse)
        {
            if (!await _warehouseRepository.DoesProductExist(warehouse.IdProduct))
                return NotFound($"Product - {warehouse.IdProduct} doesn't exist");
            if (!await _warehouseRepository.DoesWarehouseExist(warehouse.IdWarehouse))
                return NotFound($"Warehouse - {warehouse.IdWarehouse} doesn't exist");
            if (!await _warehouseRepository.DoesRightOrderExist(warehouse.IdProduct, warehouse.Amount))
                return NotFound("Right order doesn't exist");

            await _warehouseRepository.InsertIntoProductWarehouse(warehouse.IdProduct, warehouse.IdWarehouse,
                warehouse.Amount);
            return Ok("0");

        }
    }
}
