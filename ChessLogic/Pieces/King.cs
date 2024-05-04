using ChessLogic.Moves;

namespace ChessLogic.Pieces;

public sealed class King : Piece
{
    public King(PlayerColor color) : base(PieceType.King, color)
    {
    }

    private King(Piece another) : base(another)
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

    public override King GetClone() => new(this);
    public override string ToString() => $"{(Color == PlayerColor.White ? "K": "k")}";
    
    public override bool CanCaptureOpponentKing(Board board, Position from) => false;

    public override IEnumerable<Move> GetMoves(Board board, Position from)
    {
        throw new NotImplementedException();
    }
}