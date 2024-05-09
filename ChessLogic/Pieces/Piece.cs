using ChessLogic.Moves;

namespace ChessLogic.Pieces;

public abstract class Piece
{
    public static readonly Piece None = new NonePiece();

    public static Piece Create(char piece)
    {
        var color = char.IsUpper(piece) ? PlayerColor.White : PlayerColor.Black;

        return char.ToLower(piece) switch
        {
            'p' => new Pawn(color),
            'b' => new Bishop(color),
            'n' => new Knight(color),
            'r' => new Rook(color),
            'q'=> new Queen(color),
            'k' => new King(color),
            _ => None
        };
    }

    protected Piece(PieceType type, PlayerColor color)
    {
        Type = type;
        Color = color;
    }

    protected Piece(Piece another)
    {
        Type = another.Type;
        Color = another.Color;
        IsMoved = another.IsMoved;
    }

    public PieceType Type { get; }
    public PlayerColor Color { get; }
    public bool IsMoved { get; set; }

    public abstract Piece GetClone();

    public abstract IEnumerable<Move> GetMoves(Board board, Position from);

    public virtual bool CanCaptureOpponentKing(Board board, Position from) =>
        GetMoves(board, from).Any(move => board[move.To].Type == PieceType.King);
    
    protected IEnumerable<Move> GetMovesInDirections(Board board, Position from, Direction[] directions) =>
        directions.SelectMany(direction => GetMovesInDirection(board, from, direction));

    protected virtual bool CanMoveTo(Board board, Position to) => to.IsValid && (board.IsEmptyAt(to) || board[to].Color != Color);

    private IEnumerable<Move> GetMovesInDirection(Board board, Position from, Direction direction)
    {
        for (var to = from + direction; to.IsValid; to += direction)
        {
            if (board.IsEmptyAt(to))
            {
                yield return new CommonMove(from, to);
                continue;
            }

            if (board[to].Color != Color)
                yield return new CommonMove(from, to);

            yield break;
        }
    }

    private class NonePiece : Piece
    {
        public NonePiece() : base(PieceType.None, PlayerColor.None)
        {
        }

        public override Piece GetClone() => this;

        public override IEnumerable<Move> GetMoves(Board board, Position from) => Enumerable.Empty<Move>();
        public override string ToString() => "1";
    }
}