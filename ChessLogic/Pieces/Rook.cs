using System.Collections.Generic;
using ChessLogic.Moves;

namespace ChessLogic.Pieces
{
    public sealed class Rook : Piece
    {
        public Rook(PlayerColor color) : base(PieceType.Rook, color)
        {
        }

        private Rook(Piece another) : base(another)
        {
        }

        private static readonly Direction[] MoveDirections =
        {
            Direction.Up,
            Direction.Down,
            Direction.Left,
            Direction.Right,
        };

        public override Piece GetClone() => new Rook(this);
        public override string ToString() => $"{(Color == PlayerColor.White ? "R": "r")}";

        public override IEnumerable<Move> GetMoves(Board board, Position from) =>
            GetMovesInDirections(board, from, MoveDirections);
    }
}