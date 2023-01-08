using VilllaParks.Core.IRepository;
using VilllaParks.Data;
using VilllaParks.Model;

namespace VilllaParks.Core.Repository
{
    public class VillaParkNumberRepository : GenericRepository<VillaParkNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaParkNumberRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public async Task<VillaParkNumber> UpdateAsync(VillaParkNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.VillaParkNumbers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

    }
}
