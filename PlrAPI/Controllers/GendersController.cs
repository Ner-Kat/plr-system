﻿using Microsoft.AspNetCore.Http;
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
        public JsonResult GetGenderById(int id)
        {
            return new JsonResult(_db.Genders.Where(g => g.Id == id).First());
        }

        [HttpGet]
        public JsonResult GetGenderByName(string name)
        {
            return new JsonResult(_db.Genders.Where(g => g.Name == name).First());
        }
    }
}
