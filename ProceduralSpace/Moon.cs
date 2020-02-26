using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralSpace
{
    class Moon
    {
        private readonly LehmerPRNG lehmer;

        public Moon(LehmerPRNG lehmer, double distanceFromPlanet, double planetRadius)
        {
            this.lehmer = lehmer;
            DistanceFromPlanet = distanceFromPlanet;
            Radius = lehmer.NextDouble(planetRadius / 8, planetRadius / 4);
        }

        public double DistanceFromPlanet { get; }
        public double Radius { get; }

        public override string ToString()
        {
            return $"Moon Radius {Radius:N3} Earth Radii - {DistanceFromPlanet:N3} PU from planet.";
        }
    }
}
