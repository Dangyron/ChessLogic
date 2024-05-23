namespace ChessLogic.Moves
{
    public sealed class PawnDoubleMove : Move
    {
        private readonly Position _skippedPosition;
        public PawnDoubleMove(Position from, Position to) : base(from, to)
        {
            _skippedPosition = new Position(From.X, (From.Y + To.Y) / 2);
        }

        public override bool Make(Board board)
        {
            board.PawnSkippedPositions[board[From].Color] = _skippedPosition;
            new CommonMove(From, To).Make(board);
            return true;
        }

        public override string ToString(Board board) => To.ToString();
    }
}