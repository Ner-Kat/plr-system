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
    public class SocFormsController : ControllerBase
    {
        private ApplicationContext _db;

        public SocFormsController(ApplicationContext appContext)
        {
            _db = appContext;
        }

        [NonAction]
        public JsonResult GetList(IQueryable<SocialFormation> socForms, int? count, int? from)
        {
            if (count.HasValue)
            {
                return new JsonResult(socForms.TakeWhile((sf, index) => index >= from && index < from + count).ToList());
            }

            return new JsonResult(socForms.ToList());
        }

        [HttpGet]
        public JsonResult List(int? count, int? from = 0)
        {
            return GetList(_db.SocialFormations, count, from);
        }

        [HttpGet]
        public JsonResult ListOrderedByNames(int? count, int? from = 0)
        {
            return GetList(_db.SocialFormations.OrderBy(sf => sf.Name), count, from);
        }

        [HttpPost]
        public IActionResult Add(SocialFormation socForm)
        {
            _db.SocialFormations.Add(socForm);
            _db.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public JsonResult GetSocForm(int id)
        {
            return new JsonResult(_db.SocialFormations.Where(sf => sf.Id == id).First());
        }

        [HttpGet]
        public JsonResult GetSocFormByName(string name)
        {
            return new JsonResult(_db.SocialFormations.Where(sf => sf.Name == name).ToList());
        }

        [HttpGet]
        public IActionResult Remove(SocialFormation socForm)
        {
            _db.Remove(socForm);
            _db.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IActionResult RemoveById(int id)
        {
            SocialFormation socForm = new SocialFormation() { Id = id };
            _db.Attach(socForm);
            _db.Remove(socForm);
            _db.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult Change(SocialFormation socForm)
        {
            SocialFormation oldSocForm = _db.SocialFormations.Where(r => r.Id == socForm.Id).First();
            oldSocForm.Name = socForm.Name;
            oldSocForm.Desc = socForm.Desc;
            _db.SaveChanges();

            return Ok();
        }
    }
}
