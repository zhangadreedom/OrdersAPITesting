using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        private OrderItem() { }

        public OrderItem(Guid productId, int quantity)
        {
            if (productId == Guid.Empty) throw new ArgumentException("ProductId is empty");
            if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));

            ProductId = productId;
            Quantity = quantity;
        }
    }
}
