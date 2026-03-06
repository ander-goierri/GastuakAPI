using Microsoft.AspNetCore.Mvc;
using GastuakApi.Modeloak;
using GastuakApi.Repositorioak;

namespace GastuakApi.Controllerrak
{
    [ApiController]
    [Route("api/products")]
    public class ProduktuakController : ControllerBase
    {
        private readonly ProduktuaRepository _repository;

        public ProduktuakController(ProduktuaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _repository.Get(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Produktua product)
        {
            _repository.Save(product);
            return Ok(product);
        }
    }
}
