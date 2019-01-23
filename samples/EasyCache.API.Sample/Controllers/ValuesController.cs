using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCache.API.Sample.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyCache.API.Sample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly Caching _cache;
        private readonly SlowFakeDbService _slowFakeDb;

        public ValuesController(Caching cache, SlowFakeDbService slowFakeDb)
        {
            _cache = cache;
            _slowFakeDb = slowFakeDb;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var cacheKey = "sample";
            var sourceFunc = () => _slowFakeDb.GetSomeData();
            var expiration = TimeSpan.FromSeconds(30);
            var data = _cache.GetValue(cacheKey, sourceFunc, expiration);

            return Ok(data);
        }
    }
}