using ChessLogic.Moves;

namespace ChessLogic.Pieces;

public sealed class Pawn : Piece
{
    private readonly Direction _direction;

    public Pawn(PlayerColor color) : base(PieceType.Pawn, color)
    {
        _direction = color == PlayerColor.White ? Direction.Up : Direction.Down;
    }

    private Pawn(Pawn another) : base(another)
    {
        _direction = another._direction;
    }

    public override Pawn GetClone() => new(this);
    public override string ToString() => $"{(Color == PlayerColor.White ? "P": "p")}";

    public override bool CanCaptureOpponentKing(Board board, Position from) =>
        GetDiagonalMoves(board, from).Any(move =>
        {
            var piece = board[move.To];
            return piece != None && piece.Type == PieceType.King;
        });
    public override IEnumerable<Move> GetMoves(Board board, Position from) =>
        GetForwardMoves(board, from).Concat(GetDiagonalMoves(board, from));
    
    protected override bool CanMoveTo(Board board, Position to) =>
        to.IsValid && (CanForwardTo(board, to) || CanCaptureAt(board, to));

    private bool CanForwardTo(Board board, Position to) => board.IsEmptyAt(to);
    
    private bool CanCaptureAt(Board board, Position to) =>
        !board.IsEmptyAt(to) && board[to].Color != Color;
    
    private IEnumerable<Move> GetForwardMoves(Board board, Position from)
    {
        var to = from + _direction;

        if (!CanForwardTo(board, to))
            yield break;

        yield return new CommonMove(from, to);

        if (IsMoved)
            yield break;

        to += _direction;

        if (!CanForwardTo(board, to))
            yield break;

        IsMoved = true;
        yield return new CommonMove(from, to);
    }

    private IEnumerable<Move> GetDiagonalMoves(Board board, Position from)
    {
        foreach (var direction in new[] { Direction.Left, Direction.Right, })
        {
            var to = from + _direction + direction;
            if (CanCaptureAt(board, to))
                yield return new CommonMove(from, to);
        }
    }
}