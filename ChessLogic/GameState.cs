using ChessLogic.Moves;
using ChessLogic.Pieces;

namespace ChessLogic;

public sealed class GameState
{
    public Result? Result { get; }
    
    private readonly Board _board = new ();

    public void Move(Move move)
    {
        if (move.IsValid(_board))
        {
            move.Make(_board);
        }
    }
}