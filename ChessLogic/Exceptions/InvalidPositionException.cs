namespace ChessLogic.Exceptions;

public sealed class InvalidPositionException : Exception
{
    public InvalidPositionException(Position position) : base(position.ToString())
    {
    }
}