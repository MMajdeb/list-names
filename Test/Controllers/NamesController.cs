using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Test.Models;

namespace Test.Controllers
{
    public class NamesController : Controller
    {
        private List<Names> listName;
        private IMemoryCache _cache;
        private readonly string _cashKey;

        /// <summary>
        /// Constructer
        /// </summary>
        /// <param name="memoryCache">inject MemoryCache</param>
        public NamesController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _cashKey = "Names";
        }

        /// <summary>
        /// Index page, show the names list
        /// </summary>
        /// <returns>index view, list of names</returns>
        public IActionResult Index()
        {
            if (!_cache.TryGetValue(_cashKey, out listName))
            {
                listName = new List<Names>();
            }
            return View(listName);
        }

        /// <summary>
        /// Show the form to create new Name
        /// </summary>
        /// <returns>create view</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create new Name and add it to list
        /// </summary>
        /// <param name="model">name object</param>
        /// <returns>redirect to Index if all done, if not: return the same view and shows errors</returns>
        [HttpPost]
        public ActionResult Create(Names model)
        {
            if (ModelState.IsValid)
            {
                if (!_cache.TryGetValue(_cashKey, out listName))
                {
                    listName = new List<Names>();
                }

                listName.Add(model);
                _cache.Set(_cashKey, listName);

                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        /// <summary>
        /// Filter list of item
        /// </summary>
        /// <param name="IdMin">min value of filter</param>
        /// <param name="IdMax">max value of filter</param>
        /// <returns>A partial view which content filred list</returns>
        [HttpPost]
        public ActionResult Filter(int IdMin, int IdMax)
        {
            if (!_cache.TryGetValue(_cashKey, out listName))
            {
                listName = new List<Names>();
            }

            var listNameFiltred = listName.Where(x => x.Id >= IdMin && x.Id <= IdMax || (IdMin == 0 && IdMax == 0)).ToList();

            return PartialView("_FilteredList", listNameFiltred);
        }
    }
}