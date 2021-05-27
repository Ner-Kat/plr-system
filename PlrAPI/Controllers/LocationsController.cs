using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlrAPI.Models;
using PlrAPI.Models.Database;

namespace PlrAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private ApplicationContext _db;

        public LocationsController(ApplicationContext appContext)
        {
            _db = appContext;
        }

        [NonAction]
        public JsonResult GetList(IQueryable<Location> locs, int? count, int? from)
        {
            if (count.HasValue)
            {
                return new JsonResult(locs.TakeWhile((loc, index) => index >= from && index < from + count).ToList());
            }

            return new JsonResult(locs.ToList());
        }

        [HttpGet]
        public JsonResult List(int? count, int? from = 0)
        {
            return GetList(_db.Locations, count, from);
        }

        [HttpGet]
        public JsonResult ListOrderedByNames(int? count, int? from = 0)
        {
            return GetList(_db.Locations.OrderBy(r => r.Name), count, from);
        }

        [HttpPost]
        public IActionResult Add(Location loc)
        {
            try
            {
                _db.Locations.Add(loc);
                _db.SaveChanges();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public JsonResult GetLocation(int id)
        {
            return new JsonResult(_db.Locations.Where(loc => loc.Id == id).First());
        }

        [HttpGet]
        public JsonResult GetLocationByName(string name)
        {
            return new JsonResult(_db.Locations.Where(loc => loc.Name == name).ToList());
        }

        [HttpGet]
        public IActionResult Remove(Location loc)
        {
            try
            {
                _db.Remove(loc);
                _db.SaveChanges();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult RemoveById(int id)
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

        [HttpPost]
        public IActionResult Change(Location loc)
        {
            try
            {
                Location oldLoc = _db.Locations.Where(r => r.Id == loc.Id).First();
                oldLoc.Name = loc.Name;
                oldLoc.Desc = loc.Desc;
                oldLoc.Parent = loc.Parent;
                oldLoc.ParentLocationId = loc.ParentLocationId;
                oldLoc.Children = loc.Children;
                _db.SaveChanges();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public JsonResult GetRootLocations()
        {
            return new JsonResult(_db.Locations.Where(loc => !loc.ParentLocationId.HasValue));
        }

        [HttpGet]
        public JsonResult GetLocationChildren(Location loc)
        {
            return GetLocationChildrenById(loc.Id);

            /* Location parentLoc = _db.Locations.Where(dbLoc => dbLoc.Id == loc.Id).First();
            return new JsonResult(parentLoc.Children.ToList()); */
        }

        [HttpGet]
        public JsonResult GetLocationChildrenById(int id)
        {
            Location parentLoc = _db.Locations.Where(dbLoc => dbLoc.Id == id).First();
            return new JsonResult(parentLoc.Children.ToList());
        }
    }
}
