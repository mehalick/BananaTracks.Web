using BananaTracks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using System.Threading.Tasks;
using BananaTracks.Entities;

namespace BananaTracks.Web.Controllers
{
    [Route("api/workouts")]
    public class WorkoutController : ApiBaseController
    {
        private readonly WorkoutService _workoutService;

        public WorkoutController(IDocumentClient documentClient)
        {
            _workoutService = new WorkoutService(documentClient);
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkouts()
        {
            var workouts = await _workoutService.GetWorkouts();

            return Ok(workouts);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentWorkouts()
        {
            var workouts = await _workoutService.GetRecentWorkouts(90);

            return Ok(workouts);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetWorkoutById(string id)
        {
            var workout = await _workoutService.GetWorkoutById(id);
            if (workout == null)
            {
                return NotFound();
            }

            return Ok(workout);
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkout([FromBody] Workout model)
        {
            var workout = await _workoutService.AddWorkout(model.Activity.Id, model);

            return CreatedAtAction(nameof(GetWorkoutById), new { id = workout.Id }, workout);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteWorkout(string id)
        {
            await _workoutService.DeleteWorkout(id);

            return Ok();
        }
    }
}