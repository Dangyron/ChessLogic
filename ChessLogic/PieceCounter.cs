using System;
using System.Collections.Generic;
using ChessLogic.Pieces;

namespace ChessLogic
{
    public sealed class PieceCounter
    {
        private readonly Dictionary<PieceType, int> _whitePieces = new Dictionary<PieceType, int>();
        private readonly Dictionary<PieceType, int> _blackPieces = new Dictionary<PieceType, int>();

        public PieceCounter()
        {
            foreach (PieceType name in Enum.GetValues(typeof(PieceType)))
            {
                _whitePieces[name] = 0;
                _blackPieces[name] = 0;
            }
        }
    
        public int Total { get; private set; }

        public void AddPiece(PlayerColor color, PieceType pieceType)
        {
            if (color == PlayerColor.White)
                _whitePieces[pieceType]++;
            else if (color == PlayerColor.Black)
                _blackPieces[pieceType]++;

            Total++;
        }

        public void RemovePiece(PlayerColor color, PieceType pieceType)
        {
            if (color == PlayerColor.White)
                _whitePieces[pieceType]--;
            else if (color == PlayerColor.Black)
                _blackPieces[pieceType]--;

            Total--;
        }

        public int GetPieceCount(PlayerColor color, PieceType pieceType) =>
            color == PlayerColor.White ? _whitePieces[pieceType] : _blackPieces[pieceType];
    }
}