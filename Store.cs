using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductManager
{

    public class Store
    {
        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }

        /// <summary>
        /// Формирует строку со статистикой продаж продуктов
        /// Сортировка - по убыванию кол-ва проданных продуктов
        /// </summary>
        /// <param name="year">Год, за который подсчитывается статистика</param>
        public string GetProductStatistics(int year)
        {
            // Формат строки:
            // {№}) - {Название продукта} - {кол-во проданных единиц} item(s)\r\n
            //
            // Пример результирующей строки:
            //
            // 1) Product 3 - 1103 item(s)
            // 2) Product 1 - 800 item(s)
            // 3) Product 2 - 10 item(s)

            // TODO Реализовать логику получения и формирования требуемых данных  

            var products = Products.ToDictionary(product => product.Id, product => product);

            return (
                from product in (
                    from order in Orders
                    from item in order.Items
                    where order.OrderDate.Year == year
                    select new { Name = products[item.ProductId].Name, Quantity = item.Quantity }
                )
                group product by product.Name into productGroup
                select new { Name = productGroup.Key, Items = (from item in productGroup select item.Quantity).Sum() }
            )
            .Select((sales, indexer) => $"{indexer + 1}) {sales.Name} - {sales.Items} item(s)")
            .Aggregate((salesA, salesB) => $"{salesA}{Environment.NewLine}{salesB}");
        }

        /// <summary>
        /// Формирует строку со статистикой продаж продуктов по годам
        /// Сортировка - по убыванию годов.
        /// Выводятся все года, в которых были продажи продуктов
        /// </summary>
        public string GetYearsStatistics()
        {
            // Формат результата:
            // {Год} - {На какую сумму продано продуктов руб\r\n
            // Most selling: -{Название самого продаваемого продукта} (кол-во проданных единиц самого популярного продукта шт.)\r\n
            // \r\n
            //
            // Пример:
            //
            // 2021 - 630.000 руб.
            // Most selling: Product 1 (380 item(s))
            //
            // 2020 - 630.000 руб.
            // Most selling: Product 1 (380 item(s))
            //
            // 2019 - 130.000 руб.
            // Most selling: Product 3 (10 item(s))
            //
            // 2018 - 50.000 руб.
            // Most selling: Product 3 (5 item(s))

            // TODO Реализовать логику получения и формирования требуемых данных

            var products = Products.ToDictionary(product => product.Id, product => product);

            return
                (from product in (
                    from order in Orders
                    from item in order.Items
                    select new
                    {
                        Year = order.OrderDate.Year,
                        Name = products[item.ProductId].Name,
                        Quantity = item.Quantity,
                        PositionPrice = item.Quantity * products[item.ProductId].Price
                    }
                    )
                 orderby product.Year descending
                 group product by product.Year into productGroup
                 select new
                 {
                     Year = productGroup.Key,
                     TotalMoney = (from groupedProduct in productGroup select groupedProduct.PositionPrice).Sum(),
                     TopProduct = (
                         from productQuantity in (
                             from groupedProduct in productGroup
                             select new { Name = groupedProduct.Name, Quantity = groupedProduct.Quantity }
                         )
                         group productQuantity by productQuantity.Name into productQuantityGroup
                         select new
                         {
                             Name = productQuantityGroup.Key,
                             TotalQuantity = (from productQuantityGroupItem in productQuantityGroup select productQuantityGroupItem.Quantity).Sum(),
                         }
                     ).OrderBy(element => element.TotalQuantity).LastOrDefault()
                 }
                )
                .Select(salesNote => $"{salesNote.Year} - {salesNote.TotalMoney} руб.\r\nMost selling: {salesNote.TopProduct.Name} ({salesNote.TopProduct.TotalQuantity} item(s))")
                .Aggregate((noteA, noteB) => $"{noteA}\r\n\r\n{noteB}");
        }

    }

}
