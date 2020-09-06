using HotelWebApplication.Data;
using HotelWebApplication.Models;
using HotelWebApplication.ViewModels;
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

        // Controller specific methods

        /// <summary>
        /// In the Room Index view - return a list of all rooms
        /// present in the hotel, as well as a list of all the room types
        /// so we can sort rooms based on the room type category
        /// </summary>
        /// <returns></returns>
        public RoomsAdminIndexViewModel GetAllRoomsAndRoomTypes()
        {
            var rooms = _dbContext.Rooms.ToList();
            var roomTypes = _dbContext.RoomTypes.ToList();

            var RoomsAdminIndexViewModel = new RoomsAdminIndexViewModel
            {
                Rooms = rooms,
                RoomTypes = roomTypes
            };

            return RoomsAdminIndexViewModel;
        }

        // return an array of room types
        public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync()
        {
            return await _dbContext.RoomTypes.ToArrayAsync();
        }

        // return all rooms with a room type
        public IEnumerable<Room> GetAllRooms()
        {
            return _dbContext.Rooms.Include(x => x.RoomType);
        }

    }
}
