namespace ChessLogic;

public enum GameEndReason
{
    Checkmate,
    Stalemate,
    Aborted,
    FiftyMoveRule,
    InsufficientMaterial,
    ThreefoldRepetition,
}