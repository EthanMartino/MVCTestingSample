using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using MVCTestingSample.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCTestingSample.Models
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public EFProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a product to the data store
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Task AddProductAsync(Product p)
        {
            _context.Add(p);
            return _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a product from the data store
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Task DeleteProductAsync(Product p)
        {
            _context.Remove(p);
            return _context.SaveChangesAsync();
        }

        public Task<List<Product>> GetAllProductsAsync() 
        {
            return _context.products.OrderBy(p => p.Name).ToListAsync();
        }

        public Task<Product> GetProductByIdAsync(int id)
        {
            return _context.products.FindAsync(id).AsTask();
            //return _context.products
            //        .Where(p => p.ProductId == id)
            //        .SingleOrDefaultAsync();
        }

        public Task UpdateProductAsync(Product p)
        {
            _context.Entry(p).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }
    }
}
