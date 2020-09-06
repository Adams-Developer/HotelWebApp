using HotelWebApplication.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelWebApplication.Services
{
    // Service layer handles all interactions with the database
    public class GenericHotelService<TEnitity> : IGenericHotelService<TEnitity> where TEnitity : class
    {
        private readonly ApplicationDbContext _dbContext;
        protected DbSet<TEnitity> DbSet;

        public GenericHotelService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = dbContext.Set<TEnitity>();
        }

        // Add New Item
        public async Task CreateItemAsync(TEnitity entity)
        {
            DbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        // Delete Item
        public async Task DeleteItemAsync(TEnitity entity)
        {
            DbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        // Edit Item
        public async Task EditItemAsync(TEnitity entity)
        {
            DbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        // Get All Items
        public async Task<IEnumerable<TEnitity>> GetAllItemsAsync()
        {
            return await DbSet.ToArrayAsync();
        }

        // Get Single Item
        public async Task<TEnitity> GetItemByIdAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await DbSet.FindAsync(id);
        }

        // Search Item
        public async Task<IEnumerable<TEnitity>> SearchFor(Expression<Func<TEnitity, bool>> expression)
        {
            return await DbSet.Where(expression).ToArrayAsync();
        }
    }
}
