using Newtonsoft.Json;

namespace BananaTracks.Entities
{
    public class User
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    public class Activity : EntityBase
    {
        public string Name { get; set; }
    }

    public class Workout
    {
        
    }
}
