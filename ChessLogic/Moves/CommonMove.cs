using ChessLogic.Pieces;

namespace ChessLogic.Moves;

public sealed class CommonMove : Move
{
    public CommonMove(Position from, Position to) : base(from, to)
    {
    }

    public override bool Make(Board board)
    {
        var piece = board[From];
        CapturedPiece = board[To];
        
        board[To] = piece;
        board[From] = Piece.None;

        piece.IsMoved = true;
        
        return piece.Type == PieceType.Pawn || CapturedPiece.Type != PieceType.None;
    }
}