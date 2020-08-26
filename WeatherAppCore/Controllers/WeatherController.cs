using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Atp;
using NPOI.XSSF.UserModel;
using WeatherAppCore.Data;
using WeatherAppCore.Models;
using X.PagedList;

namespace WeatherAppCore.Controllers
{
    public class WeatherController : Controller
    {
        private readonly WeatherAppCoreContext _context;

        public WeatherController(WeatherAppCoreContext context)
        {
            _context = context;
        }

        // GET: Weather
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.WeatherData.ToListAsync());
        //}
        //public ViewResult Index(string sortOrder, string currentFilter, string searchString, string searchString1, int? pageNumber)
        //{
        //    ViewData["CurrentSort"] = sortOrder;
        //    ViewData["TSortParm"] = String.IsNullOrEmpty(sortOrder) ? "T_desc" : "";
        //    ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

        //    if (searchString != null)
        //    {
        //        pageNumber = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    ViewData["CurrentFilter"] = searchString;
        //    ViewData["CurrentFilter1"] = searchString1;
        //    var weathers = from w in _context.WeatherData
        //                   select w;
        //    if (!String.IsNullOrEmpty(searchString1))
        //    {
        //        weathers = weathers.Where(s => //s.Date.Month == DateTime.Parse(searchString).Month || 
        //                                s.Date.Year == DateTime.Parse(searchString1).Year);
        //    }
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        weathers = weathers.Where(s => s.Date.Month == DateTime.Parse(searchString).Month
        //                               && s.Date.Year == DateTime.Parse(searchString1).Year);
        //    }
        //    switch (sortOrder)
        //    {
        //        case "T_desc":
        //            weathers = weathers.OrderByDescending(s => s.T);
        //            break;
        //        case "Date":
        //            weathers = weathers.OrderBy(s => s.Date).OrderBy(s => s.Time);
        //            break;
        //        case "date_desc":
        //            weathers = weathers.OrderByDescending(s => s.Date).OrderBy(s => s.Time);
        //            break;
        //        default:
        //            weathers = weathers.OrderBy(s => s.Id);
        //            break;
        //    }

        //    int pageSize = 30;
        //    return View(//await PaginatedList <WeatherData>.CreateAsync(
        //        weathers.ToArray(pageNumber ?? 1, pageSize));
        //}
        //new Index
        /// <summary>
        /// Index v ViewBag
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page, string searchString0, string searchString1)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "T" ? "-T" : "T";//String.IsNullOrEmpty(sortOrder) ? 
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.CurrentFilter0 = !String.IsNullOrWhiteSpace(searchString0) ? searchString0 : "";
            ViewBag.CurrentFilter1 = !String.IsNullOrWhiteSpace(searchString1) ? searchString1 : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var weather = from s in _context.WeatherData
                           select s;
            if (!String.IsNullOrEmpty(searchString0))
            {
                DateTime dtYear = default, month;
                if (DateTime.TryParse(searchString0,out dtYear))                    
                    weather = weather.Where(s => s.Date.Year == dtYear.Date.Year);
                //|| s.FirstMidName.Contains(searchString)
                if (!String.IsNullOrEmpty(searchString1))
                    if (DateTime.TryParse(searchString1, out month))
                        weather = weather.Where(s => s.Date.Year == dtYear.Year 
                                                    && s.Date.Month == month.Month);


            }
            switch (sortOrder)
            {
                case "T":
                    weather = weather.OrderBy(s => s.T);
                    break;
                case "-T":
                    weather = weather.OrderByDescending(s => s.T);
                    break;
                case "Date":
                    weather = weather.OrderBy(s => s.Date);
                    break;
                case "date_desc":
                    weather = weather.OrderByDescending(s => s.Date);
                    break;
                default:  // id ascending 
                    weather = weather.OrderBy(s => s.Id);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(weather.ToPagedList<WeatherData>(pageNumber, pageSize));
        }
        // GET: Weather/Details/5
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

        // GET: Weather/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Weather/Create
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
        // GET: Weather/Import
        public IActionResult Import()
        {
            return View();
        }

        // POST: Weather/Import
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import([Bind("Id,Date,Time,T,Humidity,Td,Pressure,Direction,Speed,Cloudiness,h,VV,Comment")] List<WeatherData> ListweatherData)
        {
            List<WeatherData> weatherData = new List<WeatherData>();
            foreach (var file in Request.Form.Files)
            {
                XSSFWorkbook xSSFWorkbook = new XSSFWorkbook(
                    file.OpenReadStream()
                );
                for(int s = 0; s < xSSFWorkbook.NumberOfSheets; s++)
                {
                    for (int r = 4; r < xSSFWorkbook.GetSheetAt(s).LastRowNum; r++)
                    {
                        WeatherData weatherData1 = new WeatherData() { 
                            //Date = xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(0).DateCellValue,
                            //Time = TimeSpan.Parse(xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(1).StringCellValue),
                            //T = (decimal)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(2).NumericCellValue,
                            //Humidity = (int)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(3).NumericCellValue,
                            //Td = (decimal)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(4).NumericCellValue,
                            //Pressure = (int)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(5).NumericCellValue,
                            //Direction = xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(6).StringCellValue,
                            //Speed = (int)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(7).NumericCellValue,
                            //Cloudiness = (int)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(8).NumericCellValue,
                            //h = (int)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(9).NumericCellValue,
                            //VV = (int)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(10).NumericCellValue,
                            //Comment = xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(11).StringCellValue
                        };
                        if(xSSFWorkbook.GetSheetAt(s).GetRow(r).LastCellNum == 12)
                        {
                            weatherData1.Date = DateTime.Parse(xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(0).StringCellValue);
                            weatherData1.Time = TimeSpan.Parse(xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(1).StringCellValue);
                            weatherData1.T = (decimal)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(2).NumericCellValue;
                            weatherData1.Humidity = (int)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(3).NumericCellValue;
                            weatherData1.Td = (decimal)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(4).NumericCellValue;
                            weatherData1.Pressure = (int)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(5).NumericCellValue;
                            weatherData1.Direction = xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(6).StringCellValue;
                            if(xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(7).CellType != NPOI.SS.UserModel.CellType.String)
                                weatherData1.Speed = (int)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(7).NumericCellValue;
                            if(xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(8).CellType != NPOI.SS.UserModel.CellType.String)
                                weatherData1.Cloudiness = (int)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(8).NumericCellValue;
                            if(xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(9).CellType != NPOI.SS.UserModel.CellType.String)
                                weatherData1.h = (int)xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(9).NumericCellValue;
                            if (xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(10).CellType != NPOI.SS.UserModel.CellType.String)
                                weatherData1.VV = (int)(xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(10).NumericCellValue);
                            if (xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(11) != null)
                                weatherData1.Comment = xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(11).StringCellValue;
                            //weatherData1.Date = xSSFWorkbook.GetSheetAt(s).GetRow(r).GetCell(10).DateCellValue;

                        }
                        weatherData.Add(weatherData1);
                    }
                }
            }
            if (ModelState.IsValid)
            {
                _context.AddRange(weatherData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(weatherData);
        }

        // GET: Weather/Edit/5
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

        // POST: Weather/Edit/5
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

        // GET: Weather/Delete/5
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

        // POST: Weather/Delete/5
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
