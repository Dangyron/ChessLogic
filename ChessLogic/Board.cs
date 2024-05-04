using ChessLogic.Pieces;

namespace ChessLogic;

public sealed class Board
{
    public const string DefaultFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    private readonly Piece[,] _pieces = new Piece[8, 8];

    public Board(string fen = DefaultFen, string pgn = "1. ")
    {
        Fen = fen;
        Pgn = pgn;
        InitBoard();
    }

    private Board(Board board)
    {
        Fen = board.Fen;
        Pgn = board.Pgn;
        
        for (var x = 0; x < 8; ++x)
        {
            for (var y = 0; y < 8; ++y)
            {
                _pieces[x, y] = board._pieces[x, y];
            }
        }
    }
    
    public string Fen { get; private set; }
    public string Pgn { get; private set; }
    public PlayerColor CurrentPlayer { get; private set; }
    public int MoveNumber { get; private set; }
    
    public Piece this[Position position] => position.IsValid ? _pieces[position.X, position.Y] : Piece.None;

    public bool IsEmptyAt(Position position) => this[position] == Piece.None;

    public Board GetClone() => new(this);

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

    public IEnumerable<Position> GetPiecesPositionForPlayer(PlayerColor player) =>
        GetAllPiecesPositions().Where(position => this[position].Color == player);

    public bool IsInCheck(PlayerColor player) =>
        GetPiecesPositionForPlayer(player.GetOpponent()).Any(position =>
        {
            var piece = this[position];
            return piece.CanCaptureOpponentKing(this, position);
        });
    
    private void InitBoard()
    {
        var parts = Fen.Split(' ');
        InitArray(parts[0]);
        CurrentPlayer = parts[1] == "w" ? PlayerColor.White : PlayerColor.Black;

        MoveNumber = int.Parse(parts[5]);
    }

    private void InitArray(string part)
    {
        for (var i = 8; i > 1; --i)
        {
            part = part.Replace(i.ToString(), new string('1', i));
        }
        var data = part.Split('/');

        for (var i = 7; i >= 0; --i)
        {
            for (var j = 0; j < 8; ++j)
            {
                _pieces[7 - i, j] = Piece.Create(data[i][j]);
            }
        }
    }
}