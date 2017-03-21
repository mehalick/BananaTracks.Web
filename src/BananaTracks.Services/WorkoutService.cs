using BananaTracks.Data;
using BananaTracks.Entities;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BananaTracks.Services
{
    public class WorkoutService
    {
        private readonly DocumentRepository<Workout> _workoutRepository;
        private readonly DocumentRepository<Activity> _activityRepository;

        public WorkoutService(IDocumentClient documentClient)
        {
            _workoutRepository = new DocumentRepository<Workout>(documentClient);
            _activityRepository = new DocumentRepository<Activity>(documentClient);
        }

        public async Task<IReadOnlyCollection<Workout>> GetWorkouts()
        {
            var workouts = await _workoutRepository.GetAll();

            return workouts.OrderBy(i => i.StartUtc).ToList();
        }

        public async Task<IReadOnlyCollection<Workout>> GetRecentWorkouts(int days)
        {
            var daysSince = DateTime.UtcNow.Date.AddDays(-days);
            var workouts = await _workoutRepository.GetItems(i => i.StartUtc >= daysSince);

            return workouts.OrderBy(i => i.StartUtc).ToList();
        }

        public async Task<Workout> GetWorkoutById(string id)
        {
            return await _workoutRepository.GetById(id);
        }

        public async Task<Workout> AddWorkout(string activityId, Workout workout)
        {
            var activity = await _activityRepository.GetById(activityId);
            if (activity == null)
            {
                throw new ArgumentException(nameof(activityId));
            }

            workout.Activity = activity;

            return await _workoutRepository.CreateItem(workout);
        }

        public async Task DeleteWorkout(string id)
        {
            await _workoutRepository.DeleteItem(id);
        }
    }
}
