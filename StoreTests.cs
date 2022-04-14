using FluentAssertions;
using NUnit.Framework;
using ProductManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager
{
    [TestFixture]
    public class StoreTests
    {
        [TestCase(2020, ExpectedResult = "1) Product 1 - 3 item(s)\r\n2) Product 2 - 1 item(s)\r\n3) Product 3 - 1 item(s)")]
        [TestCase(2021, ExpectedResult = "1) Product 4 - 8 item(s)\r\n2) Product 5 - 2 item(s)")]
        public string ShouldProductStatisticsReturnReportOnYear(int year)
        {
            Store store = new Store() {
                Orders = TestDataFixture.GetOrderList,
                Products = TestDataFixture.GetProductList
            };

            return store.GetProductStatistics(year);
        }

        [Test]
        public void ShouldStoreGetYearsStatistics()
        {
            Store store = new Store() {
                Orders = TestDataFixture.GetOrderList,
                Products = TestDataFixture.GetProductList
            };
            
            store.GetYearsStatistics()
                .Should()
                .Be("2021 - 630.000 руб.\r\nMost selling: Product 1 (380 item(s))\r\n\r\n2020 - 630.000 руб.\r\nMost selling: Product 1 (380 item(s))");
        }
    }

    public static class TestDataFixture
    {
        public static List<Product> GetProductList => new List<Product>
            {
                new() { Id = 1, Name = "Product 1", Price = 1000d },
                new() { Id = 2, Name = "Product 2", Price = 3000d },
                new() { Id = 3, Name = "Product 3", Price = 10000d },
                new() { Id = 4, Name = "Product 4", Price = 100d },
                new() { Id = 5, Name = "Product 5", Price = 1d }
            };

        public static List<Order> GetOrderList => new List<Order>
            {
                new()
                {
                    UserId = 1,
                    OrderDate = DateTime.UtcNow.AddYears(-2),
                    Items = new List<Order.OrderItem>
                    {
                        new() { ProductId = 1, Quantity = 2 }
                    }
                },
                new()
                {
                    UserId = 3,
                    OrderDate = DateTime.UtcNow.AddYears(-2),
                    Items = new List<Order.OrderItem>
                    {
                        new() { ProductId = 1, Quantity = 1 },
                        new() { ProductId = 2, Quantity = 1 },
                        new() { ProductId = 3, Quantity = 1 }
                    }
                },
                new()
                {
                    UserId = 2,
                    OrderDate = DateTime.UtcNow.AddYears(-1),
                    Items = new List<Order.OrderItem>
                    {
                        new() { ProductId = 4, Quantity = 2 },
                    }
                },
                new()
                {
                    UserId = 2,
                    OrderDate = DateTime.UtcNow.AddYears(-1),
                    Items = new List<Order.OrderItem>
                    {
                        new() { ProductId = 4, Quantity = 6 },
                        new() { ProductId = 5, Quantity = 2 }
                    }
                },
            };
    }
}
