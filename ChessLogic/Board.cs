using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessLogic.Pieces;

namespace ChessLogic
{
    public sealed class Board
    {
        private readonly Piece[,] _pieces = new Piece[8, 8];

        public Board(string fen, string castlingRights = "KQkq", string enPassant = "-")
        {
            InitBoard(fen);
            InitCastlingRights(castlingRights);
            PawnSkippedPositions = new Dictionary<PlayerColor, Position>()
            {
                { PlayerColor.White, Position.None },
                { PlayerColor.Black, Position.None }
            };
            InitPossibleEnPassant(enPassant);
        }

        private Board(Board board)
        {
            for (var x = 0; x < 8; ++x)
            {
                for (var y = 0; y < 8; ++y)
                {
                    var piece = board._pieces[x, y].GetClone();
                    _pieces[x, y] = piece;
                }
            }

            PawnSkippedPositions = new Dictionary<PlayerColor, Position>(board.PawnSkippedPositions);
        }

        public Dictionary<PlayerColor, Position> PawnSkippedPositions { get; private set; }

        public Piece this[Position position]
        {
            get => position.IsValid ? _pieces[position.Y, position.X] : Piece.None;
            set
            {
                if (position.IsValid)
                    _pieces[position.Y, position.X] = value;
            }
        }

        public bool IsEmptyAt(Position position) => this[position] == Piece.None;

        public Board GetClone() => new Board(this);

        public IEnumerable<Position> GetAllPiecesPositions()
        {
            for (var x = 0; x < 8; ++x)
            {
                for (var y = 0; y < 8; ++y)
                {
                    var position = new Position(x, y);

                    if (!IsEmptyAt(position))
                        yield return position;
                }
            }
        }

        public string GetCastlingRights()
        {
            var result = string.Empty;

            if (CanCastleKingSide(new Position(4, 0)))
                result += "K";
        
            if (CanCastleQueenSide(new Position(4, 0)))
                result += "Q";
        
            if (CanCastleKingSide(new Position(4, 7)))
                result += "k";
        
            if (CanCastleQueenSide(new Position(4, 7)))
                result += "q";

            return result == string.Empty ? "-" : result;
        
            bool CanCastleKingSide(Position kingPosition)
            {
                if (IsEmptyAt(kingPosition))
                    return false;

                var king = this[kingPosition];
                var rook = this[new Position(7, kingPosition.Y)];

                return king is { IsMoved: false, Type: PieceType.King }
                       && rook is { IsMoved: false, Type: PieceType.Rook };
            }

            bool CanCastleQueenSide(Position kingPosition)
            {
                if (IsEmptyAt(kingPosition))
                    return false;

                var king = this[kingPosition];
                var rook = this[new Position(0, kingPosition.Y)];

                return king is { IsMoved: false, Type: PieceType.King }
                       && rook is { IsMoved: false, Type: PieceType.Rook };
            }
        
        }
    
        public override string ToString()
        {
            var builder = new StringBuilder(64);

            for (var i = 7; i >= 0; --i)
            {
                var noneCount = 0;
                for (var j = 0; j < 8; ++j)
                {
                    if (_pieces[i, j].Type == PieceType.None)
                    {
                        noneCount++;
                        continue;
                    }

                    if (noneCount != 0)
                        builder.Append(noneCount);

                    builder.Append(_pieces[i, j]);
                    noneCount = 0;
                }

                if (noneCount != 0)
                    builder.Append(noneCount);

                builder.Append('/');
            }

            return builder.ToString();
        }
    
    
        public IEnumerable<Position> GetPiecesPositionForPlayer(PlayerColor player) =>
            GetAllPiecesPositions().Where(position => this[position].Color == player);

        public bool IsInCheck(PlayerColor player) =>
            GetPiecesPositionForPlayer(player.GetOpponent())
                .Any(position => this[position].CanCaptureOpponentKing(this, position));

        private void InitBoard(string fen)
        {
            for (var i = 8; i > 1; --i)
            {
                fen = fen.Replace(i.ToString(), new string('1', i));
            }

            var data = fen.Split('/');

            for (var i = 7; i >= 0; --i)
            {
                for (var j = 0; j < 8; ++j)
                {
                    var piece = Piece.Create(data[i][j]);
                    _pieces[7 - i, j] = piece;
                }
            }
        }

        private void InitCastlingRights(string castlingRights)
        {
            if (!castlingRights.Contains('K'))
                _pieces[0, 7].IsMoved = true;

            if (!castlingRights.Contains('Q'))
                _pieces[0, 0].IsMoved = true;

            if (!castlingRights.Contains('k'))
                _pieces[7, 7].IsMoved = true;

            if (!castlingRights.Contains('q'))
                _pieces[7, 0].IsMoved = true;
        }

        private void InitPossibleEnPassant(string enPassant)
        {
            if (enPassant.Contains('f'))
                PawnSkippedPositions[PlayerColor.Black] = new Position(enPassant);
            else if (enPassant.Contains('e'))
                PawnSkippedPositions[PlayerColor.White] = new Position(enPassant);
        }
    }
}