using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlrAPI.Models;
using PlrAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlrAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RacesController : ControllerBase
    {
        private ApplicationContext _db;

        public RacesController(ApplicationContext appContext)
        {
            _db = appContext;
        }

        [NonAction]
        public JsonResult GetList(IQueryable<Race> races, int? count, int? from)
        {
            if (count.HasValue)
            {
                return new JsonResult(races.Where(r => r.Id >= from).Take(count.Value).ToList());
            }

            return new JsonResult(races.ToList());
        }

        [HttpGet]
        public JsonResult List(int? count, int? from = 0)
        {
            return GetList(_db.Races, count, from);
        }

        [HttpGet]
        public JsonResult ListOrderedByNames(int? count, int? from = 0)
        {
            return GetList(_db.Races.OrderBy(r => r.Name), count, from);
        }

        [HttpPost]
        public IActionResult Add(Race race)
        {
            _db.Races.Add(race);
            _db.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public JsonResult GetRace(int id)
        {
            return new JsonResult(_db.Races.ElementAt(id));
        }

        [HttpGet]
        public JsonResult GetRaceByName(string name)
        {
            return new JsonResult(_db.Races.Where(r => r.Name == name).ToList());
        }

        [HttpGet]
        public IActionResult Remove(Race race)
        {
            _db.Remove(race);
            _db.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IActionResult RemoveById(int id)
        {
            Race race = new Race() { Id = id };
            _db.Attach(race);
            _db.Remove(race);
            _db.SaveChanges();

            return Ok();
        }
    }
}
