using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus_3 {
    static class Constants {
        internal const string ENGINE_INFO = "id name Stocktopus-3\nid author ZlomenyMesic\nuciok";
        internal const string STARTPOS_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        internal const string FILES = "abcdefgh";
        internal const string PIECES = "pnbrqk";
    }

    enum Color {
        White = 0,
        Black = 1,
        None = 2
    }

    enum PieceType {
        None = 0,
        Pawn = 1,
        Knight = 2,
        Bishop = 3,
        Rook = 4,
        Queen = 5,
        King = 6
    }
}
