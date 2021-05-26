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

        [HttpGet]
        public JsonResult List(int? count, int? from = 0)
        {
            if (count.HasValue)
            {
                return new JsonResult(_db.Races.Where(r => r.Id >= from && r.Id < from + count).ToList());
            }

            return new JsonResult(_db.Races.ToList());
        }

        [HttpPost]
        public IActionResult Add(Race race)
        {
            _db.Races.Add(race);
            _db.SaveChanges();

            return Ok();
        }
    }
}
