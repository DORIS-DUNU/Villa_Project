using VilllaParks.Model;

namespace VilllaParks.Core.IRepository
{
    public interface IVillaParkRepository : IGenericRepository<VillaPark>
    {
        Task<VillaPark> UpdateAsync(VillaPark entity);
    }
}
