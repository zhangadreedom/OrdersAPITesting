using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entities
{
    public class Orders
    {
        public Guid OrderId { get; private set; }
        public string CustomerName { get; private set; } = default!;
        public DateTimeOffset CreatedAt { get; private set; }

        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        private Orders() { } // EF

        public Orders(Guid orderId, string customerName, DateTimeOffset createdAt, IEnumerable<OrderItem> items)
        {
            if (orderId == Guid.Empty) throw new ArgumentException("OrderId is empty");
            if (string.IsNullOrWhiteSpace(customerName)) throw new ArgumentException("CustomerName required");
            var list = (items ?? throw new ArgumentNullException(nameof(items))).ToList();
            if (list.Count == 0) throw new ArgumentException("Order item is empty!");

            OrderId = orderId;
            CustomerName = customerName.Trim();
            CreatedAt = createdAt;
            _items.AddRange(list);
        }
    }
}
