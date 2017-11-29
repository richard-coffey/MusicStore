using System;

namespace MusicStore.Models
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ShoppingCart _shoppingCart;

        public OrderRepository(AppDbContext appDbContext, ShoppingCart shoppingCart)
        {
            _appDbContext = appDbContext;
            _shoppingCart = shoppingCart;
        }

        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;
            _appDbContext.Orders.Add(order);

            var shoppingCartItems = _shoppingCart.ShoppingCartItems;
            decimal orderTotal = 0;

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = shoppingCartItem.Amount,
                    AlbumId = shoppingCartItem.Album.AlbumId,
                    Price = shoppingCartItem.Album.Price,
                    OrderId = order.OrderId
                };

                orderTotal = shoppingCartItem.Amount * shoppingCartItem.Album.Price;
                _appDbContext.OrderDetails.Add(orderDetail);
            }

            order.OrderTotal += orderTotal;

            _appDbContext.SaveChanges();
        }

    }
}