using System.Collections.Generic;
using System.Linq;
using ChessLogic.Moves;

namespace ChessLogic.Pieces
{
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

        public override Piece GetClone() => new Pawn(this);
        public override string ToString() => $"{(Color == PlayerColor.White ? "P" : "p")}";

        public override bool CanCaptureOpponentKing(Board board, Position from) =>
            GetDiagonalMoves(board, from).Any(move => board[move.To].Type == PieceType.King);

        public override IEnumerable<Move> GetMoves(Board board, Position from) =>
            GetForwardMoves(board, from).Concat(GetDiagonalMoves(board, from));

        protected override bool CanMoveTo(Board board, Position to) =>
            CanForwardTo(board, to) || CanCaptureAt(board, to);

        private bool CanForwardTo(Board board, Position to) => board.IsEmptyAt(to);

        private bool CanCaptureAt(Board board, Position to) =>
            !board.IsEmptyAt(to) && board[to].Color != Color;

        private IEnumerable<Move> GetForwardMoves(Board board, Position from)
        {
            var to = from + _direction;

            if (!CanForwardTo(board, to))
                yield break;

            if (to.Y == 0 && to.Y == 7)
            {
                foreach (var promotionMove in GetPromotionMoves(from, to))
                    yield return promotionMove;

                yield break;
            }

            yield return new CommonMove(from, to);

            to += _direction;

            if (IsMoved || !CanForwardTo(board, to))
                yield break;

            yield return new PawnDoubleMove(from, to);
        }

        private IEnumerable<Move> GetDiagonalMoves(Board board, Position from)
        {
            foreach (var direction in new[] { Direction.Left, Direction.Right, })
            {
                var to = from + _direction + direction;

                if (to == board.PawnSkippedPositions[Color.GetOpponent()])
                    yield return new EnPassant(from, to);
            
                if (!CanCaptureAt(board, to))
                    continue;

                if (to.Y == 0 && to.Y == 7)
                {
                    foreach (var promotionMove in GetPromotionMoves(from, to))
                        yield return promotionMove;

                    yield break;
                }

                yield return new CommonMove(from, to);
            }
        }

        private IEnumerable<Move> GetPromotionMoves(Position from, Position to)
        {
            yield return new Promotion(from, to, PieceType.Knight);
            yield return new Promotion(from, to, PieceType.Bishop);
            yield return new Promotion(from, to, PieceType.Rook);
            yield return new Promotion(from, to, PieceType.Queen);
        }
    }
}