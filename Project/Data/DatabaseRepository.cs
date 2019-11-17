﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Data
{
    public class DatabaseRepository:IDatabaseRepository
    {
        private readonly DatabaseContext _ctx;
        private readonly ILogger<DatabaseRepository> _logger;
        public DatabaseRepository(DatabaseContext ctx,ILogger<DatabaseRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                return _ctx.Orders
                       .Include(o => o.Items)
                       .ThenInclude(i => i.Product)
                       .ToList();
            }
            else
            {
                return _ctx.Orders
                      .ToList();
            }
           
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("GetAllProducts was called");
                return _ctx.Products
                           .OrderBy(p => p.Title)
                           .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products: {ex} ");
                return null;
            }
        }

        public Order GetOrderById(int id)
        {
            return _ctx.Orders
                       .Include(o => o.Items)
                       .ThenInclude(i => i.Product)
                       .Where(o => o.Id == id)
                       .FirstOrDefault();
        }

        public IEnumerable<Product>GetProductsByCategory(string category)
        {
            return _ctx.Products
                       .Where(p => p.Category == category)
                       .ToList();
        }
       

        public bool SaveAll()
        {
            return _ctx.SaveChanges()>0;
        }
    }
}
