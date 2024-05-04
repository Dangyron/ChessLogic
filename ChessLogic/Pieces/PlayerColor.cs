namespace ChessLogic.Pieces;

public enum PlayerColor
{
    None,
    Black,
    White,
}

public static class PlayerColorExtensions
{
    public static PlayerColor GetOpponent(this PlayerColor player) =>
        player switch
        {
            PlayerColor.White => PlayerColor.Black,
            PlayerColor.Black => PlayerColor.White,
            _ => player,
        };
}