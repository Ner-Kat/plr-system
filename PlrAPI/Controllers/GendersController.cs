using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlrAPI.Models;
using PlrAPI.Models.Database;
using Microsoft.AspNetCore.Authorization;
using PlrAPI.Systems;
using System.Text.Json;

namespace PlrAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class GendersController : ControllerBase
    {
        private ApplicationContext _db;
        private JsonSerializerOptions _jsonOptions;

        public GendersController(ApplicationContext appContext, IPlrJsonOptions plrJsonOptions)
        {
            _db = appContext;
            _jsonOptions = plrJsonOptions.GetJsonOptions();
        }

        [HttpGet]
        public JsonResult List()
        {
            return new JsonResult(_db.Genders.Select(g => new { g.Id, g.Name }).ToList(), _jsonOptions);
        }

        [HttpGet]
        public JsonResult Get(int id)
        {
            return new JsonResult(_db.Genders.Where(g => g.Id == id).Select(g => g.Name).FirstOrDefault(), _jsonOptions);
        }

        [HttpGet]
        public JsonResult GetId(string name)
        {
            return new JsonResult(_db.Genders.Where(g => g.Name == name).Select(g => g.Id).FirstOrDefault(), _jsonOptions);
        }
    }
}
