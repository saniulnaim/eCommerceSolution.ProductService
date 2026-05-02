using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccessLayer.RepositoryContacts
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();

        Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> condition);

        Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> condition);

        Task<Product?> AddProduct(Product product);

        Task<bool> DeleteProduct(Guid productID);

        Task<Product?> UpdateProduct(Product product);

    }
}
