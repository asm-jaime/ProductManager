using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductManager
{
    [TestFixture]
    public class OrderTest
    {
        [Test]
        public void ShouldGetOrderPrice()
        {
            var prices = TestDataFixture.GetProductList
                .ToDictionary(product => product.Id, product => product.Price);
            var order = TestDataFixture.GetOrderList.FirstOrDefault();
            order.GetPrice(prices).Should().Be(2000d);
        }
    }

}
