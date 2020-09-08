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
using Microsoft.AspNetCore.Http;

namespace HotelWebApplication.Controllers
{
    public class ImagesController : Controller
    {
        // storing the images/files on the server file system, not in the DB
        private readonly IGenericHotelService<Image> _iamgeService;

        public ImagesController(IGenericHotelService<Image> imageService)
        {
            _iamgeService = imageService;
        }

        // GET: Images
        // Fetch all rows of the model entity's table in the DB
        // and display it as a list
        public async Task<IActionResult> Index()
        {
            return View(await _iamgeService.GetAllItemsAsync());
        }

        // GET: Images
        public async Task<IActionResult> GetAllImagesJson()
        {
            var images = await _iamgeService.GetAllItemsAsync();
            return PartialView("GetAllImagesPartial", images);
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = _iamgeService.GetItemByIdAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: Images/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<IFormFile> files)
        {
            var result = await _iamgeService.AddImagesAsync(files);
            var addedImages = new List<string>();

            foreach (var image in result.AddedImages)
            {
                addedImages.Add(image.Name + " Added successfully.");
            }

            ViewData["AddedImages"] = addedImages;
            ViewData["UploadErrors"] = result.UploadErrors;

            return View();

        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var image = await _iamgeService.GetItemByIdAsync(id);

            await _iamgeService.RemoveImageAsync(image);
            return RedirectToAction(nameof(Index));
        }


    }
}
