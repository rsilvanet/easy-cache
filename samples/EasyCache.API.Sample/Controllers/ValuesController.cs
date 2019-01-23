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
        [Route("")]
        public ActionResult Get()
        {
            var cacheKey = "sample";
            var expiration = TimeSpan.FromSeconds(30);
            var data = _cache.GetValue(
                cacheKey, 
                () => _slowFakeDb.GetSomeData(), 
                expiration);

            return Ok(data);
        }

        [HttpGet]
        [Route("async")]
        public async Task<ActionResult> GetAsync()
        {
            var cacheKey = "sample-async";
            var expiration = TimeSpan.FromSeconds(30);
            var data = await _cache.GetValue(
                cacheKey, 
                async () => await _slowFakeDb.GetSomeDataAsync(), 
                expiration);

            return Ok(data);
        }
    }
}