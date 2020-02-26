using System;
namespace ProceduralSpace
{
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
