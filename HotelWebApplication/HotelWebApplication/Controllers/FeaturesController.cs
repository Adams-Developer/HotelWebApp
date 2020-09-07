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
    public class FeaturesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericHotelService<Feature> _featureService;

        public FeaturesController(ApplicationDbContext context, IGenericHotelService<Feature> featureService)
        {
            _context = context;
            _featureService = featureService;
        }

        // GET: Features
        public async Task<IActionResult> Index()
        {
            return View(await _featureService.GetAllItemsAsync());
        }

        // GET: Features/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feature = await _featureService.GetItemByIdAsync(id);

            if (feature == null)
            {
                return NotFound();
            }

            return View(feature);
        }

        // GET: Features/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Features/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Icon")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                await _featureService.CreateItemAsync(feature);
                return RedirectToAction(nameof(Index));
            }
            return View(feature);
        }

        // GET: Features/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feature = await _featureService.GetItemByIdAsync(id);

            var rooms = _featureService.GetAllRoomsWithFeature(id);

            ViewData["RoomsWithFeature"] = rooms;

            if (feature == null)
            {
                return NotFound();
            }
            return View(feature);
        }

        // POST: Features/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Icon")] Feature feature)
        {
            if (id != feature.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _featureService.CreateItemAsync(feature);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_featureService.GetItemByIdAsync(id) == null)
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
            return View(feature);
        }

        // GET: Features/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feature = await _featureService.GetItemByIdAsync(id);
               
            if (feature == null)
            {
                return NotFound();
            }

            return View(feature);
        }

        // POST: Features/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feature = await _featureService.GetItemByIdAsync(id);

            await _featureService.DeleteItemAsync(feature);
            return RedirectToAction(nameof(Index));
        }

       
    }
}
