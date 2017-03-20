using BananaTracks.Entities;
using BananaTracks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BananaTracks.Web.Controllers
{
    [Route("api/activities")]
    public class ActivityController : ApiBaseController
    {
        private readonly ActivityService _activitiesService;

        public ActivityController(IDocumentClient documentClient)
        {
            _activitiesService = new ActivityService(documentClient);
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<Activity>> GetActivities()
        {
            return await _activitiesService.GetActivities();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetActivityById(string id)
        {
            var activity = await _activitiesService.GetActivityById(id);
            if (activity == null)
            {
                return NotFound();
            }

            return Ok(activity);
        }

        [HttpPost]
        public async Task<IActionResult> AddActivity([FromBody]Activity model)
        {
            var activity = await _activitiesService.AddActivity(model);

            return CreatedAtAction(nameof(GetActivityById), new { id = activity.Id }, activity);
        }
    }
}
