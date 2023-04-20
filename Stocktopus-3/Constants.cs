using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus_3 {
    static class Constants {
        internal static string ENGINE_INFO = "id name Stocktopus-3\nid author ZlomenyMesic\nuciok";
        internal static string STARTPOS_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    }

    enum Color {
        White,
        Black,
        None
    }

    enum PieceType {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King,
        None
    }
}
