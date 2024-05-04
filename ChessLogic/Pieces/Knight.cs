using ChessLogic.Moves;

namespace ChessLogic.Pieces;

public sealed class Knight : Piece
{
    public Knight(PlayerColor color) : base(PieceType.Knight, color)
    {
    }

    private Knight(Piece another) : base(another)
    {
    }

    public override Knight GetClone() => new(this);
    public override string ToString() => $"{(Color == PlayerColor.White ? "N": "n")}";

    public override IEnumerable<Move> GetMoves(Board board, Position from)
    {
        foreach (var vDirection in new[] {Direction.Up, Direction.Down})
        {
            foreach (var hDirection in new[] { Direction.Left, Direction.Right })
            {
                var to = from + vDirection * 2 + hDirection;
                if (CanMoveTo(board, to))
                    yield return new CommonMove(from, to);

                to = from + hDirection * 2 + vDirection;
                
                if (CanMoveTo(board, to))
                    yield return new CommonMove(from, to);
            }
        }
    }
}