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
    public class CharactersController : ControllerBase
    {
        private ApplicationContext _db;

        public CharactersController(ApplicationContext appContext)
        {
            _db = appContext;
        }


        // Основные методы API

        [HttpGet]
        public JsonResult Get(int id)
        {
            // Загрузка всех основных данных персонажа
            var charData = (from ch in _db.Characters.Include(c => c.Gender).Include(c => c.LocBirth).Include(c => c.LocDeath).Include(c => c.Race)
                        join father in _db.Characters
                            on ch.BioFatherId equals father.Id
                        join mother in _db.Characters
                            on ch.BioMotherId equals mother.Id
                        where ch.Id == id
                        select new { 
                            ch.Id, ch.Name, ch.AltNames, ch.DateBirth, ch.DateDeath, ch.GenderId, ch.LocBirthId, ch.LocDeathId, 
                            ch.RaceId, ch.SocFormsId, ch.Growth, ch.BioFatherId, ch.BioMotherId, ch.ChildrenId, ch.Titles, 
                            ch.ColorHair, ch.ColorEyes, ch.Desc, ch.AltCharsId, ch.Additions,
                            Gender = ch.Gender.Name, LocBirth = ch.LocBirth.Name, LocDeath = ch.LocDeath.Name, Race = ch.Race.Name,
                            BioFather = father.Name, BioMother = mother.Name
                        }).FirstOrDefault();

            // Загрузка списка социальных формирований
            var socForms = _db.SocialFormations.Where(sf => charData.SocFormsId.Contains(sf.Id)).Select(sf => new { sf.Id, sf.Name }).ToList();

            // Загрузка списка детей
            var children = _db.Characters.Where(c => charData.ChildrenId.Contains(c.Id)).Select(c => new { c.Id, c.Name }).ToList();

            // Загрузка списка альтернативных карточек
            var altChars = _db.Characters.Where(c => charData.AltCharsId.Contains(c.Id)).Select(c => new { c.Id, c.Name }).ToList();


            var data = new { MainData = charData, SocialFormations = socForms, Children = children, AltCharCards = altChars };
            return new JsonResult(data);
        }

        [HttpGet]
        public JsonResult List(int? count, int? from = 0)
        {
            if (count.HasValue)
            {
                var data = _db.Characters.Select(c => new { c.Id, c.Name })
                    .Skip(from.Value).Take(count.Value).ToList();
                return new JsonResult(data);
            }
            else
            {
                var data = _db.Characters.Select(c => new { c.Id, c.Name })
                    .Skip(from.Value).ToList();
                return new JsonResult(data);
            }
        }

        [HttpGet]
        public JsonResult Find(string name)
        {
            var data = _db.Characters.Where(c => c.Name.ToLower().Contains(name.ToLower()) || ContainsWithIgnoringCase(c.AltNames, name))
                .Select(c => new { c.Id, c.Name }).ToList();

            return new JsonResult(data);
        }

        [Authorize(Policy = "ForEditors")]
        [HttpPost]
        public IActionResult Add(InputCharacter character)
        {
            try
            {
                _db.Characters.Add(character.ToCharacter(() => GetSocForms(character.SocFormsId)));
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
        public IActionResult Change(InputCharacter character)
        {
            try
            {
                Character oldChar = _db.Characters.Where(c => c.Id == character.Id).FirstOrDefault();
                character.WriteIn(oldChar, () => GetSocForms(character.SocFormsId));
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
                Character character = new Character() { Id = id };
                _db.Attach(character);
                _db.Remove(character);
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
                var data = _db.Characters.OrderBy(c => c.Name).Select(c => new { c.Id, c.Name })
                    .Skip(from.Value).Take(count.Value).ToList();
                return new JsonResult(data);
            }
            else
            {
                var data = _db.Characters.OrderBy(c => c.Name).Select(c => new { c.Id, c.Name })
                    .Skip(from.Value).ToList();
                return new JsonResult(data);
            }
        }


        // Дополнительные методы API

        [HttpGet]
        public JsonResult GetShort(int id)
        {
            var charData = (from ch in _db.Characters
                        where ch.Id == id
                        select new { 
                            ch.Name, ch.AltNames, ch.DateBirth, ch.DateDeath, ch.GenderId, ch.LocBirthId, ch.LocDeathId, 
                            ch.RaceId, ch.SocFormsId, ch.Growth, ch.BioFatherId, ch.BioMotherId, ch.ChildrenId, ch.Titles, 
                            ch.ColorHair, ch.ColorEyes, ch.Desc, ch.AltCharsId, ch.Additions
                        }).FirstOrDefault();

            var data = new { mainData = charData };
            return new JsonResult(data);
        }


        // Вспомогательные методы

        [NonAction]
        private static bool ContainsWithIgnoringCase(IEnumerable<string> list, string str)
        {
            foreach (string s in list)
            {
                if (s.ToLower().Contains(str.ToLower()))
                    return true;
            }

            return false;
        }

        [NonAction]
        private List<SocialFormation> GetSocForms(int[] indexes)
        {
            if (indexes != null)
                return _db.SocialFormations.Where(sf => indexes.Contains(sf.Id)).ToList();
            else
                return new List<SocialFormation>();
        }
    }
}
