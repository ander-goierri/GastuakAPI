using GastuakApi.Repositorioak;
using GastuakApi.Modeloak;
using GastuakApi.DTOak;
using Microsoft.AspNetCore.Mvc;

namespace GastuakApi.Controllerrak
{
    /// <summary>
    /// Familia baliabidearekin (familien kudeaketa) lotutako API amaiera-puntuak (endpoints) eskaintzen dituen kontrolatzailea.
    /// </summary>
    /// <remarks>
    /// Kontrolatzaile honek familiak zerrendatu, bilatu, sortu, eguneratu eta ezabatzeko eragiketak eskaintzen ditu.
    /// Gainera, familia bati erabiltzaileak gehitu edo kentzeko PATCH amaiera-puntuak ditu.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class FamiliaController : ControllerBase
    {
        private readonly FamiliaRepository _familiaRepo;
        private readonly ErabiltzaileaRepository _erabiltzaileaRepo;

        /// <summary>
        /// <see cref="FamiliaController"/> klasearen eraikitzailea.
        /// </summary>
        /// <param name="familiaRepo">Familien bilaketa eta CRUD eragiketak egiteko repositorioa.</param>
        /// <param name="erabiltzaileaRepo">Erabiltzaileen bilaketa eta kudeaketa egiteko repositorioa.</param>
        public FamiliaController(
            FamiliaRepository familiaRepo,
            ErabiltzaileaRepository erabiltzaileaRepo)
        {
            _familiaRepo = familiaRepo;
            _erabiltzaileaRepo = erabiltzaileaRepo;
        }

        /// <summary>
        /// Familia guztiak itzultzen ditu.
        /// </summary>
        /// <param name="eager">
        /// <c>true</c> bada, familiaren barruan dauden erabiltzaileak ere itzuliko dira (Erabiltzaileak beteak).
        /// <c>false</c> bada, familiaren oinarrizko datuak bakarrik itzuliko dira (Id eta Izena).
        /// </param>
        /// <returns>
        /// Familia zerrenda bat (DTO formatuan). Ongi badoa, HTTP 200 (OK) erantzuten du.
        /// </returns>
        /// <remarks>
        /// Aukera erabilgarria da front-endean: zerrenda azkarragoa nahi bada eager=false; xehetasunak behar badira eager=true.
        /// </remarks>
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

        /// <summary>
        /// ID baten bidez familia bat lortzen du.
        /// </summary>
        /// <param name="id">Bilatu nahi den familiaren identifikatzailea.</param>
        /// <param name="eager">
        /// <c>true</c> bada, familiaren erabiltzaileak ere kargatzen/itzultzen dira.
        /// <c>false</c> bada, familiaren datu nagusiak bakarrik itzultzen dira.
        /// </param>
        /// <returns>
        /// Familia aurkitzen bada: HTTP 200 (OK) + <see cref="FamiliaDto"/>.
        /// Familia existitzen ez bada: HTTP 404 (NotFound).
        /// </returns>
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

        /// <summary>
        /// Familia berri bat sortzen du (eta nahi bada, hasierako erabiltzaileak lotzen dizkio).
        /// </summary>
        /// <param name="dto">
        /// Sortzeko datuak: familiaren izena eta familiari lotu nahi zaizkion erabiltzaileen ID zerrenda.
        /// </param>
        /// <returns>
        /// Ongi badoa, HTTP 200 (OK) eta sortutako familiaren IDa bueltatzen du.
        /// </returns>
        /// <remarks>
        /// Kontuan hartu: dto.ErabiltzaileIds barruan datozen IDak existitzen ez badira,
        /// repositorioaren inplementazioaren arabera null itzul daiteke edo errorea gerta daiteke.
        /// (Hemen ez da balidazio espliziturik egiten.)
        /// </remarks>
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

            // Sartutako erabiltzaileak bilatzen dira eta familiari gehitzen zaizkio
            foreach (var erabiltzaileId in dto.ErabiltzaileIds)
            {
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

        /// <summary>
        /// Familia bati erabiltzaileak gehitzen dizkio (lotura gehitu).
        /// </summary>
        /// <param name="id">Eguneratu nahi den familiaren identifikatzailea.</param>
        /// <param name="dto">Gehitu nahi diren erabiltzaileen ID zerrenda.</param>
        /// <returns>
        /// Familia existitzen ez bada: HTTP 404 (NotFound).  
        /// Erabiltzailerik ez bada aurkitzen: HTTP 400 (BadRequest).  
        /// Ongi badoa: HTTP 200 (OK).
        /// </returns>
        /// <remarks>
        /// Metodo honek ez ditu duplikatuak gehitzen: erabiltzailea familian badago, ez du berriz txertatzen.
        /// </remarks>
        // PATCH api/familia/{id}/gehituErabiltzaileak
        [HttpPatch("{id}/gehituErabiltzaileak")]
        public IActionResult AddErabiltzaileak(int id, [FromBody] FamiliaAddErabiltzaileakDTO dto)
        {
            var familia = _familiaRepo.Get(id, eager: true);
            if (familia == null)
                return NotFound(new { mezua = "Familia ez da existitzen" });

            // Erabiltzaileak lortu
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

        /// <summary>
        /// Familia bati erabiltzaileak kentzen dizkio (lotura kendu).
        /// </summary>
        /// <param name="id">Eguneratu nahi den familiaren identifikatzailea.</param>
        /// <param name="dto">Kendu nahi diren erabiltzaileen ID zerrenda.</param>
        /// <returns>
        /// Familia existitzen ez bada: HTTP 404 (NotFound).  
        /// Erabiltzailerik ez bada aurkitzen: HTTP 400 (BadRequest).  
        /// Ongi badoa: HTTP 200 (OK).
        /// </returns>
        /// <remarks>
        /// Oharra: baldintza txiki bat dago kodean: hemen <c>if (!familia.Erabiltzaileak.Contains(e))</c> dago,
        /// baina kentzeko logikan normaltasunez <c>if (familia.Erabiltzaileak.Contains(e))</c> izan beharko luke.
        /// Bestela, daudenak ez ditu kenduko.
        /// </remarks>
        // PATCH api/familia/{id}/kenduErabiltzaileak
        [HttpPatch("{id}/kenduErabiltzaileak")]
        public IActionResult RemoveErabiltzaileak(int id, [FromBody] FamiliaAddErabiltzaileakDTO dto)
        {
            var familia = _familiaRepo.Get(id, eager: true);
            if (familia == null)
                return NotFound(new { mezua = "Familia ez da existitzen" });

            // Erabiltzaileak lortu
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

        /// <summary>
        /// Familia baten datu osoak eguneratzen ditu (PUT: ordezkatu).
        /// </summary>
        /// <param name="id">Eguneratu nahi den familiaren identifikatzailea.</param>
        /// <param name="dto">Familia berrien datuak (une honetan: Izena).</param>
        /// <returns>
        /// Familia existitzen ez bada: HTTP 404 (NotFound).  
        /// Ongi badoa: HTTP 200 (OK).
        /// </returns>
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

        /// <summary>
        /// Familia baten datuen zati bat eguneratzen du (PATCH: partziala).
        /// </summary>
        /// <param name="id">Eguneratu nahi den familiaren identifikatzailea.</param>
        /// <param name="dto">Aldatu nahi diren eremuak (ez bada hutsik/whitespace).</param>
        /// <returns>
        /// Familia existitzen ez bada: HTTP 404 (NotFound).  
        /// Ongi badoa: HTTP 200 (OK).
        /// </returns>
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

        /// <summary>
        /// Familia bat ezabatzen du.
        /// </summary>
        /// <param name="id">Ezabatu nahi den familiaren identifikatzailea.</param>
        /// <returns>
        /// Familia existitzen ez bada: HTTP 404 (NotFound).  
        /// Ongi badoa: HTTP 200 (OK).
        /// </returns>
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

    /// <summary>
    /// Familia berri bat sortzeko (POST/PUT) erabiltzen den DTOa.
    /// </summary>
    /// <remarks>
    /// - <see cref="Izena"/>: familiaren izena.
    /// - <see cref="ErabiltzaileIds"/>: familiari lotu nahi zaizkion erabiltzaileen ID zerrenda.
    /// </remarks>
    public class FamiliaSortuDTO
    {
        /// <summary>
        /// Familiaren izena.
        /// </summary>
        public string Izena { get; set; }

        /// <summary>
        /// Familia sortzean familiari esleitu nahi zaizkion erabiltzaileen identifikatzaileak.
        /// </summary>
        public List<int> ErabiltzaileIds { get; set; } = new();
    }

    /// <summary>
    /// Familia partzialki eguneratzeko (PATCH) erabiltzen den DTOa.
    /// </summary>
    public class FamiliaPatchDTO
    {
        /// <summary>
        /// Familiaren izen berria (aukerakoa).
        /// </summary>
        public string? Izena { get; set; }
    }

    /// <summary>
    /// Familia bati erabiltzaileak gehitu/kentzeko erabiltzen den DTOa.
    /// </summary>
    /// <remarks>
    /// Endpoint hauetan erabiltzen da:
    /// - PATCH: /api/familia/{id}/gehituErabiltzaileak
    /// - PATCH: /api/familia/{id}/kenduErabiltzaileak
    /// </remarks>
    public class FamiliaAddErabiltzaileakDTO
    {
        /// <summary>
        /// Gehitu edo kendu nahi diren erabiltzaileen identifikatzaileak.
        /// </summary>
        public List<int> ErabiltzaileIds { get; set; } = new();
    }
}
