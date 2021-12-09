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
using PlrAPI.Systems;
using System.Text.Json;

namespace PlrAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CharactersController : ControllerBase
    {
        private ApplicationContext _db;
        private JsonSerializerOptions _jsonOptions;
        private ILogger<Startup> _logger;

        public CharactersController(ApplicationContext appContext, IPlrJsonOptions plrJsonOptions, ILogger<Startup> logger)
        {
            _db = appContext;
            _jsonOptions = plrJsonOptions.GetJsonOptions();
            _logger = logger;
        }


        // Основные методы API

        [HttpGet]
        public JsonResult Get(int id)
        {
            var charData = (from ch in _db.Characters
                        where ch.Id == id
                        select new { 
                            ch.Id, ch.Name, ch.AltNames, ch.DateBirth, ch.DateDeath, ch.Growth, ch.Titles,
                            ch.ColorHair, ch.ColorEyes, ch.Desc, Additions = ch.CharAdditionalValues,
                            Gender = ch.Gender, LocBirth = ch.LocBirth, LocDeath = ch.LocDeath, Race = ch.Race,
                            SocForms = ch.SocForms, ChildrenIds = ch.ChildrenId, ch.AltCharsId,
                            BioFatherId = ch.BioFatherId, BioMotherId = ch.BioMotherId,
                        }).FirstOrDefault();

            // Загрузка родителей
            dynamic father = null;
            if (charData.BioFatherId is not null)
                father = _db.Characters.Where(c => c.Id == charData.BioFatherId).Select(c => new { c.Id, c.Name }).FirstOrDefault();
            dynamic mother = null;
            if (charData.BioMotherId is not null)
                mother = _db.Characters.Where(c => c.Id == charData.BioMotherId).Select(c => new { c.Id, c.Name }).FirstOrDefault();

            // Загрузка списка детей
            dynamic children = null;
            if (charData.ChildrenIds is not null)
                children = _db.Characters.Where(c => charData.ChildrenIds.Contains(c.Id)).Select(c => new { c.Id, c.Name }).ToList();

            // Загрузка списка альтернативных карточек
            dynamic altChars = null;
            if (charData.AltCharsId is not null)
                altChars = _db.Characters.Where(c => charData.AltCharsId.Contains(c.Id)).Select(c => new { c.Id, c.Name }).ToList();

            // Формирование списка социальных формирований
            var socForms = new List<object>();
            if (charData.SocForms is not null)
            {
                foreach (SocialFormation socForm in charData.SocForms)
                {
                    socForms.Add(new { Id = socForm.Id, Name = socForm.Name });
                }
            }

            // Формирование набора дополнительных полей
            var additionalFields = new List<object>();
            if (charData.Additions is not null)
            {
                foreach (CharAdditionalValue field in charData.Additions)
                {
                    additionalFields.Add(new
                    {
                        TypeId = field.AdditionalFieldTypeId,
                        TypeName = field.AdditionalFieldType.Name,
                        Value = field.Value
                    });
                }
            }

            var data = new {
                Id = charData.Id, Name = charData.Name, AltNames = charData.AltNames, DateBirth = charData.DateBirth, DateDeath = charData.DateDeath, Growth = charData.Growth,
                Titles = charData.Titles, ColorHair = charData.ColorHair, ColorEyes = charData.ColorEyes, Desc = charData.Desc, Additions = additionalFields,

                Gender = charData.Gender is not null ? new { Id = charData.Gender.Id, Name = charData.Gender.Name } : null,
                LocBirth = charData.LocBirth is not null ? new { Id = charData.LocBirth.Id, Name = charData.LocBirth.Name } : null,
                LocDeath = charData.LocDeath is not null ? new { Id = charData.LocDeath.Id, Name = charData.LocDeath.Name } : null,
                Race = charData.Race is not null ? new { Id = charData.Race.Id, Name = charData.Race.Name } : null,

                SocForms = socForms, Children = children, AltChars = altChars,

                BioFather = father is not null ? new { Id = father.Id, Name = father.Name } : null,
                BioMother = mother is not null ? new { Id = mother.Id, Name = mother.Name } : null
            };
            return new JsonResult(data, _jsonOptions);
        }

        [HttpGet]
        public JsonResult List(int? count, int? from = 0)
        {
            if (count.HasValue)
            {
                var data = _db.Characters.Select(c => new { c.Id, c.Name }).OrderBy(c => c.Id)
                    .Skip(from.Value).Take(count.Value).ToList();
                return new JsonResult(data, _jsonOptions);
            }
            else
            {
                var data = _db.Characters.Select(c => new { c.Id, c.Name }).OrderBy(c => c.Id)
                    .Skip(from.Value).ToList();
                return new JsonResult(data, _jsonOptions);
            }
        }

        [HttpGet]
        public JsonResult Find(string name)
        {
            var data = _db.Characters.Where(c => c.Name.ToLower().Contains(name.ToLower()) || ContainsWithIgnoringCase(c.AltNames, name))
                .Select(c => new { c.Id, c.Name }).OrderBy(c => c.Id).ToList();

            return new JsonResult(data, _jsonOptions);
        }

        [Authorize(Policy = "ForEditors")]
        [HttpPost]
        public IActionResult Add(InputCharacter character)
        {
            try
            {
                Character addedChar = character.ToCharacter(
                    () => GetSocForms(character.SocFormsIds), () => FormAdditionals(character.Additions, character.Id)
                    );
                _db.Characters.Add(addedChar);
                _db.SaveChanges();
                SetFamilyRelations(addedChar);
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
                Character oldChar = _db.Characters.Where(c => c.Id == character.Id).Include(c => c.SocForms).FirstOrDefault();
                int? oldFatherId = oldChar.BioFatherId;
                int? oldMotherId = oldChar.BioMotherId;

                character.WriteIn(
                    oldChar, () => GetSocForms(character.SocFormsIds), () => FormAdditionals(character.Additions, character.Id)
                    );
                SetFamilyRelations(oldChar, oldFatherId, oldMotherId);
                _db.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
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
                return new JsonResult(data, _jsonOptions);
            }
            else
            {
                var data = _db.Characters.OrderBy(c => c.Name).Select(c => new { c.Id, c.Name })
                    .Skip(from.Value).ToList();
                return new JsonResult(data, _jsonOptions);
            }
        }


        // Дополнительные методы API

        //[HttpGet]
        //public JsonResult GetShort(int id)
        //{
        //    var charData = (from ch in _db.Characters
        //                where ch.Id == id
        //                select new { 
        //                    ch.Name, ch.AltNames, ch.DateBirth, ch.DateDeath, ch.GenderId, ch.LocBirthId, ch.LocDeathId, 
        //                    ch.RaceId, ch.SocFormsId, ch.Growth, ch.BioFatherId, ch.BioMotherId, ch.ChildrenId, ch.Titles, 
        //                    ch.ColorHair, ch.ColorEyes, ch.Desc, ch.AltCharsId
        //                }).FirstOrDefault();

        //    var data = new { mainData = charData };
        //    return new JsonResult(data);
        //}


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
            {
                try
                {
                    return _db.SocialFormations.Where(sf => indexes.Contains(sf.Id)).OrderBy(sf => sf.Id).ToList();
                }
                catch (ArgumentNullException)
                { }
            }

            return new List<SocialFormation>();
        }

        [NonAction]
        private List<CharAdditionalValue> FormAdditionals(Dictionary<string, string> newAdditions, int charId)
        {
            List<CharAdditionalValue> additions = new();

            try
            {
                additions = _db.CharAdditionalValues.Where(cav => cav.CharacterId == charId).OrderBy(cav => cav.Id).ToList();
            }
            catch (ArgumentNullException)
            { }

            foreach (var oldAdd in additions)
            {
                string fieldName = oldAdd.AdditionalFieldType.Name;

                // Изменение существующих значений
                if (newAdditions.ContainsKey(fieldName))
                {
                    oldAdd.Value = newAdditions[fieldName];
                    newAdditions.Remove(fieldName);
                }
                // Удаление неиспользуемых значений
                else
                {
                    additions.Remove(oldAdd);
                    _db.CharAdditionalValues.Remove(oldAdd);
                }
            }

            if (newAdditions is not null)
            {
                // Добавление новых значений
                foreach (var newAdd in newAdditions)
                {
                    var fieldType = _db.AdditionalFieldTypes.FirstOrDefault(aft => aft.Name == newAdd.Key);
                    CharAdditionalValue newAddEntry = null;

                    // Если поле такого типа уже существует
                    if (fieldType is not null)
                    {
                        newAddEntry = new CharAdditionalValue { AdditionalFieldTypeId = fieldType.Id, Value = newAdd.Value, CharacterId = charId };
                    }
                    // Если поле полностью новое
                    else
                    {
                        fieldType = _db.AdditionalFieldTypes.Add(new AdditionalFieldType { Name = newAdd.Key }).Entity;
                        newAddEntry = new CharAdditionalValue { AdditionalFieldTypeId = fieldType.Id, Value = newAdd.Value, CharacterId = charId };
                    }

                    _db.CharAdditionalValues.Add(newAddEntry);
                }
            }

            // _db.SaveChanges();
            return additions;
        }

        private void SetFamilyRelations(Character c, int? oldFatherId = null, int? oldMotherId = null)
        {
            var chars = _db.Characters.Where(
                chr => chr.Id == c.BioFatherId || chr.Id == c.BioMotherId
                || chr.Id == oldFatherId || chr.Id == oldMotherId
                ).ToList();

            var father = chars.Where(chr => chr.Id == c.BioFatherId).FirstOrDefault();
            var mother = chars.Where(chr => chr.Id == c.BioMotherId).FirstOrDefault();
            var oldFather = chars.Where(chr => chr.Id == oldFatherId).FirstOrDefault();
            var oldMother = chars.Where(chr => chr.Id == oldMotherId).FirstOrDefault();

            // Если отец изменился
            if (father != oldFather)
            {
                // Удалить у старого отца (если он есть) текущего ребёнка
                if (oldFather is not null && oldFather.ChildrenId is not null && oldFather.ChildrenId.Contains(c.Id))
                    oldFather.ChildrenId.Remove(c.Id);
            }

            // Если отец указан и не равен старому отцу (отец изменился и он есть)
            if (father is not null && father != oldFather)
            {
                // Добавить новому отцу текущего ребёнка
                if (father.ChildrenId is not null)
                    father.ChildrenId.Add(c.Id);
                else
                    father.ChildrenId = new() { c.Id };
            }


            // Если мать изменилась
            if (mother != oldMother)
            {
                // Удалить у старой матери (если она есть) текущего ребёнка
                if (oldMother is not null && oldMother.ChildrenId is not null && oldMother.ChildrenId.Contains(c.Id))
                    oldMother.ChildrenId.Remove(c.Id);
            }

            // Если мать указана и не равна старой матери (мать изменилась и она есть)
            if (mother is not null && mother != oldMother)
            {
                // Добавить новой матери текущего ребёнка
                if (mother.ChildrenId is not null)
                    mother.ChildrenId.Add(c.Id);
                else
                    mother.ChildrenId = new() { c.Id };
            }
        }
    }
}
