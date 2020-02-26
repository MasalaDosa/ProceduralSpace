using System;
namespace ProceduralSpace
{
    /// <summary>
    /// Represents a location in our universe.
    /// </summary>
    class UPoint
    {
        public UPoint(uint x, uint y)
        {
            X = x & 0xffff;
            Y = y & 0xffff;
        }

        public uint X { get; }
        public uint Y { get; }
    }
}
