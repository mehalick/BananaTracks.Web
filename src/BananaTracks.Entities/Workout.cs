﻿using System;

namespace BananaTracks.Entities
{
    public class Workout : EntityBase
    {
        public string UserId { get; set; }
        public Activity Activity { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime? EndUtc { get; set; }

        /// <summary>
        /// Gets a workout's duration in minutes.
        /// </summary>
        public double? Duration => EndUtc?.Subtract(StartUtc).TotalMinutes;
    }
}