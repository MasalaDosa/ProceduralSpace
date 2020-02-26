using System;
using System.Collections.Generic;
using System.Linq;

namespace ProceduralSpace
{
    class Planet
    {
        const double RING_CHANCE = 0.05;
        const double MOON_CHANCE = 0.3;
        private class PlanetType
        {
            public PlanetType(string type, double minRadius, double maxRadius, double minDistance, double maxDistance)
            {
                Type = type;
                MinRadius = minRadius;
                MaxRadius = maxRadius;
                MinDistance = minDistance;
                MaxDistance = maxDistance;
            }

            public string Type { get; }
            public double MinRadius { get; }
            public double MaxRadius { get; }
            public double MinDistance { get; }
            public double MaxDistance { get; }
        }

        List<PlanetType> planetTypes = new List<PlanetType>
        {
            new PlanetType("Rocky", 0.3, 0.8, 0, 1.6), // 0.25 - 1.6 AU
            new PlanetType("Earth Like Planet", 0.8, 1.3, 0.75, 1.5), // 0.75 AU - 1.5 AU
            new PlanetType("Super Earth", 1.3, 4, 1, 7), // 1 AU - 7 AU
            new PlanetType("Gas Giant",4, 14, 3, 15), // 4 AU - 15
            new PlanetType("Ice Giant", 4, 8, 13, double.MaxValue), // 15 AU - UP
            new PlanetType("Ice world", 0.3, 0.8, 15, double.MaxValue) // 15 AU - UP

        };
        private readonly LehmerPRNG lehmer;

        public Planet(LehmerPRNG lehmer, double distanceFromStar)
        {
            this.lehmer = lehmer;
            DistanceFromStar = distanceFromStar;
            HasRings = lehmer.NextBool(RING_CHANCE);
            var candidates = planetTypes.Where(pt => pt.MaxDistance >= distanceFromStar && pt.MinDistance <= distanceFromStar);
            var planetType = lehmer.NextChoice(candidates);
            Type = planetType.Type;
            Radius = lehmer.NextDouble(planetType.MinRadius, planetType.MaxRadius);

            var hasMoons = lehmer.NextBool(MOON_CHANCE);

            if (!hasMoons)
            {
                return;
            }

            Moons = new List<Moon>();
            var numMoons = lehmer.NextInt(1, 4);

            var currentDisanceInPU = lehmer.NextDouble(0.5, 4);
            for (int i = 0; i < numMoons; i++)
            {
                Moons.Add(new Moon(lehmer, currentDisanceInPU, Radius));
                currentDisanceInPU += lehmer.NextDouble(0.5, 1.5) * (i + 1) * 0.5;
            }
        }

        public double DistanceFromStar { get; }
        public bool HasRings { get; }
        public string Type { get; }
        public double Radius { get; }
        public List<Moon>? Moons { get; }
        public bool HasMoons => Moons != null && Moons.Any();

        public override string ToString()
        {
            return $"{Type} {(HasRings ? "(Ringed) " : "")}- Radius {Radius:N3} Earth Radii - {DistanceFromStar:N3} AU from star.";
        }
    }
}
