using System;
using Newtonsoft.Json;

namespace BananaTracks.Entities
{
    public abstract class EntityBase
    {
        [JsonProperty(PropertyName = "id", Order = -4)]
        public string Id { get; set; }

        [JsonProperty(Order = -3)]
        public string Type => GetType().ToString();

        [JsonProperty(Order = -2)]
        public DateTime CreatedUtc { get; set; }
    }
}