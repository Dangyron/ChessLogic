using ChessLogic.Pieces;

namespace ChessLogic.Moves
{
    public sealed class Promotion : Move
    {
        public Promotion(Position from, Position to, PieceType promotionType) : base(from, to)
        {
            PromotionPiece = CreatePromotionPiece(To.Y == 7 ? PlayerColor.White : PlayerColor.Black, promotionType);
        }

        public Piece PromotionPiece { get; }

        public override string ToString(Board board)
        {
            return base.ToString(board) + "=" + PromotionPiece;
        }

        public override bool Make(Board board)
        {
            board[From] = Piece.None;
        
            CapturedPiece = board[To];
            board[To] = PromotionPiece;
            return true;
        }

        private Piece CreatePromotionPiece(PlayerColor player, PieceType promotionType) =>
            promotionType switch
            {
                PieceType.Knight => new Knight(player) { IsMoved = true },
                PieceType.Bishop => new Bishop(player) { IsMoved = true },
                PieceType.Rook => new Rook(player) { IsMoved = true },
                PieceType.Queen => new Queen(player) { IsMoved = true },
                _ => Piece.None,
            };
    }
}