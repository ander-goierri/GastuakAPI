using GastuakApi.Repositorioak;
using GastuakApi.Modeloak;
using GastuakApi.DTOak;
using Microsoft.AspNetCore.Mvc;

namespace GastuakApi.Controllerrak
{
    [ApiController]
    [Route("api/[controller]")]
    public class FamiliaController: ControllerBase
    {
        private readonly FamiliaRepository _familiaRepo;
        private readonly ErabiltzaileaRepository _erabiltzaileaRepo;

        public FamiliaController(
            FamiliaRepository familiaRepo,
            ErabiltzaileaRepository erabiltzaileaRepo)
        {
            _familiaRepo = familiaRepo;
            _erabiltzaileaRepo = erabiltzaileaRepo;
        }
        

        // GET api/familia
        [HttpGet]
        public IActionResult GetAll(bool eager = false)
        {
            var familiak = _familiaRepo.GetAll();

            var familiakDto = new List<FamiliaDto>();

            foreach (var familia in familiak)
            {
                if (eager)
                {
                    familiakDto.Add(new FamiliaDto
                    {
                        Id = familia.Id,
                        Izena = familia.Izena,
                        Erabiltzaileak = familia.Erabiltzaileak
                            .Select(e => new ErabiltzaileaDto
                            {
                                Id = e.Id,
                                Izena = e.Izena,
                                Abizena = e.Abizena
                            })
                            .ToList()
                    });
                }
                else
                {
                    familiakDto.Add(new FamiliaDto
                    {
                        Id = familia.Id,
                        Izena = familia.Izena
                    });
                }
            }

            return Ok(familiakDto);
        }

        // GET api/familia/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id, bool eager = false)
        {
            var familia = _familiaRepo.Get(id, eager);
            var familiaDto = new FamiliaDto { };

            if (familia == null)
                return NotFound(new { mezua = "Familia ez da existitzen" });
            if (eager)
            {
                familiaDto = new FamiliaDto
                {
                    Id = familia.Id,
                    Izena = familia.Izena,
                    Erabiltzaileak = familia.Erabiltzaileak
                    .Select(e => new ErabiltzaileaDto
                    {
                        Id = e.Id,
                        Izena = e.Izena,
                        Abizena = e.Abizena
                    })
                .ToList()
                };
            }
            else
            {
                familiaDto = new FamiliaDto
                {
                    Id = familia.Id,
                    Izena = familia.Izena
                };

            }

            return Ok(familiaDto);
        }


        // POST api/familia
        [HttpPost]
        public IActionResult SortuFamilia([FromBody] FamiliaSortuDTO dto)
        {
            // Familia sortu
            var familia = new Familia
            {
                Izena = dto.Izena
            };

            IList<Erabiltzailea> erabZerrenda = [];

            //Sartutako erabiltzaileak bilatzen dira eta familiari gehitzen zaizkio
            foreach (var erabiltzaileId in dto.ErabiltzaileIds) {
                Erabiltzailea erab = _erabiltzaileaRepo.Get(erabiltzaileId);
                erabZerrenda.Add(erab);
            }

            familia.Erabiltzaileak = erabZerrenda;

            _familiaRepo.Add(familia);

            return Ok(new
            {
                mezua = "Familia sortuta",
                familiaId = familia.Id
            });
        }


        // PATCH api/familia/{id}/gehituErabiltzaileak
        [HttpPatch("{id}/gehituErabiltzaileak")]
        public IActionResult AddErabiltzaileak(int id, [FromBody] FamiliaAddErabiltzaileakDTO dto)
        {
            var familia = _familiaRepo.Get(id, eager: true);
            if (familia == null)
                return NotFound(new { mezua = "Familia ez da existitzen" });

            // Obtener los usuarios
            var erabiltzaileak = _erabiltzaileaRepo.GetByIds(dto.ErabiltzaileIds);

            if (!erabiltzaileak.Any())
                return BadRequest(new { mezua = "Ez da aurkitu erabiltzailerik" });

            foreach (var e in erabiltzaileak)
            {
                if (!familia.Erabiltzaileak.Contains(e))
                    familia.Erabiltzaileak.Add(e);
            }

            _familiaRepo.Update(familia);

            return Ok(new { mezua = "Erabiltzaileak gehitu dira familiara" });
        }

        // PATCH api/familia/{id}/gehituErabiltzaileak
        [HttpPatch("{id}/kenduErabiltzaileak")]
        public IActionResult RemoveErabiltzaileak(int id, [FromBody] FamiliaAddErabiltzaileakDTO dto)
        {
            var familia = _familiaRepo.Get(id, eager: true);
            if (familia == null)
                return NotFound(new { mezua = "Familia ez da existitzen" });

            // Obtener los usuarios
            var erabiltzaileak = _erabiltzaileaRepo.GetByIds(dto.ErabiltzaileIds);

            if (!erabiltzaileak.Any())
                return BadRequest(new { mezua = "Ez da aurkitu erabiltzailerik" });

            foreach (var e in erabiltzaileak)
            {
                if (!familia.Erabiltzaileak.Contains(e))
                    familia.Erabiltzaileak.Remove(e);
            }

            _familiaRepo.Update(familia);

            return Ok(new { mezua = "Erabiltzaileak gehitu dira familiara" });
        }


        //TODO: 1- bi ruta familiarrak gehitu eta familiarrak kentzeko
        //TODO: 2- put eta patch-ak eguneratu

        // PUT api/familia/{id}
        [HttpPut("{id}")]
        public IActionResult EguneratuFamilia(int id, [FromBody] FamiliaSortuDTO dto)
        {
            var familia = _familiaRepo.Get(id);
            if (familia == null)
                return NotFound(new { mezua = "Familia ez da existitzen" });

            familia.Izena = dto.Izena;

            _familiaRepo.Update(familia);

            return Ok(new { mezua = "Familia eguneratuta" });
        }

        // PATCH api/familia/{id}
        [HttpPatch("{id}")]
        public IActionResult EguneratuZatia(int id, [FromBody] FamiliaPatchDTO dto)
        {
            var familia = _familiaRepo.Get(id);
            if (familia == null)
                return NotFound(new { mezua = "Familia ez da existitzen" });

            if (!string.IsNullOrWhiteSpace(dto.Izena))
                familia.Izena = dto.Izena;

            _familiaRepo.Update(familia);

            return Ok(new { mezua = "Familia zati batean eguneratuta" });
        }

        // DELETE api/familia/{id}
        [HttpDelete("{id}")]
        public IActionResult EzabatuFamilia(int id)
        {
            var familia = _familiaRepo.Get(id);
            if (familia == null)
                return NotFound(new { mezua = "Familia ez da existitzen" });

            _familiaRepo.Delete(familia);

            return Ok(new { mezua = "Familia ezabatua" });
        }


    }

    public class FamiliaSortuDTO
    {
        public string Izena { get; set; }
        public List<int> ErabiltzaileIds { get; set; } = new();
    }


    public class FamiliaPatchDTO
    {
        public string? Izena { get; set; }
    }
    public class FamiliaAddErabiltzaileakDTO
    {
        public List<int> ErabiltzaileIds { get; set; } = new();
    }
}
