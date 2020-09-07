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


        // RoomsController Section

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

        // FeaturesController Section

        public IEnumerable<Room> GetAllRoomsWithFeature(int? featureId)
        {
            var roomFeatures = _dbContext.RoomFeature
                .Include(x => x.Room)
                .Include(x => x.Room.RoomType)
                .Where(x => x.FeatureId == featureId);

            var selectedRooms = new List<Room>();

            foreach (var roomFeature in roomFeatures)
            {
                selectedRooms.Add(roomFeature.Room);
            }
            return selectedRooms;
        }


        // Take in a Room entity and return a list of SelectedRoomFeatureViewModel 
        // This is a boolean property specifying if that particular feature
        // is related to the room entity in question
        public List<SelectedRoomFeatureViewModel> PopulateSelectedFeaturesForRoom(Room room)
        {
            var viewModel = new List<SelectedRoomFeatureViewModel>();

            var allFeatures = _dbContext.Features;

            // If the room is a new room entity whose Id isnt set yet
            // set the Selected property to false
            if (room.Id == 0)
            {
                foreach (var feature in allFeatures)
                {
                    viewModel.Add(new SelectedRoomFeatureViewModel
                    {
                        FeatureId = feature.Id,
                        Feature = feature,
                        Selected = false
                    });
                    
                }
            }
            // if the condition fails, create a new HashSet of the rows,
            // selecting the FeatureId
            else 
            {
                var roomFeatures = _dbContext.RoomFeature.Where(x => x.RoomId == room.Id);
                var roomFeatureIds = new HashSet<int>(roomFeatures.Select(x => x.FeatureId));

                //Loop through all the features, 
                //create a viewModel for each instance of feature
                //checking if the featureId is present in the HashSet of roomFeatureIds
                //If present, set Selected property to true, else false
                foreach (var feature in allFeatures)
                {
                    viewModel.Add(new SelectedRoomFeatureViewModel
                    {
                        FeatureId = feature.Id,
                        Feature = feature,
                        Selected = roomFeatureIds.Contains(feature.Id)
                    });
                }
            }

            return viewModel;
        }



    }
}
