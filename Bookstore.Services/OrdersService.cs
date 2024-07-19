using Bookstore.Data;
using Bookstore.Entities;
using Bookstore.Services.DTO.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public class OrdersService
    {
        private readonly Bookstore_v2023Context _dbContext;
        public OrdersService(Bookstore_v2023Context dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<OrderDto> GetLatestOrderByUser(string username)
        {
            var order = await _dbContext.Orders
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Book)
            .Include(x => x.Customer)
            .OrderByDescending(x => x.OrderDateTime)
            .LastOrDefaultAsync(x => x.Customer.EmailAddress == username &&
           !x.IsPaid);
            if (order == null)
            {
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(x =>
               x.EmailAddress == username);
                if (customer == null)
                {
                    customer = new Customer()
                    {
                        EmailAddress = username,
                        FullName = "",
                        PhoneNumber = ""
                    };
                    _dbContext.Add(customer);
                    await _dbContext.SaveChangesAsync();
                }
                order = new Order()
                {
                    OrderDateTime = DateTime.Now,
                    PaymentMethod = "card",
                    DeliveryAddress = "",
                    IsPaid = false,
                    IsDelivered = false,
                    CustomerId = customer.Id
                };
                _dbContext.Add(order);
                await _dbContext.SaveChangesAsync();
            }
            return new OrderDto()
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                DeliveryAddress = order.DeliveryAddress,
                IsDelivered = order.IsDelivered,
                IsPaid = order.IsPaid,
                OrderDateTime = order.OrderDateTime,
                Customer = new DTO.Orders.CustomerDto()
                {
                    Id = order.Customer.Id,
                    EmailAddress = order.Customer.EmailAddress,
                    FullName = order.Customer.FullName,
                    PhoneNumber = order.Customer.PhoneNumber,
                },
                PaymentMethod = order.PaymentMethod,
                OrderDetails = order.OrderDetails.Select(d => new OrderDetailsDto()
                {
                    BookId = d.BookId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    Book = new DTO.Books.BookDto()
                    {
                        Id = d.BookId,
                        Title = d.Book.Title
                    }
                }).ToList()
            };
        }
        public async Task AddBook(int orderId, int bookId)
        {
            var order = await _dbContext.Orders.Include(x =>
           x.OrderDetails).FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null)
            {
                return;
            }
            var book = await _dbContext.Books.FindAsync(bookId);
            if (book == null)
            {
                return;
            }
            var existing = order.OrderDetails.FirstOrDefault(x => x.BookId ==
           bookId);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                order.OrderDetails.Add(new OrderDetail()
                {
                    BookId = bookId,
                    Quantity = 1,
                    UnitPrice = book.Price!.Value
                });
            }
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddBook(string username, int bookId)
        {
            var order = await this.GetLatestOrderByUser(username);
            await this.AddBook(order.Id, bookId);
        }
        public async Task SetOrderPaid(string username)
        {
            var order = await this.GetLatestOrderByUser(username);
            if (order != null)
            {
                order.IsPaid = true;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
