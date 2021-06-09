using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlrAPI.Models;
using PlrAPI.Models.Database;
using Microsoft.AspNetCore.Authorization;

namespace PlrAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class GendersController : ControllerBase
    {
        private ApplicationContext _db;
        public GendersController(ApplicationContext appContext)
        {
            _db = appContext;
        }

        [HttpGet]
        public JsonResult List()
        {
            return new JsonResult(_db.Genders.ToList());
        }

        [HttpGet]
        public JsonResult Get(int id)
        {
            return new JsonResult(_db.Genders.Where(g => g.Id == id).Select(g => g.Name).FirstOrDefault());
        }

        [HttpGet]
        public JsonResult GetId(string name)
        {
            return new JsonResult(_db.Genders.Where(g => g.Name == name).Select(g => g.Id).FirstOrDefault());
        }
    }
}
