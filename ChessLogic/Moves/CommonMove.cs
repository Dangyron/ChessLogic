using ChessLogic.Pieces;

namespace ChessLogic.Moves;

public sealed class CommonMove : Move
{
    public CommonMove(Position from, Position to) : base(from, to)
    {
    }

    public override bool Make(Board board)
    {
        var isPawnMovedOrPieceCaptured = false;
        var piece = board[From];

        if (piece.Type == PieceType.Pawn || board[To].Type != PieceType.None)
            isPawnMovedOrPieceCaptured = true;


        return isPawnMovedOrPieceCaptured;
    }
}