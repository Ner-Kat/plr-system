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
    public class CharactersController : ControllerBase
    {
        private ApplicationContext _db;

        public CharactersController(ApplicationContext appContext)
        {
            _db = appContext;
        }

        [NonAction]
        public JsonResult GetList(IQueryable<Character> characters, int? count, int? from)
        {
            if (count.HasValue)
            {
                return new JsonResult(characters.TakeWhile((ch, index) => index >= from && index < from + count).ToList());
            }

            return new JsonResult(characters.ToList());
        }

        [HttpGet]
        public JsonResult List(int? count, int? from = 0)
        {
            return GetList(_db.Characters, count, from);
        }

        [HttpGet]
        public JsonResult ListOrderedByNames(int? count, int? from = 0)
        {
            return GetList(_db.Characters.OrderBy(ch => ch.Name), count, from);
        }

        [HttpGet]
        public JsonResult ListOrderedByBirthDates(int? count, int? from = 0)
        {
            return GetList(_db.Characters.OrderBy(ch => ch.DateBirth), count, from);
        }

        [HttpGet]
        public JsonResult ListOrderedByBirthLocs(int? count, int? from = 0)
        {
            return GetList(_db.Characters.OrderBy(ch => ch.LocBirth.Name), count, from);
        }

        [HttpPost]
        public IActionResult Add(Character character)
        {
            _db.Characters.Add(character);
            _db.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public JsonResult GetCharacter(int id)
        {
            return new JsonResult(_db.Characters.Where(ch => ch.Id == id).First());
        }

        [HttpGet]
        public JsonResult GetCharacterByName(string name)
        {
            return new JsonResult(_db.Characters.Where(ch => ch.Name == name).ToList());
        }

        [HttpGet]
        public IActionResult Remove(Character character)
        {
            _db.Remove(character);
            _db.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IActionResult RemoveById(int id)
        {
            Character character = new Character() { Id = id };
            _db.Attach(character);
            _db.Remove(character);
            _db.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult Change(Character character)
        {
            Character oldCharacter = _db.Characters.Where(ch => ch.Id == character.Id).First();
            oldCharacter.Name = character.Name;
            oldCharacter.Desc = character.Desc;
            _db.SaveChanges();

            return Ok();
        }
    }
}
