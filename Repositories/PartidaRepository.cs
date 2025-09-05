// Repositories/PartidaRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OGCBackend.Data;
using OGCBackend.Models;

namespace OGCBackend.Repositories
{
    public class PartidaRepository : Repository<Partida>, IPartidaRepository
    {
        public PartidaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Partida>> GetPartidasByFamiliaAsync(string familia)
        {
            return await _context.Partidas
                .Where(p => p.Familia == familia)
                .OrderBy(p => p.SubPartida)
                .ToListAsync();
        }

        public async Task<IEnumerable<Partida>> GetPartidasByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Partidas
                .Where(p => p.FechaCarga >= startDate && p.FechaCarga <= endDate)
                .OrderByDescending(p => p.FechaCarga)
                .ToListAsync();
        }

        public async Task<Partida> GetPartidaWithDetailsAsync(int id)
        {
            return await _context.Partidas
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ExistsAsync(string nombre, string familia, string subPartida)
        {
            return await _context.Partidas
                .AnyAsync(p => p.Nombre == nombre &&
                              p.Familia == familia &&
                              p.SubPartida == subPartida);
        }

        public async Task DeleteByArchivoOrigenAsync(string archivoOrigen)
        {
            var partidas = await _context.Partidas
                .Where(p => p.ArchivoOrigen == archivoOrigen)
                .ToListAsync();

            _context.Partidas.RemoveRange(partidas);
            await _context.SaveChangesAsync();
        }
    }
}