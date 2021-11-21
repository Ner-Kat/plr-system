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
    public class SocFormsController : ControllerBase
    {
        private ApplicationContext _db;
        private JsonSerializerOptions _jsonOptions;

        public SocFormsController(ApplicationContext appContext, IPlrJsonOptions plrJsonOptions)
        {
            _db = appContext;
            _jsonOptions = plrJsonOptions.GetJsonOptions();
        }


        // Основные методы API

        [HttpGet]
        public JsonResult Get(int id)
        {
            var data = _db.SocialFormations.Where(sf => sf.Id == id).Select(sf => new { sf.Id, sf.Name, sf.Desc }).FirstOrDefault();

            return new JsonResult(data, _jsonOptions);
        }

        [HttpGet]
        public JsonResult List(int? count, int? from = 0)
        {
            if (count.HasValue)
            {
                var data = _db.SocialFormations.Select(sf => new { sf.Id, sf.Name })
                    .Skip(from.Value).Take(count.Value).ToList();
                return new JsonResult(data, _jsonOptions);
            }
            else
            {
                var data = _db.SocialFormations.Select(sf => new { sf.Id, sf.Name })
                    .Skip(from.Value).ToList();
                return new JsonResult(data, _jsonOptions);
            }
        }

        [HttpGet]
        public JsonResult Find(string name)
        {
            var data = (from socForm in _db.SocialFormations
                        where socForm.Name.ToLower().Contains(name.ToLower())
                        select new { socForm.Id, socForm.Name }).ToList();

            return new JsonResult(data, _jsonOptions);
        }

        [Authorize(Policy = "ForEditors")]
        [HttpPost]
        public IActionResult Add(InputSocialFormation socForm)
        {
            try
            {
                _db.SocialFormations.Add(socForm.ToSocialFormation());
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
        public IActionResult Change(InputSocialFormation socForm)
        {
            try
            {
                SocialFormation oldSocForm = _db.SocialFormations.Where(sf => sf.Id == socForm.Id).FirstOrDefault();
                socForm.WriteIn(oldSocForm);
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
                SocialFormation socForm = new SocialFormation() { Id = id };
                _db.Attach(socForm);
                _db.Remove(socForm);
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
                var data = _db.SocialFormations.OrderBy(sf => sf.Name).Select(sf => new { sf.Id, sf.Name })
                    .Skip(from.Value).Take(count.Value).ToList();
                return new JsonResult(data, _jsonOptions);
            }
            else
            {
                var data = _db.SocialFormations.OrderBy(sf => sf.Name).Select(sf => new { sf.Id, sf.Name })
                    .Skip(from.Value).ToList();
                return new JsonResult(data, _jsonOptions);
            }
        }
    }
}
