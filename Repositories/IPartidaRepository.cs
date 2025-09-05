using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OGCBackend.Models;

namespace OGCBackend.Repositories
{
    public interface IPartidaRepository : IRepository<Partida>
    {

        Task<IEnumerable<Partida>> GetAllAsync();
        Task<IEnumerable<Partida>> GetPartidasByFamiliaAsync(string familia);
        Task<IEnumerable<Partida>> GetPartidasByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Partida> GetPartidaWithDetailsAsync(int id);
        Task<bool> ExistsAsync(string nombre, string familia, string subPartida);
        Task DeleteByArchivoOrigenAsync(string archivoOrigen);
    }
}