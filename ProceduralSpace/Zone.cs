﻿using System;
namespace ProceduralSpace
{
    /// <summary>
    /// Represents an area at a particular point
    /// </summary>
    class Zone
    {
        const double CHANCE_OF_STAR = 0.04;
        private readonly UPoint at;
        private readonly LehmerPRNG lehmer;

        public Star? Star { get; }
        public bool HasStar => Star != null;

        public Zone(UPoint at)
        {
            this.at = at;
            // The seed is generated by combining the lowest 2 bytes of x and y into 1 unit.
            // This means that the universe will repeat every 65536 x and y b
            lehmer = new LehmerPRNG((at.X & 0xFFFF) << 16 | (at.Y & 0xFFFF));

            var isStar = lehmer.NextBool(CHANCE_OF_STAR);

            if (!isStar)
            {
                return;
            }

            this.Star = new Star(lehmer);
        }
    }
}
