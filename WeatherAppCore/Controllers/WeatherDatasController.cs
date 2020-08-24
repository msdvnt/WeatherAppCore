using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WeatherAppCore.Data;
using WeatherAppCore.Models;

namespace WeatherAppCore.Controllers
{
    public class WeatherDatasController : Controller
    {
        private readonly WeatherAppCoreContext _context;

        public WeatherDatasController(WeatherAppCoreContext context)
        {
            _context = context;
        }

        // GET: WeatherDatas
        public async Task<IActionResult> Index()
        {
            return View(await _context.WeatherData.ToListAsync());
        }

        // GET: WeatherDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weatherData = await _context.WeatherData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weatherData == null)
            {
                return NotFound();
            }

            return View(weatherData);
        }

        // GET: WeatherDatas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WeatherDatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Time,T,Humidity,Td,Pressure,Direction,Speed,Cloudiness,h,VV,Comment")] WeatherData weatherData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weatherData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(weatherData);
        }

        // GET: WeatherDatas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weatherData = await _context.WeatherData.FindAsync(id);
            if (weatherData == null)
            {
                return NotFound();
            }
            return View(weatherData);
        }

        // POST: WeatherDatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Time,T,Humidity,Td,Pressure,Direction,Speed,Cloudiness,h,VV,Comment")] WeatherData weatherData)
        {
            if (id != weatherData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weatherData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeatherDataExists(weatherData.Id))
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
            return View(weatherData);
        }

        // GET: WeatherDatas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weatherData = await _context.WeatherData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weatherData == null)
            {
                return NotFound();
            }

            return View(weatherData);
        }

        // POST: WeatherDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var weatherData = await _context.WeatherData.FindAsync(id);
            _context.WeatherData.Remove(weatherData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeatherDataExists(int id)
        {
            return _context.WeatherData.Any(e => e.Id == id);
        }
    }
}
