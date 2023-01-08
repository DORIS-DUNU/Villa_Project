using VilllaParks.Model;

namespace VilllaParks.Core.IRepository
{
    public interface IVillaNumberRepository : IGenericRepository<VillaParkNumber>
    {
        Task<VillaParkNumber> UpdateAsync(VillaParkNumber entity);
    }
}
