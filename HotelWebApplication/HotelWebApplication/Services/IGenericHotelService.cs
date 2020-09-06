﻿using HotelWebApplication.Models;
using HotelWebApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelWebApplication.Services
{
    public interface IGenericHotelService<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllItemsAsync();

        Task<TEntity> GetItemByIdAsync(int? id);

        Task<IEnumerable<TEntity>> SearchFor(Expression<Func<TEntity, bool>> expression);

        Task CreateItemAsync(TEntity entity);

        Task EditItemAsync(TEntity entity);

        Task DeleteItemAsync(TEntity entity);

        #region Specific Controller Methods
        // RoomsController
        RoomsAdminIndexViewModel GetAllRoomsAndRoomTypes();

        // RoomsController - Get RoomType entities
        Task<IEnumerable<RoomType>> GetAllRoomTypesAsync();

        // RoomsController - Get all rooms
        IEnumerable<Room> GetAllRooms();

        #endregion
    }
}
