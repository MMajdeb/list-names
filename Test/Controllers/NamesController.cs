using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using Test.Models;

namespace Test.Controllers
{
    public class NamesController : Controller
    {
        private List<Names> listName;/* = new List<Names>
        {
            new Names{Id=1,Name="Mohamed" },
            new Names{Id=2,Name="Zakaria" },
        };*/
        private IMemoryCache _cache;
        private readonly string _cashKey;

        public NamesController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _cashKey = "Names";
        }

        public IActionResult Index()
        {
            if (!_cache.TryGetValue(_cashKey, out listName))
            {
                listName = new List<Names>();
            }
            return View(listName);
        }

        public ActionResult Create()
        {
            return View();
        }

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

        [HttpPost]
        public ActionResult Filter(int IdMin, int IdMax)
        {
            if (!_cache.TryGetValue(_cashKey, out listName))
            {
                listName = new List<Names>();
            }
            var listNameFiltred = listName.Where(x => x.Id >= IdMin && x.Id <= IdMax || (IdMin == 0 && IdMax == 0)).ToList();

            return PartialView("FilteredList", listNameFiltred);
        }
    }
}