using BakeryApp.Core.Entities;

namespace BakeryApp.Core.Repositary
{
    public interface IUserRepo
    {
        Task<User> GetByIdAsync(Guid id);
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
