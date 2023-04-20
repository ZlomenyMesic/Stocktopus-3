using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus_3 {
    internal struct Piece {
        internal PieceType pieceType;
        internal Color color;

        public Piece(PieceType pieceType, Color color) {
            this.pieceType = pieceType;
            this.color = color;
        }
    }
}
