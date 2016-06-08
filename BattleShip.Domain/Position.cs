using System.Collections.Generic;

namespace BattleShip.Domain
{
    public class Position
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        private sealed class YPositionXPositionEqualityComparer : IEqualityComparer<Position>
        {
            public bool Equals(Position x, Position y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.YPosition == y.YPosition && x.XPosition == y.XPosition;
            }

            public int GetHashCode(Position obj)
            {
                unchecked
                {
                    return (obj.YPosition*397) ^ obj.XPosition;
                }
            }
        }

        public static IEqualityComparer<Position> YPositionXPositionComparer { get; } = new YPositionXPositionEqualityComparer();
    }
}