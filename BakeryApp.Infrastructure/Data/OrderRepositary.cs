using BakeryApp.Core.Entities;
using BakeryApp.Core.Repositary;
using LiteDB;
using System.Threading;

namespace BakeryApp.Infrastructure.Data
{
    public class OrderRepository : IOrderRepo
    {
        private readonly LiteDbContext _context;
        private readonly IBakeryItemRepo _bakeryItemRepo;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public OrderRepository(LiteDbContext context, IBakeryItemRepo bakeryItemRepo)
        {
            _context = context;
            _bakeryItemRepo = bakeryItemRepo;
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            await _lock.WaitAsync();
            try
            {
                using (var db = _context.GetDb())
                {
                    var orders = db.GetCollection<Order>("orders");
                    return orders.FindById(id);
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            await _lock.WaitAsync();
            try
            {
                using (var db = _context.GetDb())
                {
                    var orders = db.GetCollection<Order>("orders");
                    return orders.FindAll().ToList();
                }
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task AddAsync(Order order)
        {
            await _lock.WaitAsync();
            try
            {
                using (var db = _context.GetDb())
                {
                    if (db.BeginTrans())
                    {
                        try
                        {
                            var orders = db.GetCollection<Order>("orders");

                            foreach (var orderItem in order.Items)
                            {
                                var items = db.GetCollection<BakeryItem>("bakeryItems");

                                var bakeryItem = items.FindById(orderItem.Id);

                                if (bakeryItem == null || bakeryItem.Quantity <= 0)
                                {
                                    orderItem.Status = "Cancelled due to out of stock or not found";
                                    orderItem.Price = 0;
                                    orderItem.Quantity = 0;
                                    continue;
                                }

                                if (bakeryItem.Quantity < orderItem.Quantity)
                                {
                                    orderItem.Status = "Partially Fulfilled";
                                    orderItem.Quantity = bakeryItem.Quantity;
                                    bakeryItem.Quantity = 0;

                                }
                                else
                                {
                                    orderItem.Status = "Fulfilled";
                                    bakeryItem.Quantity -= orderItem.Quantity;
                                }


                                items.Update(bakeryItem);
                            }
                            order.Total = order.Items.Sum(or => or.Price * or.Quantity);

                            orders.Insert(order);
                            db.Commit();
                        }
                        catch (Exception ex)
                        {
                            db.Rollback();
                            throw new Exception($"Transaction failed: {ex.Message}", ex);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Failed to start the transaction.");
                    }
                }
            }
            finally
            {
                _lock.Release();
            }
        }
        public async Task UpdateAsync(Order order)
        {
            await _lock.WaitAsync();
            try
            {
                using (var db = _context.GetDb())
                {
                    if (db.BeginTrans())
                    {
                        try
                        {
                            var orders = db.GetCollection<Order>("orders");
                            var bakeryItems = db.GetCollection<BakeryItem>("bakeryItems");


                            var oldOrder = orders.FindById(order.Id);
                            if (oldOrder == null)
                            {
                                throw new InvalidOperationException("Order not found.");
                            }


                            foreach (var orderItem in order.Items)
                            {
                                var bakeryItem = bakeryItems.FindById(orderItem.Id);
                                if (bakeryItem == null)
                                {
                                    throw new InvalidOperationException($"Bakery item with ID {orderItem.Id} not found.");
                                }


                                var oldOrderItem = oldOrder.Items.FirstOrDefault(i => i.Id == orderItem.Id);


                                int quantityAdjustment = orderItem.Quantity - (oldOrderItem?.Quantity ?? 0);




                                if (bakeryItem.Quantity <= 0)
                                {

                                    orderItem.Status = $"Out of stock!! was only able to server {orderItem.Quantity}";
                                    bakeryItem.Quantity = 0;

                                }
                                else if (quantityAdjustment > bakeryItem.Quantity)
                                {
                                    orderItem.Quantity = oldOrderItem.Quantity + bakeryItem.Quantity;
                                    orderItem.Status = $"Out of stock!! was only able to server {orderItem.Quantity}";

                                    bakeryItem.Quantity = 0;
                                }
                                else
                                {
                                    bakeryItem.Quantity -= quantityAdjustment;
                                }

                                bakeryItems.Update(bakeryItem);
                            }

                            // Remove items that are no longer in the order
                            foreach (var oldOrderItem in oldOrder.Items)
                            {
                                if (!order.Items.Any(i => i.Id == oldOrderItem.Id))
                                {
                                    var bakeryItem = bakeryItems.FindById(oldOrderItem.Id);
                                    if (bakeryItem != null)
                                    {
                                        bakeryItem.Quantity += oldOrderItem.Quantity;
                                        bakeryItems.Update(bakeryItem);
                                    }
                                }
                            }


                            orders.Update(order);

                            db.Commit(); // Commit the transaction
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Transaction failed: {ex.Message}");
                            db.Rollback(); // Rollback transaction on error
                            throw;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to start the transaction.");
                        throw new InvalidOperationException("Transaction could not be started.");
                    }
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
                    if (db.BeginTrans()) // Start the transaction
                    {
                        try
                        {
                            var orders = db.GetCollection<Order>("orders");
                            orders.Delete(id);
                            db.Commit(); // Commit transaction
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Transaction failed: {ex.Message}");
                            db.Rollback(); // Rollback transaction on error
                            throw;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to start the transaction.");
                        throw new InvalidOperationException("Transaction could not be started.");
                    }
                }
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
