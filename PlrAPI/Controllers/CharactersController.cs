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
            // var charData = (from ch in _db.Characters.Include(c => c.Gender).Include(c => c.LocBirth).Include(c => c.LocDeath).Include(c => c.Race)
            var charData = (from ch in _db.Characters
                        join father in _db.Characters
                            on ch.BioFatherId equals father.Id
                        join mother in _db.Characters
                            on ch.BioMotherId equals mother.Id
                        where ch.Id == id
                        select new { 
                            ch.Id, ch.Name, ch.AltNames, ch.DateBirth, ch.DateDeath, ch.Growth, ch.Titles,
                            ch.ColorHair, ch.ColorEyes, ch.Desc, ch.Additions,
                            Gender = ch.Gender, LocBirth = ch.LocBirth, LocDeath = ch.LocDeath, Race = ch.Race,
                            SocForms = ch.SocForms, ChildrenIds = ch.ChildrenId, ch.AltCharsId,
                            BioFather = father, BioMother = mother, 
                        }).FirstOrDefault();

            // Загрузка списка детей
            var children = _db.Characters.Where(c => charData.ChildrenIds.Contains(c.Id)).Select(c => new { c.Id, c.Name }).ToList();

            // Загрузка списка альтернативных карточек
            var altChars = _db.Characters.Where(c => charData.AltCharsId.Contains(c.Id)).Select(c => new { c.Id, c.Name }).ToList();

            // Формирование списка социальных формирований
            var socForms = new List<object>();
            foreach (SocialFormation socForm in charData.SocForms)
            {
                socForms.Add(new { Id = socForm.Id, Name = socForm.Name });
            }

            var data = new {
                Id = charData.Id, Name = charData.Name, AltNames = charData.AltNames, DateBirth = charData.DateBirth, DateDeath = charData.DateDeath, Growth = charData.Growth,
                Titles = charData.Titles, ColorHair = charData.ColorHair, ColorEyes = charData.ColorEyes, Desc = charData.Desc, Additions = charData.Additions,

                Gender = charData.Gender is not null ? new { Id = charData.Gender.Id, Name = charData.Gender.Name } : null,
                LocBirth = charData.LocBirth is not null ? new { Id = charData.LocBirth.Id, Name = charData.LocBirth.Name } : null,
                LocDeath = charData.LocDeath is not null ? new { Id = charData.LocDeath.Id, Name = charData.LocDeath.Name } : null,
                Race = charData.Race is not null ? new { Id = charData.Race.Id, Name = charData.Race.Name } : null,

                SocForms = socForms, Children = children, AltChars = altChars,

                BioFather = charData.BioFather is not null ? new { Id = charData.BioFather.Id, Name = charData.BioFather.Name } : null,
                BioMother = charData.BioMother is not null ? new { Id = charData.BioMother.Id, Name = charData.BioMother.Name } : null
            };
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
                _db.Characters.Add(character.ToCharacter(() => GetSocForms(character.SocFormsIds)));
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
                character.WriteIn(oldChar, () => GetSocForms(character.SocFormsIds));
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
