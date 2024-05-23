using ChessLogic.Pieces;

namespace ChessLogic.Moves
{
    public sealed class Castling : Move
    {
        public Castling(Position from, Direction direction) : base(from, from + direction * 2)
        {
            Direction = direction;
        }
    
        public Direction Direction { get; }

        public override bool Make(Board board)
        {
            if (!IsLegal(board))
                return false;
        
            new CommonMove(From, To).Make(board);

            if (Direction == Direction.Right)
                new CommonMove(new Position(7, From.Y), To + Direction.Left).Make(board);
            else if (Direction == Direction.Left)
                new CommonMove(new Position(0, From.Y), To + Direction.Right).Make(board);

            return false;
        }

        public override string ToString(Board board)
        {
            return Direction == Direction.Right ? "O-O" : "O-O-O";
        }

        public override bool IsLegal(Board board)
        {
            if (board[From].Type == PieceType.None)
                return false;
        
            var player = board[From].Color;

            if (board.IsInCheck(player))
                return false;

            var clone = board.GetClone();
            var position = From;

            for (var i = 0; i < 2; ++i)
            {
                new CommonMove(position, position + Direction).Make(clone);
                position += Direction;
                if (clone.IsInCheck(player))
                    return false;
            }

            return true;
        }
    }
}