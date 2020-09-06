using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelWebApplication.Data;
using HotelWebApplication.Models;
using HotelWebApplication.Services;

namespace HotelWebApplication.Controllers
{
    public class RoomTypesController : Controller
    {
        private readonly IGenericHotelService<RoomType> _roomTypeService;

        public RoomTypesController(IGenericHotelService<RoomType> roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }

        // GET: RoomTypes
        // GetAllItemsAsync() action now means GetAllRoomTypesAsync()
        public async Task<IActionResult> Index()
        {
            return View(await _roomTypeService.GetAllItemsAsync());
        }

        // GET: RoomTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _roomTypeService.GetItemByIdAsync(id);

            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        // GET: RoomTypes/Create
        // Serves the form(view)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoomTypes/Create
        // Processes the form submission request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, BasePrice, Description, ImageUrl")] RoomType roomType)
        {
            if (ModelState.IsValid)
            {
                await _roomTypeService.CreateItemAsync(roomType);
                return RedirectToAction(nameof(Index));
            }
            return View(roomType);
        }

        // GET: RoomTypes/Edit/5
        // Serves the form(view)
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _roomTypeService.GetItemByIdAsync(id);

            if (roomType == null)
            {
                return NotFound();
            }
            return View(roomType);
        }

        // POST: RoomTypes/Edit/5
        // Processes the form submission request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Name, BasePrice, Description, ImageUrl")] RoomType roomType, int? id)
        {
            if (id != roomType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _roomTypeService.EditItemAsync(roomType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_roomTypeService.GetItemByIdAsync(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(roomType);
        }

        // GET: RoomTypes/Delete/5
        // Serves the form(view)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _roomTypeService.GetItemByIdAsync(id);

            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        // POST: RoomTypes/Delete/5
        // Processes the form submission request
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var roomType = await _roomTypeService.GetItemByIdAsync(id);

            await _roomTypeService.DeleteItemAsync(roomType);
            return RedirectToAction(nameof(Index));
        }

    }
}
