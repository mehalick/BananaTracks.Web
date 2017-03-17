using BananaTracks.Entities;
using BananaTracks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BananaTracks.Web.Controllers
{
    [Route("api/activities")]
    public class ActivityController : Controller
    {
        private readonly ActivityService _activitiesService;

        public ActivityController(IDocumentClient documentClient)
        {
            _activitiesService = new ActivityService(documentClient);   
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<Activity>> Get()
        {
            return await _activitiesService.GetActivities();
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var activity = new Activity
            {
                Name = "Weights"
            };

            await _activitiesService.AddActivity(activity);

            return Ok(activity.Id);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
