using BakeryApp.Core.Entities;

namespace BakeryApp.Core.Repositary
{
    public interface IBakeryItemRepo
    {
        Task<BakeryItem> GetByIdAsync(Guid id);
        Task<IEnumerable<BakeryItem>> GetAllAsync();
        Task AddAsync(BakeryItem item);
        Task UpdateAsync(BakeryItem item);
        Task DeleteAsync(Guid id);
    }
}
