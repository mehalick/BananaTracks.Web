using System.Threading.Tasks;
using BananaTracks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;

namespace BananaTracks.Web.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly WorkoutService _workoutService;

        public HomeController(IDocumentClient documentClient)
        {
            _workoutService = new WorkoutService(documentClient);
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var workouts = await _workoutService.GetWorkouts();

            return View(workouts);
        }
    }
}