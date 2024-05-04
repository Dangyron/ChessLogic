namespace ChessLogic;

public readonly struct Position : IEquatable<Position>
{ 
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public Position(string position)
    {
        X = position[0] - 'a';
        Y = position[1] - '1';
    }
    
    public int X { get; }
    public int Y { get; }

    public bool IsValid => X is >= 0 and < 8 && Y is >= 0 and < 8;

    public static Position operator +(Position position, Direction direction) => new(position.X + direction.DeltaX, position.Y + direction.DeltaY);

    public static bool operator ==(Position f, Position s) => f.Equals(s);
    public static bool operator !=(Position f, Position s) => !f.Equals(s);

    public bool Equals(Position other) => X == other.X && Y == other.Y;

    public override bool Equals(object? obj) => obj is Position other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public override string ToString() => $"{Convert.ToChar(X + 'a')}{Y + 1}";
}