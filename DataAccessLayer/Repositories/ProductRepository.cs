using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContacts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product?> AddProduct(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(Guid productID)
        {
            var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(x=> x.ProductID == productID);
            if (existingProduct == null) 
            {
                return false;
            }

            _dbContext.Products.Remove(existingProduct);
            int affectedRowsCount = await _dbContext.SaveChangesAsync();

            return affectedRowsCount > 0;
        }

        public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> condition)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(condition);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> condition)
        {
            return await _dbContext.Products.Where(condition).ToListAsync();
        }

        public async Task<Product?> UpdateProduct(Product product)
        {
            var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == product.ProductID);
            if (existingProduct == null)
            {
                return null;
            }

            _dbContext.Entry(existingProduct).CurrentValues.SetValues(product);
            await _dbContext.SaveChangesAsync();
            return existingProduct;
        }
    }
}
