using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Infrastructure;
using Hotel.Infrastructure.Models;

namespace Hotel.MVC.Controllers
{
    public class HotelsController : Controller
    {
        private readonly HotelContext _context;

        public HotelsController(HotelContext context)
        {
            _context = context;
        }

        // GET: Hotels
        public async Task<IActionResult> Index()
        {
            return _context.Hotels != null ?
                        View(await _context.Hotels.ToListAsync()) :
                        Problem("Entity set 'HotelContext.Hotels'  is null.");
        }

        // GET: Hotels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotelModel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelModel == null)
            {
                return NotFound();
            }

            return View(hotelModel);
        }

        // GET: Hotels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,City,Stars,Id")] HotelModel hotelModel)
        {
            if (ModelState.IsValid)
            {
                hotelModel.Id = Guid.NewGuid();
                _context.Add(hotelModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotelModel);
        }

        // GET: Hotels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotelModel = await _context.Hotels.FindAsync(id);
            if (hotelModel == null)
            {
                return NotFound();
            }
            return View(hotelModel);
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,City,Stars,Id")] HotelModel hotelModel)
        {
            if (id != hotelModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotelModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelModelExists(hotelModel.Id))
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
            return View(hotelModel);
        }

        // GET: Hotels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotelModel = await _context.Hotels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotelModel == null)
            {
                return NotFound();
            }

            return View(hotelModel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Hotels == null)
            {
                return Problem("Entity set 'HotelContext.Hotels'  is null.");
            }
            var hotelModel = await _context.Hotels.FindAsync(id);
            if (hotelModel != null)
            {
                _context.Hotels.Remove(hotelModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelModelExists(Guid id)
        {
            return (_context.Hotels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
