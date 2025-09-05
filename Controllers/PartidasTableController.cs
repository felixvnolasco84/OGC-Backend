using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OGCBackend.Models;
using OGCBackend.Repositories;

namespace OGCBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartidasTableController : ControllerBase
    {



        private readonly IPartidaRepository _partidaRepository;

        private readonly ILogger<PartidasTableController> _logger;


        public PartidasTableController(IPartidaRepository partidaRepository, ILogger<PartidasTableController> logger)
        {
            _partidaRepository = partidaRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Partida>> GetPartidasByFamilia(string familia)
        {
            return await _partidaRepository.GetPartidasByFamiliaAsync(familia);
        }

    }
}