using ChessLogic.Pieces;

namespace ChessLogic.Moves;

public abstract class Move
{
    public static readonly Move None = new NoneMove();
    
    public static Move Create(ReadOnlySpan<char> move)
    {
        var moveLength = move.Length;
        
        if (moveLength is < 4 or > 5)
            return None;

        var from = new Position(move[..2]);
        var to = new Position(move[2..4]);

        return new CommonMove(from, to);
    }
    
    protected Move(Position from, Position to)
    {
        From = from;
        To = to;
    }

    public Position From { get; }
    public Position To { get; }

    public virtual string ToString(Board board)
    {
        var piece = board[From];
        var capturing = board[To] != Piece.None ? From.ToString()[0] + "x" : "";
        var pieceName = piece.Type == PieceType.Pawn ? "" : piece.ToString();
        
        return pieceName + capturing + To;
    }

    public Piece CapturedPiece { get; protected set; } = Piece.None;
    
    public abstract bool Make(Board board);

    public virtual bool IsLegal(Board board)
    {
        if (board[From].Type == PieceType.None)
            return false;
        
        var color = board[From].Color;
        var clone = board.GetClone();

        Make(clone);
        return !clone.IsInCheck(color);
    }

    private class NoneMove : Move
    {
        public NoneMove() : base(Position.None, Position.None)
        {
        }

        public override bool Make(Board board) => false;
        public override bool IsLegal(Board board) => false;
    }
}