using ChessLogic.Moves;

namespace ChessLogic.Pieces;

public sealed class Queen : Piece
{
    public Queen(PlayerColor color) : base(PieceType.Queen, color)
    {
    }

    private Queen(Piece another) : base(another)
    {
    }

    private static readonly Direction[] MoveDirections =
    {
        Direction.Up,
        Direction.Down,
        Direction.Left,
        Direction.Right,
        Direction.UpLeft,
        Direction.UpRight,
        Direction.DownLeft,
        Direction.DownRight,
    };
    
    public override Queen GetClone() => new(this);
    public override string ToString() => $"{(Color == PlayerColor.White ? "Q": "q")}";

    public override IEnumerable<Move> GetMoves(Board board, Position from) =>
        GetMovesInDirections(board, from, MoveDirections);
}