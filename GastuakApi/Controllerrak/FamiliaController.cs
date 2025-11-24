using GastuakApi.Repositorioak;
using GastuakApi.Modeloak;
using Microsoft.AspNetCore.Mvc;

namespace GastuakApi.Controllerrak
{
    [ApiController]
    [Route("api/[controller]")]
    public class FamiliaController: ControllerBase
    {
        private readonly FamiliaRepository _familiaRepo;

        public FamiliaController(
            FamiliaRepository familiaRepo,
            ErabiltzaileaRepostory erabRepo)
        {
            _familiaRepo = familiaRepo;
        }

        /*
        // POST api/familia
        [HttpPost]
        public IActionResult SortuFamilia([FromBody] FamiliaSortuDTO dto)
        {
            // Familia sortu
            var familia = new Familia
            {
                Izena = dto.Izena
            };

            _familiaRepo.Add(familia);

            return Ok(new
            {
                mezua = "Familia sortuta",
                familiaId = familia.Id
            });
        }
        */

        // GET api/familia
        [HttpGet]
        public IActionResult GetAll()
        {
            var familiak = _familiaRepo.GetAll();
            return Ok(familiak);
        }

        //TODO: Eager loading-ekin arazoak dauzkat. Post beintzat sortu du baina kargatzerakoan arazoa ematen du. Begiratu ea zergatik den hau.

        /*
        // GET api/familia/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id, bool eager = false)
        {
            var familia = _familiaRepo.Get(id, eager);

            if (familia == null)
                return NotFound(new { mezua = "Familia ez da existitzen" });

            return Ok(familia);
        }
        */
    }

    public class FamiliaSortuDTO
    {
        public string Izena { get; set; }
        public List<int> ErabiltzaileIds { get; set; } = new();
    }
}
