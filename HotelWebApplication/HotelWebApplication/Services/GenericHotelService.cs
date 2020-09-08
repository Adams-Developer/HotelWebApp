using ByteSizeLib;
using HotelWebApplication.Data;
using HotelWebApplication.Models;
using HotelWebApplication.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IHostingEnvironment _hostingEnvironment;

        public GenericHotelService(ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            DbSet = dbContext.Set<TEnitity>();
            _hostingEnvironment = hostingEnvironment;
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
            DbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        // Edit Item
        public async Task EditItemAsync(TEnitity entity)
        {
            DbSet.Update(entity);
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

        /// <summary>
        /// return an array of room types
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync()
        {
            return await _dbContext.RoomTypes.ToArrayAsync();
        }

        /// <summary>
        /// return all rooms with a room type
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Room> GetAllRooms()
        {
            return _dbContext.Rooms.Include(x => x.RoomType);
        }

        //FeaturesController Section

        /// <summary>
        /// 
        /// </summary>
        /// <param name="featureId"></param>
        /// <returns></returns>
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

        // RoomsController Section

        /// <summary>
        /// Take in a Room entity and return a list of SelectedRoomFeatureViewModel 
        /// This is a boolean property specifying if that particular feature
        /// is related to the room entity in question
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Handle saving/updating the features for a particular room
        /// </summary>
        /// <param name="room"></param>
        /// <param name="selectedFeatureId"></param>
        public void UpdateRoomFeaturesList(Room room, int[] selectedFeatureId)
        {
            var previouslySelectedFeatures = _dbContext.RoomFeature
                .Where(x => x.RoomId == room.Id);

            _dbContext.RoomFeature.RemoveRange(previouslySelectedFeatures);
            _dbContext.SaveChanges();

            if (selectedFeatureId != null)
            {
                foreach (var featureId in selectedFeatureId)
                {
                    var allFeatureIds = new HashSet<int>(_dbContext.Features.Select(x => x.Id));
                    if (allFeatureIds.Contains(featureId))
                    {
                        _dbContext.RoomFeature.Add(new RoomFeature
                        {
                            FeatureId = featureId,
                            RoomId = room.Id
                        });
                    }
                }

                _dbContext.SaveChanges();
                
            }
        }

        // ImagesController
        public async Task<AddImagesViewModel> AddImagesAsync(List<IFormFile> files)
        {
            var uploadErrors = new List<string>();
            var addedImages = new List<Image>();
            var imagesFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

            foreach (var formFile in files)
            {
                var _ext = Path.GetExtension(formFile.FileName).ToLower(); // file extension

                if (formFile.Length > 0 && formFile.Length < 1000000)
                {
                    if (!(_ext == ".jpg" || _ext == ".png" || _ext == ".gif" || _ext == ".jpeg"))
                    {
                        uploadErrors.Add("The file \"" + formFile.FileName + "\" could not be uploaded because it has a bad extension -->\"" + _ext + "\"");
                        continue;
                    }

                    string newFileName;

                    var exisitingFilePath = Path.Combine(imagesFolder, formFile.FileName);
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(formFile.FileName);

                    for(var count = 1; File.Exists(exisitingFilePath) == true; count++)
                    {
                        fileNameWithoutExtension = fileNameWithoutExtension + "(" + count.ToString() + ")";

                        var updatedFileName = fileNameWithoutExtension + _ext;
                        var updatedFilePath = Path.Combine(imagesFolder, updatedFileName);

                        exisitingFilePath = updatedFilePath;
                    }

                    newFileName = fileNameWithoutExtension + _ext;

                    var filePath = Path.Combine(imagesFolder, newFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    var image = new Image
                    {
                        Name = newFileName,
                        Size = ByteSize.FromBytes(formFile.Length).ToString(),
                        ImageUrl = "~/images/" + newFileName,
                        FilePath = filePath
                    };
                    addedImages.Add(image);

                }
                else
                {
                    uploadErrors.Add(formFile.FileName + "Size is not valid. -->(" + ByteSize.FromBytes(formFile.Length).ToString() + ")... Upload a file less than 1MB");
                }
            }

            _dbContext.Images.AddRange(addedImages);
            _dbContext.SaveChanges();

            var result = new AddImagesViewModel
            {
                AddedImages = addedImages,
                UploadErrors = uploadErrors
            };
            return result;
        }

        public async Task RemoveImageAsync(Image image)
        {
            File.Delete(image.FilePath);

            _dbContext.Images.Remove(image);
            await _dbContext.SaveChangesAsync();
        }



    }
}
