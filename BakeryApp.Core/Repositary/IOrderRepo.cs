using BakeryApp.Core.Entities;

namespace BakeryApp.Core.Repositary
{
    public interface IOrderRepo
    {
        Task<Order> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Guid id);
    }
}
