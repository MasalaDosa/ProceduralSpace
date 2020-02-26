using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ProceduralSpace
{
    class Star
    {
        const double CHANCE_OF_PLANETS = 0.4;
        private class StarType
        {
            public StarType(string type, double prevalence, double minRadius, double maxRadius)
            {
                Type = type;
                Prevalence = prevalence;
                MinRadius = minRadius;
                MaxRadius = maxRadius;
            }

            public string Type { get; }
            public double Prevalence { get; }
            public double MinRadius { get; }
            public double MaxRadius { get; }
        }

        static List<StarType> starTypes = new List<StarType>
        {
            new StarType("Red Dwarf",       72,         0.7, 0.8),
            new StarType("Yellow Dwarf",    9,         0.96, 1.4),
            new StarType("Orange Dwarf",    9,         0.7, 0.96),
            new StarType("Brown Dwarf",     4.8,          0.06, 0.12),
            new StarType("White Dwarf",     4,          0.008, 0.2),
            new StarType("Neutron Star",    0.7,        000007, 0.000022),
            new StarType("Red Giant",       0.4,        20, 100),
            new StarType("Black Hole",      0.1,        0.000022, 0.00007),
            new StarType("Red Super Giant", 0.0001,     100, 1650),
            new StarType("Blue",            0.00003,    2.7, 10),
            new StarType("Blue Giant",      0.000003,    5, 10),
            new StarType("Blue Super Giant",0.000003,    20, 30),
        };

        private readonly LehmerPRNG lehmer;

        public string Name { get; }
        public string TypeOfStar { get; }
        public double Radius { get; }
        public List<Planet>? Planets { get; }
        public bool HasPlanets => Planets != null && Planets.Any();

        public Star(LehmerPRNG lehmer)
        {
            this.lehmer = lehmer;

            this.Name = GenerateName();

            var type = lehmer.NextChoice(starTypes, starTypes.Select(st => st.Prevalence));
            this.TypeOfStar = type.Type;
            this.Radius = lehmer.NextDouble(type.MinRadius, type.MaxRadius);

            var hasPlanets = lehmer.NextBool(CHANCE_OF_PLANETS);

            if (!hasPlanets)
            {
                return;
            }
            Planets = new List<Planet>();
            var numPlanets = lehmer.NextInt(1, 10);

            var currentDisanceInAU = lehmer.NextDouble(0.5, 4);
            for (int i = 0; i < numPlanets; i++)
            {
                Planets.Add(new Planet(lehmer, currentDisanceInAU));
                currentDisanceInAU += lehmer.NextDouble(0.5, 1.5) * (i + 1) * 0.5;
            }
        }

        private string GenerateName()
        {
            StringBuilder sb = new StringBuilder();
            int length = lehmer.NextInt(3, 11);
            bool isConsonant = lehmer.NextBool();
            for (int i = 0; i < length; i++)
            {
                sb.Append(isConsonant ? NextConsonant() : NextVowel());
                isConsonant = !isConsonant;
            }
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(sb.ToString());
        }


        private char NextConsonant()
        {
            char[] consonants = {
                'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm',
                'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z' };
            return lehmer.NextChoice(consonants);
        }

        private char NextVowel()
        {
            char[] vowels = {
                'a', 'e', 'i', 'o', 'u', 'y' };
            return lehmer.NextChoice(vowels);
        }

        public override string ToString()
        {
            return $"{Name} : {TypeOfStar} - {Radius:N3} Solar radii.";
        }
    }
}
