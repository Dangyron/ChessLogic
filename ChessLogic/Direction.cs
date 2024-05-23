using System;

namespace ChessLogic
{
    public readonly struct Direction : IEquatable<Direction>
    {
        public static readonly Direction Up = new Direction(0, 1);
        public static readonly Direction Down = new Direction(0, -1);
        public static readonly Direction Left = new Direction(-1, 0);
        public static readonly Direction Right = new Direction(1, 0);
        public static readonly Direction UpLeft = Up + Left;
        public static readonly Direction UpRight = Up + Right;
        public static readonly Direction DownLeft = Down + Left;
        public static readonly Direction DownRight = Down + Right;

        private Direction(int deltaX, int deltaY)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
        }

        public int DeltaX { get; }
        public int DeltaY { get; }

        public static Direction operator +(Direction f, Direction s) => new Direction(f.DeltaX + s.DeltaX, f.DeltaY + s.DeltaY);

        public static Direction operator *(Direction f, int multiplier) =>
            new Direction(f.DeltaX * multiplier, f.DeltaY * multiplier);

        public static bool operator ==(Direction f, Direction s) => f.Equals(s);
        public static bool operator !=(Direction f, Direction s) => !f.Equals(s);

        public bool Equals(Direction other) => DeltaX == other.DeltaX && DeltaY == other.DeltaY;

        public override bool Equals(object? obj) => obj is Direction other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(DeltaX, DeltaY);
    }
}