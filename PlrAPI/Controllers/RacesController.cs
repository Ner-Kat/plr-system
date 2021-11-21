using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlrAPI.Models;
using PlrAPI.Models.Database;
using PlrAPI.Models.InputCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlrAPI.Systems;
using System.Text.Json;

namespace PlrAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RacesController : ControllerBase
    {
        private ApplicationContext _db;
        private JsonSerializerOptions _jsonOptions;

        public RacesController(ApplicationContext appContext, IPlrJsonOptions plrJsonOptions)
        {
            _db = appContext;
            _jsonOptions = plrJsonOptions.GetJsonOptions();
        }


        // Основные методы API

        [HttpGet]
        public JsonResult Get(int id)
        {
            var data = _db.Races.Where(r => r.Id == id).Select(r => new { r.Id, r.Name, r.Desc }).FirstOrDefault();

            return new JsonResult(data, _jsonOptions);
        }

        [HttpGet]
        public JsonResult List(int? count, int? from = 0)
        {
            if (count.HasValue)
            {
                var data = _db.Races.Select(r => new { r.Id, r.Name })
                    .Skip(from.Value).Take(count.Value).ToList();
                return new JsonResult(data, _jsonOptions);
            }
            else
            {
                var data = _db.Races.Select(r => new { r.Id, r.Name })
                    .Skip(from.Value).ToList();
                return new JsonResult(data, _jsonOptions);
            }
        }

        [HttpGet]
        public JsonResult Find(string name)
        {
            var data = (from race in _db.Races
                        where race.Name.ToLower().Contains(name.ToLower())
                        select new { race.Id, race.Name }).ToList();

            return new JsonResult(data, _jsonOptions);
        }

        [Authorize(Policy = "ForEditors")]
        [HttpPost]
        public IActionResult Add(InputRace race)
        {
            try
            {
                _db.Races.Add(race.ToRace());
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
        public IActionResult Change(InputRace race)
        {
            try
            {
                Race oldRace = _db.Races.Where(r => r.Id == race.Id).FirstOrDefault();
                race.WriteIn(oldRace);
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

        [HttpGet]
        public JsonResult SortedList(int? count, int? from = 0)
        {
            if (count.HasValue)
            {
                var data = _db.Races.OrderBy(r => r.Name).Select(r => new { r.Id, r.Name })
                    .Skip(from.Value).Take(count.Value).ToList();
                return new JsonResult(data, _jsonOptions);
            }
            else
            {
                var data = _db.Races.OrderBy(r => r.Name).Select(r => new { r.Id, r.Name })
                    .Skip(from.Value).ToList();
                return new JsonResult(data, _jsonOptions);
            }
        }
    }
}
