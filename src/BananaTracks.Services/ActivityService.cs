using BananaTracks.Data;
using BananaTracks.Entities;
using Microsoft.Azure.Documents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BananaTracks.Services
{
    public class ActivityService
    {
        private readonly DocumentRepository<Activity> _activityRepository;

        public ActivityService(IDocumentClient documentClient)
        {
            _activityRepository = new DocumentRepository<Activity>(documentClient);
        }

        public async Task<IReadOnlyCollection<Activity>> GetActivities()
        {
            var activities = await _activityRepository.GetItems(i => true);

            return activities.ToList();
        }

        public async Task<Activity> AddActivity(Activity activity)
        {
            return await _activityRepository.CreateItem(activity);
        }
    }
}
