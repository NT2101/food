using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileFoodPermitController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MobileFoodPermitController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private async Task<List<MobileFoodPermit>> FetchMobileFoodPermitsAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://data.sfgov.org/resource/rqzj-sfat.json");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();


                return JsonConvert.DeserializeObject<List<MobileFoodPermit>>(json);

            }
            else
            {
                return new List<MobileFoodPermit>();
            }
        }
        [HttpGet]
        [Route("Getall")]
        public async Task<ActionResult<IEnumerable<MobileFoodPermit>>> SearchMobileFoodPermits1()
        {
            var results = await FetchMobileFoodPermitsAsync();
            return Ok(results);
        }

        [HttpGet]
        [Route("Getobjectid")]
        public async Task<ActionResult<IEnumerable<MobileFoodPermit>>> SearchMobileFoodPermits([FromQuery] int objectid)
        {
            var results = await FetchMobileFoodPermitsAsync();


            if (!int.IsNegative(objectid))
            {
                results = results.Where(p => p.objectid == objectid).ToList();
            }
            return Ok(results);
        }
        [HttpGet]
        [Route("GetFacilityType")]

        public async Task<ActionResult<IEnumerable<MobileFoodPermit>>> SearchMobileFoodPermits2([FromQuery] string FacilityType )
        {
            var results = await FetchMobileFoodPermitsAsync();

            if (!string.IsNullOrEmpty(FacilityType))
            {
                results = results.Where(p => p.FacilityType == FacilityType).ToList();
            }
            return Ok(results);
        }
       
    }
}
