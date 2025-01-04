using BakeryApp.Core.Entities;
using BakeryApp.Core.Repositary;

namespace BakeryApp.Infrastructure.Data
{
    public class UserRepositary : IUserRepo
    {
        private readonly LiteDbContext _context;

        public UserRepositary(LiteDbContext context)
        {
            _context = context;
        }
        public async Task<User> GetByIdAsync(Guid id)
        {
            return await Task.Run(() =>
            {
                using (var db = _context.GetDb())
                {
                    var users = db.GetCollection<User>("users");
                    return users.FindById(id);
                }
            });
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {

            if (string.IsNullOrEmpty(username))
            {
                return null;
            }


            using (var db = _context.GetDb())
            {

                var users = db.GetCollection<User>("users");


                var user = users.Find(u => u.Username == username).FirstOrDefault();

                return user;
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await Task.Run(() =>
            {
                using (var db = _context.GetDb())
                {
                    var users = db.GetCollection<User>("users");
                    return users.FindAll().ToList();
                }
            });
        }

        public async Task AddAsync(User user)
        {
            await Task.Run(() =>
            {
                using (var db = _context.GetDb())
                {
                    var users = db.GetCollection<User>("users");


                    if (users.FindOne(u => u.Email == user.Email) != null)
                    {
                        throw new InvalidOperationException("A user with the same email already exists.");
                    }

                    users.Insert(user);
                }
            });
        }

        public async Task UpdateAsync(User user)
        {
            await Task.Run(() =>
            {
                using (var db = _context.GetDb())
                {
                    var users = db.GetCollection<User>("users");
                    if (!users.Update(user))
                    {
                        throw new InvalidOperationException("User not found or could not be updated.");
                    }
                }
            });
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.Run(() =>
            {
                using (var db = _context.GetDb())
                {
                    var users = db.GetCollection<User>("users");
                    if (!users.Delete(id))
                    {
                        throw new InvalidOperationException("User not found or could not be deleted.");
                    }
                }
            });
        }

    }
}
