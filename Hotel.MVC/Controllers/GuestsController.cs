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
    public class GuestsController : Controller
    {
        private readonly HotelContext _context;

        public GuestsController(HotelContext context)
        {
            _context = context;
        }

        // GET: Guests
        public async Task<IActionResult> Index()
        {
              return _context.Guests != null ? 
                          View(await _context.Guests.ToListAsync()) :
                          Problem("Entity set 'HotelContext.Guests'  is null.");
        }

        // GET: Guests/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            var guestModel = await _context.Guests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (guestModel == null)
            {
                return NotFound();
            }

            return View(guestModel);
        }

        // GET: Guests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Guests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,PhoneNumber,Id")] GuestModel guestModel)
        {
            if (ModelState.IsValid)
            {
                guestModel.Id = Guid.NewGuid();
                _context.Add(guestModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(guestModel);
        }

        // GET: Guests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            var guestModel = await _context.Guests.FindAsync(id);
            if (guestModel == null)
            {
                return NotFound();
            }
            return View(guestModel);
        }

        // POST: Guests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FullName,PhoneNumber,Id")] GuestModel guestModel)
        {
            if (id != guestModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guestModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuestModelExists(guestModel.Id))
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
            return View(guestModel);
        }

        // GET: Guests/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Guests == null)
            {
                return NotFound();
            }

            var guestModel = await _context.Guests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (guestModel == null)
            {
                return NotFound();
            }

            return View(guestModel);
        }

        // POST: Guests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Guests == null)
            {
                return Problem("Entity set 'HotelContext.Guests'  is null.");
            }
            var guestModel = await _context.Guests.FindAsync(id);
            if (guestModel != null)
            {
                _context.Guests.Remove(guestModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuestModelExists(Guid id)
        {
          return (_context.Guests?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
