using Project.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.OBS.DAL.Interfaces
{
    public interface ITranscriptModelRepository
    {
        public Task AddAsync(TranscriptModel entity);
        public Task DeleteAsync(Guid id);
        public Task<TranscriptModel> GetByIdAsync(Guid id);
        public Task<List<TranscriptModel>> GetAllAsync(Expression<Func<TranscriptModel, bool>> filter = null);
        public Task UpdateAsync(TranscriptModel entity);

    }
}
