using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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
                return new JsonResult(races.TakeWhile((r, index) => index >= from && index < from + count).ToList());
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

        [Authorize(Policy = "ForEditors")]
        [HttpPost]
        public IActionResult Add(Race race)
        {
            try
            {
                _db.Races.Add(race);
                _db.SaveChanges();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public JsonResult GetRace(int id)
        {
            return new JsonResult(_db.Races.Where(r => r.Id == id).FirstOrDefault());
        }

        [HttpGet]
        public JsonResult GetRaceByName(string name)
        {
            return new JsonResult(_db.Races.Where(r => r.Name == name).ToList());
        }

        [Authorize(Policy = "ForEditors")]
        [HttpGet]
        public IActionResult Remove(Race race)
        {
            try
            {
                _db.Remove(race);
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
        public IActionResult RemoveById(int id)
        {
            try
            {
                Race race = new Race() { Id = id };
                _db.Attach(race);
                _db.Remove(race);
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
        public IActionResult Change(Race race)
        {
            try
            {
                Race oldRace = _db.Races.Where(r => r.Id == race.Id).FirstOrDefault();
                oldRace.Name = race.Name;
                oldRace.Desc = race.Desc;
                _db.SaveChanges();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
