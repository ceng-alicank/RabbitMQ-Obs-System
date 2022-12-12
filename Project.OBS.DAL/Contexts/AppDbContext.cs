using Microsoft.EntityFrameworkCore;
using Project.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.OBS.DAL.Contexts
{
    public class AppDbContext:DbContext
    {
        public DbSet<TranscriptModel> TranscriptModels { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=OBSDB1;Username=postgres;Password=password");

    }
}
