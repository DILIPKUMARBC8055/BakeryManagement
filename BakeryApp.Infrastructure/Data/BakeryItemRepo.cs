using BakeryApp.Core.Entities;
using BakeryApp.Core.Repositary;

namespace BakeryApp.Infrastructure.Data
{
    public class BakeryItemRepo : IBakeryItemRepo
    {
        private readonly LiteDbContext _context;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1); 

        public BakeryItemRepo(LiteDbContext context)
        {
            _context = context;
        }

        public async Task<BakeryItem> GetByIdAsync(Guid id)
        {
            await _lock.WaitAsync();
            try
            {
                using (var db = _context.GetDb())
                {
                    var items = db.GetCollection<BakeryItem>("bakeryItems");
                    return items.FindById(id);
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<IEnumerable<BakeryItem>> GetAllAsync()
        {
            await _lock.WaitAsync();
            try
            {
                using (var db = _context.GetDb())
                {
                    var items = db.GetCollection<BakeryItem>("bakeryItems");
                    return items.FindAll().ToList();
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task AddAsync(BakeryItem item)
        {
            await _lock.WaitAsync();
            try
            {
                using (var db = _context.GetDb())
                {
                    var items = db.GetCollection<BakeryItem>("bakeryItems");
                    items.Insert(item);
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task UpdateAsync(BakeryItem item)
        {
            await _lock.WaitAsync();
            try
            {
                using (var db = _context.GetDb())
                {
                    var items = db.GetCollection<BakeryItem>("bakeryItems");
                    items.Update(item);
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _lock.WaitAsync();
            try
            {
                using (var db = _context.GetDb())
                {
                    var items = db.GetCollection<BakeryItem>("bakeryItems");
                    items.Delete(id);
                }
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
