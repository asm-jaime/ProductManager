using System;
using System.Collections.Generic;

namespace ProductManager
{
    public class Order
    {
        public int UserId { get; set; }
        public List<OrderItem> Items { get; set; }
        public DateTime OrderDate { get; set; }

        public class OrderItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }

    }
}
