namespace ChessLogic;

public readonly struct Direction
{
    public static readonly Direction Up = new(0, 1);
    public static readonly Direction Down = new(0, -1);
    public static readonly Direction Left = new(-1, 0);
    public static readonly Direction Right = new(1, 0);
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

    public static Direction operator +(Direction f, Direction s) => new(f.DeltaX + s.DeltaX, f.DeltaY + s.DeltaY);
    public static Direction operator *(Direction f, int multiplier) => new(f.DeltaX * multiplier, f.DeltaY * multiplier);
}