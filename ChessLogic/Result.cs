using ChessLogic.Pieces;

namespace ChessLogic;

public sealed class Result
{
    public static Result Win(PlayerColor winner) => new(winner, GameEndReason.Checkmate);
    public static Result Draw(GameEndReason endReason) => new(PlayerColor.None, endReason);
    
    public Result(PlayerColor winner, GameEndReason endReason)
    {
        Winner = winner;
        EndReason = endReason;
    }

    public PlayerColor Winner { get; }
    public GameEndReason EndReason { get; }
}