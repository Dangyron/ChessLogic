namespace ChessLogic.Moves;

public abstract class Move
{
    protected Move(Position from, Position to)
    {
        From = from;
        To = to;
    }

    public Position From { get; }
    public Position To { get; }
    
    public override string ToString() => $"{From}{To}";

    public abstract bool Make(Board board);

    public virtual bool IsValid(Board board)
    {
        return true;
    }
}