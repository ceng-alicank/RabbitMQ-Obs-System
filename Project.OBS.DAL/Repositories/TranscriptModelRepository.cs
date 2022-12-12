using Microsoft.EntityFrameworkCore;
using Project.Common;
using Project.OBS.DAL.Interfaces;
using System.Linq.Expressions;

namespace Project.OBS.DAL.Repositories
{

    public class TranscriptModelRepository : ITranscriptModelRepository
    {
        private readonly DbContext _context;
        public TranscriptModelRepository(DbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TranscriptModel entity)
        {
            _context.Set<TranscriptModel>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Set<TranscriptModel>().Where(p => p.Id == id).FirstOrDefaultAsync();
            if (entity != null)
            {
                _context.Set<TranscriptModel>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TranscriptModel> GetByIdAsync(Guid id)
        {
            var entity = await _context.Set<TranscriptModel>().Where(p => p.Id == id).FirstOrDefaultAsync();
            if (entity != null)
            {
                return entity;
            }
            return null;
        }

        public async Task<List<TranscriptModel>> GetAllAsync(Expression<Func<TranscriptModel, bool>> filter = null)
        {
            if (filter != null) await _context.Set<TranscriptModel>().Where(filter).ToListAsync();
            return await _context.Set<TranscriptModel>().ToListAsync();
        }

        public async Task UpdateAsync(TranscriptModel entity)
        {
            TranscriptModel? existing = await _context.Set<TranscriptModel>().FindAsync(entity.Id);
            if (existing != null)
            {

                _context.Entry(existing).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }

    }
    
}
