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
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericHotelService<Room> _roomService;

        public RoomsController(IGenericHotelService<Room> roomService, ApplicationDbContext context)
        {
            _context = context;
            _roomService = roomService;
        }

        // GET: Rooms
        public IActionResult Index()
        {
            return View(_roomService.GetAllRoomsAndRoomTypes());
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = _roomService.GetAllRooms()
                .SingleOrDefault(x => x.Id == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            var RoomTypes = _roomService.GetAllRoomTypesAsync().Result;

            ViewData["RoomTypeId"] = new SelectList(RoomTypes, "Id", "Name");

            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,RoomTypeId,Price,Available,Description,MaximumGuests")] Room room)
        {
            if (ModelState.IsValid)
            {
                await _roomService.CreateItemAsync(room);
                return RedirectToAction(nameof(Index));
            }

            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _roomService.GetItemByIdAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            var RoomTypes = _roomService.GetAllRoomTypesAsync().Result;

            ViewData["RoomTypeId"] = new SelectList(RoomTypes, "Id", "Name");

            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,RoomTypeId,Price,Available,Description,MaximumGuests")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _roomService.EditItemAsync(room);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_roomService.GetItemByIdAsync(id) == null)
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

            var RoomTypes = _roomService.GetAllRoomTypesAsync().Result;

            ViewData["RoomTypeId"] = new SelectList(RoomTypes, "Id", "Id", room.RoomTypeId);

            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _roomService.GetItemByIdAsync(id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var roomType = await _roomService.GetItemByIdAsync(id);
            await _roomService.DeleteItemAsync(roomType);

            return RedirectToAction(nameof(Index));
        }

    }
}
