using ChessLogic.Pieces;

namespace ChessLogic.Moves;

public sealed class EnPassant : Move
{
    private readonly Position _capturedPosition;
    
    public EnPassant(Position from, Position to) : base(from, to)
    {
        _capturedPosition = new Position(to.X, from.Y);
    }

    public override bool Make(Board board)
    {
        new CommonMove(From, To).Make(board);
        board[_capturedPosition] = Piece.None;
        CapturedPiece = board[_capturedPosition];
        return true;
    }

    public override string ToString(Board board) => From.ToString()[0] + "x" + To;
}