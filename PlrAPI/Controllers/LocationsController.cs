using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlrAPI.Models;
using PlrAPI.Models.Database;
using Microsoft.AspNetCore.Authorization;
using PlrAPI.Models.InputCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PlrAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        private ApplicationContext _db;
        private ILogger<Startup> _logger;

        public LocationsController(ApplicationContext appContext, ILogger<Startup> logger)
        {
            _db = appContext;
            _logger = logger;
        }


        // Основные методы API

        [HttpGet]
        public JsonResult Get(int id)
        {
            var data = (from loc in _db.Locations.Include(loc => loc.ParentLoc)
                        where loc.Id == id
                        select new { loc.Id, loc.Name, loc.Desc, ParentLoc = loc.ParentLoc.Name, loc.ParentLocId }).FirstOrDefault();

            return new JsonResult(data);
        }

        [HttpGet]
        public JsonResult List(int? count, int? from = 0)
        {
            if (count.HasValue)
            {
                var data = _db.Locations.Select(loc => new { loc.Id, loc.Name })
                    .Skip(from.Value).Take(count.Value).ToList();
                return new JsonResult(data);
            }
            else
            {
                var data = _db.Locations.Select(loc => new { loc.Id, loc.Name })
                    .Skip(from.Value).ToList();
                return new JsonResult(data);
            }
        }

        [HttpGet]
        public JsonResult Find(string name)
        {
            var data = (from loc in _db.Locations
                        where loc.Name.ToLower().Contains(name.ToLower())
                        select new { loc.Id, loc.Name }).ToList();

            return new JsonResult(data);
        }

        [Authorize(Policy = "ForEditors")]
        [HttpPost]
        public IActionResult Add(InputLocation loc)
        {
            try
            {
                _db.Locations.Add(loc.ToLocation());
                _db.SaveChanges();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Policy = "ForEditors")]
        [HttpPost]
        public IActionResult Change(InputLocation loc)
        {
            try
            {
                Location oldLoc = _db.Locations.Where(r => r.Id == loc.Id).FirstOrDefault();
                loc.WriteIn(oldLoc);
                _db.SaveChanges();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Policy = "ForEditors")]
        [HttpGet]
        public IActionResult Remove(int id)
        {
            try
            {
                Location loc = new Location() { Id = id };
                _db.Attach(loc);
                _db.Remove(loc);
                _db.SaveChanges();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }


        // Дополнительные методы API

        [HttpGet]
        public JsonResult SortedList(int? count, int? from = 0)
        {
            if (count.HasValue)
            {
                var data = _db.Locations.OrderBy(loc => loc.Name).Select(loc => new { loc.Id, loc.Name })
                    .Skip(from.Value).Take(count.Value).ToList();
                return new JsonResult(data);
            }
            else
            {
                var data = _db.Locations.OrderBy(loc => loc.Name).Select(loc => new { loc.Id, loc.Name })
                    .Skip(from.Value).ToList();
                return new JsonResult(data);
            }
        }

        [HttpGet]
        public JsonResult Sublocations(int id)
        {
            var data = _db.Locations.Where(loc => loc.ParentLocId == id)
                .Select(loc => new { loc.Id, loc.Name }).ToList();

            return new JsonResult(data);
        }

        [HttpGet]
        public JsonResult RootLocations()
        {
            var data = _db.Locations.Where(loc => !loc.ParentLocId.HasValue)
                .Select(loc => new { loc.Id, loc.Name }).ToList();

            return new JsonResult(data);
        }

    }
}
