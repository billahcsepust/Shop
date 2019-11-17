using Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Data
{
    public interface IDatabaseRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
         bool SaveAll();
        IEnumerable<Order> GetAllOrders(bool includesItems);
        Order GetOrderById(int id);
        void AddEntity(object model);
    }
}
