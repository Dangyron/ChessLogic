using System.Collections.Generic;
using ChessLogic.Moves;

namespace ChessLogic.Pieces
{
    public sealed class Bishop : Piece
    {
        public Bishop(PlayerColor color) : base(PieceType.Bishop, color)
        {
        }

        private Bishop(Piece another) : base(another)
        {
        }

        private static readonly Direction[] MoveDirections =
        {
            Direction.UpLeft,
            Direction.UpRight,
            Direction.DownLeft,
            Direction.DownRight,
        };

        public override Piece GetClone() => new Bishop(this);
        public override string ToString() => $"{(Color == PlayerColor.White ? "B": "b")}";

        public override IEnumerable<Move> GetMoves(Board board, Position from) =>
            GetMovesInDirections(board, from, MoveDirections);
    }
}