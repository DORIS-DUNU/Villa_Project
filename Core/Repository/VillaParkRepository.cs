using VilllaParks.Core.IRepository;
using VilllaParks.Data;
using VilllaParks.Model;

namespace VilllaParks.Core.Repository
{
    public class VillaParkRepository : GenericRepository<VillaPark>, IVillaParkRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaParkRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public async Task<VillaPark> UpdateAsync(VillaPark entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.VillaParks.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
