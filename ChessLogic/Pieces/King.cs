using System.Collections.Generic;
using System.Linq;
using ChessLogic.Moves;

namespace ChessLogic.Pieces
{
    public sealed class King : Piece
    {
        public King(PlayerColor color) : base(PieceType.King, color)
        {
        }

        private King(Piece another) : base(another)
        {
        }

        private static readonly Direction[] MoveDirections =
        {
            Direction.Up,
            Direction.Down,
            Direction.Left,
            Direction.Right,
            Direction.UpLeft,
            Direction.UpRight,
            Direction.DownLeft,
            Direction.DownRight,
        };

        public override Piece GetClone() => new King(this);
        public override string ToString() => $"{(Color == PlayerColor.White ? "K" : "k")}";

        public override bool CanCaptureOpponentKing(Board board, Position from) => false;

        public override IEnumerable<Move> GetMoves(Board board, Position from)
        {
            foreach (var direction in MoveDirections)
            {
                var to = from + direction;
                if (!CanMoveTo(board, to))
                    continue;

                yield return new CommonMove(from, to);
            }

            if (IsMoved)
                yield break;
        
            if (CanCastleKingSide(board, from))
                yield return new Castling(from, Direction.Right);
            if (CanCastleQueenSide(board, from))
                yield return new Castling(from, Direction.Left);
        }

        private bool CanCastleKingSide(Board board, Position from)
        {
            var rook = board[new Position(7, from.Y)];
            var isEmpty = new[] { new Position(5, from.Y), new Position(6, from.Y) }.All(board.IsEmptyAt);

            return IsRookValidForCastling(rook) && isEmpty;
        }

        private bool CanCastleQueenSide(Board board, Position from)
        {
            var rook = board[new Position(0, from.Y)];
            var isEmpty = new[] { new Position(1, from.Y), new Position(2, from.Y), new Position(3, from.Y) }.All(board.IsEmptyAt);

            return IsRookValidForCastling(rook) && isEmpty;
        }

        private bool IsRookValidForCastling(Piece rook) => rook is { Type: PieceType.Rook, IsMoved: false };
    }
}