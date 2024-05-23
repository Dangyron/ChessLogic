using System;
using System.Collections.Generic;
using System.Linq;
using ChessLogic.Moves;
using ChessLogic.Pieces;

namespace ChessLogic
{
    public sealed class GameState
    {
        public const string DefaultFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        private readonly Board _board;
        private readonly PieceCounter _counter = new PieceCounter();

        private readonly Dictionary<string, int> _positionsHistory = new Dictionary<string, int>();
        private string _currentState;

        public GameState(string fen = DefaultFen)
        {
            var parts = fen.Split();
            _board = new Board(parts[0], parts[2], parts[3]);
            CurrentPlayer = parts[1] == "w" ? PlayerColor.White : PlayerColor.Black;
            FiftyMoveRuleCount = int.Parse(parts[4]);
            MoveNumber = int.Parse(parts[5]);
            _currentState = _board + " " + _board.GetCastlingRights() + GetEnPassantTarget();
            Pgn = $"{MoveNumber}. ";

            InitCounter();
            CheckIfGameOver();
            _positionsHistory[_currentState] = 1;
        }

        public Result? Result { get; private set; }
        public bool IsGameOver => Result != null;
        public PlayerColor CurrentPlayer { get; private set; }
        public string Pgn { get; private set; }
        public int FiftyMoveRuleCount { get; private set; }
        public int MoveNumber { get; private set; }

        public bool MakeMove(Move move)
        {
            if (IsGameOver || !IsValid(move))
                return false;

            _board.PawnSkippedPositions[CurrentPlayer] = Position.None;

            if (move.Make(_board))
            {
                FiftyMoveRuleCount = 0;
                _positionsHistory.Clear();

                if (move.CapturedPiece != Piece.None)
                    _counter.RemovePiece(CurrentPlayer.GetOpponent(), move.CapturedPiece.Type);
            }
            else
                FiftyMoveRuleCount++;

            _currentState = _board + " " + _board.GetCastlingRights() + GetEnPassantTarget();
            _positionsHistory.TryAdd(_currentState, 0);
            _positionsHistory[_currentState]++;
            CurrentPlayer = CurrentPlayer.GetOpponent();
            CheckIfGameOver();
            return true;
        }

        public override string ToString() =>
            _board + " " + PlayerColorExtensions.ToString(CurrentPlayer)
            + " " + _board.GetCastlingRights() + " " + GetEnPassantTarget() + " " + FiftyMoveRuleCount + " " + MoveNumber;

        public string GetBoard() => _board.ToString();

        public Move ParseMove(ReadOnlySpan<char> move)
        {
            if (move.Length != 7)
                return Move.None;

            var from = new Position(move[1..3]);
            var to = new Position(move[3..5]);

            var piece = _board[from];

            return piece.GetMoves(_board, from).FirstOrDefault(mv => mv.To == to) ?? Move.None;
        }

        public string ConvertMoveToString(Move move)
        {
            var piece = _board[move.To];
            var type = GetLastElements();

            return piece.ToString() + move.From + move.To + type;

            string GetLastElements()
            {
                return move.GetType() switch
                {
                    var t when t == typeof(CommonMove) => "C ",
                    var t when t == typeof(Castling) => ((Castling)move).Direction == Direction.Right ? "R " : "L ",
                    var t when t == typeof(EnPassant) => "E ",
                    var t when t == typeof(PawnDoubleMove) => "D ",
                    var t when t == typeof(Promotion) => "P" + ((Promotion)move).PromotionPiece,
                    _ => "  "
                };
            }
        }

        public IEnumerable<Move> GetLegalMovesForPieceAt(Position from)
        {
            if (_board.IsEmptyAt(from) || _board[from].Color != CurrentPlayer)
                return Enumerable.Empty<Move>();

            return _board[from].GetMoves(_board, from).Where(move => move.IsLegal(_board));
        }

        public IEnumerable<Move> GetAllMovesForPlayer(PlayerColor player) =>
            _board.GetPiecesPositionForPlayer(player)
                .SelectMany(from => _board[from].GetMoves(_board, from))
                .Where(move => move.IsLegal(_board));

        private string GetEnPassantTarget() =>
            GetAllMovesForPlayer(CurrentPlayer).FirstOrDefault(move => move is EnPassant)?.To.ToString() ?? "-";

        private void CheckIfGameOver()
        {
            if (!GetAllMovesForPlayer(CurrentPlayer).Any())
            {
                Result = _board.IsInCheck(CurrentPlayer)
                    ? Result.Win(CurrentPlayer.GetOpponent())
                    : Result.Draw(GameEndReason.Stalemate);
            }
            else if (IsInsufficientMaterial())
                Result = Result.Draw(GameEndReason.InsufficientMaterial);
            else if (IsFiftyMoveRule())
                Result = Result.Draw(GameEndReason.FiftyMoveRule);
            else if (IsThreefoldRepetition())
                Result = Result.Draw(GameEndReason.ThreefoldRepetition);
        }

        private bool IsInsufficientMaterial()
        {
            return IfKingVsKing() || IfKingVsKingAndBishop() || IfKingVsKingAndKnight() || IfKingAndBishopVsKingAndBishop();

            bool IfKingVsKing() => _counter.Total == 2;

            bool IfKingVsKingAndBishop() =>
                _counter.Total == 3 && (_counter.GetPieceCount(CurrentPlayer, PieceType.Bishop) == 1 ||
                                        _counter.GetPieceCount(CurrentPlayer.GetOpponent(), PieceType.Bishop) == 1);

            bool IfKingVsKingAndKnight() =>
                _counter.Total == 3 && (_counter.GetPieceCount(CurrentPlayer, PieceType.Knight) == 1 ||
                                        _counter.GetPieceCount(CurrentPlayer.GetOpponent(), PieceType.Knight) == 1);

            bool IfKingAndBishopVsKingAndBishop()
            {
                if (_counter.Total != 4)
                    return false;

                if (_counter.GetPieceCount(CurrentPlayer, PieceType.Bishop) != 1 ||
                    _counter.GetPieceCount(CurrentPlayer.GetOpponent(), PieceType.Bishop) != 1)
                    return false;

                var whiteBishop = _board.GetPiecesPositionForPlayer(PlayerColor.White)
                    .First(p => _board[p].Type == PieceType.Bishop);

                var blackBishop = _board.GetPiecesPositionForPlayer(PlayerColor.Black)
                    .First(p => _board[p].Type == PieceType.Bishop);

                return (whiteBishop.X + whiteBishop.Y) % 2 == (blackBishop.X + blackBishop.Y) % 2;
            }
        }

        private bool IsFiftyMoveRule() => FiftyMoveRuleCount == 100;

        private bool IsThreefoldRepetition() => _positionsHistory.ContainsValue(3);

        private bool IsValid(Move move)
        {
            var piece = _board[move.From];

            return piece.Color == CurrentPlayer &&
                   piece.GetMoves(_board, move.From).Any(m => m.From == move.From && m.To == move.To);
        }

        private void InitCounter()
        {
            for (var x = 0; x < 8; ++x)
            {
                for (var y = 0; y < 8; ++y)
                {
                    var position = new Position(x, y);
                    var piece = _board[position];

                    if (piece != Piece.None)
                        _counter.AddPiece(piece.Color, piece.Type);
                }
            }
        }
    }
}