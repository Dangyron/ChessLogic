using ChessLogic.Pieces;

namespace ChessLogic.Moves;

public sealed class Promotion : Move
{
    private Piece? _newPiece;

    public Promotion(Position from, Position to, PieceType promotionType) : base(from, to)
    {
        PromotionType = promotionType;
    }

    public PieceType PromotionType { get; }

    public override string ToString(Board board)
    {
        _newPiece ??= CreatePromotionPiece(board[From].Color);
        return base.ToString(board) + "=" + _newPiece;
    }

    public override bool Make(Board board)
    {
        var pawn = board[From];
        board[From] = Piece.None;

        _newPiece ??= CreatePromotionPiece(pawn.Color);
        CapturedPiece = board[To];
        board[To] = _newPiece;
        return true;
    }

    private Piece CreatePromotionPiece(PlayerColor player) =>
        PromotionType switch
        {
            PieceType.Knight => new Knight(player) { IsMoved = true },
            PieceType.Bishop => new Bishop(player) { IsMoved = true },
            PieceType.Rook => new Rook(player) { IsMoved = true },
            PieceType.Queen => new Queen(player) { IsMoved = true },
            _ => Piece.None,
        };
}