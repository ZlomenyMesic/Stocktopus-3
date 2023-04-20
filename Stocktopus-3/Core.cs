//
// Stocktopus 3 chess engine
//      by ZlomenyMesic 
//

namespace Stocktopus_3 {
    static class Core {
        internal static Board board = new();
        internal static Color engineColor;
        internal static Color playerColor;

        internal static bool forceEnPassant = false; // don't ask

        internal static void SetOption(string[] tokens) {
            // setoption command format:
            //
            // setoption name <id> [value <x>]
            //
            // This is sent to the engine when the user wants to change the internal parameters of the engine.
            if (tokens.Length == 3 && tokens[1] == "name") {
                // toggleable options
                // ToLower() is called because the option name should not be case sensitive
                switch (tokens[2].ToLower()) {
                    case "forceenpassant": forceEnPassant ^= true; Console.WriteLine($"the option forceEnPassant is now set to {forceEnPassant}"); break;
                    default: Console.WriteLine($"unknown option: {tokens[2]}"); break;
                }
            } else if (tokens.Length == 5 && tokens[1] == "name" && tokens[3] == "value") {
                // options that need a value
            } else Console.WriteLine("invalid setoption format");
        }

        internal static void SetPosition(string[] tokens) {
            for (int i = 0; i < 64; i++)
                board.mailbox[i] = new Piece(PieceType.None, Color.None);
            // position command format:
            //
            // position [fen <fenstring> | startpos ] moves <move1> .... <movei>
            //
            // The position can either be set to the starting position or using a custom FEN string.
            // After the keyword "moves" is a list of moves played from the position. The moves are
            // stored using a form of the Long Algebraic Notation (LAN).

            if (tokens.Length > 1 && tokens[1] == "startpos") {
                SetPositionFEN(Constants.STARTPOS_FEN);
            } 
            else if (tokens.Length > 7 && tokens[1] == "fen") {
                // creates a sub-array and then joins the elements into a FEN string. 
                // the command could look for example like this:
                //
                // position fen rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
                //
                // offset 2 is needed to skip "position fen", and length 6 to fit all tokens

                string[] fenArray = new string[6];
                Array.Copy(tokens, 2, fenArray, 0, 6);
                SetPositionFEN(string.Join(' ', fenArray));
            } 
            else Console.WriteLine($"invalid token: {tokens[1]}");

            Board.Print(board);
        }

        private static void SetPositionFEN(string fen) {
            // set the current board position using a FEN string (Forsyth-Edwards Notation)
            // the string consists of 6 tokens

            string[] tokens = fen.Split(' ');

            // 1. PIECE PLACEMENT
            // Each rank is described, starting with rank 8 and ending with rank 1,
            // with a "/" between each one; within each rank, the contents of the squares are described
            // in order from the a-file to the h - file.Each piece is identified by a single letter taken
            // from the standard English names in algebraic notation (pawn = "P", knight = "N", bishop = "B",
            // rook = "R", queen = "Q" and king = "K"). White pieces are designated using uppercase letters ("PNBRQK"),
            // while black pieces use lowercase letters ("pnbrqk"). A set of one or more consecutive empty squares
            // within a rank is denoted by a digit from "1" to "8", corresponding to the number of squares.

            for (int i = 0, square = 0; i < tokens[0].Length; i++) {
                if (char.IsDigit(tokens[0][i])) {
                    square += tokens[0][i] - '0';
                }
                else if (char.IsLetter(tokens[0][i])) {
                    Color color = char.IsUpper(tokens[0][i]) ? Color.White : Color.Black;
                    PieceType pieceType;
                    switch (char.ToLower(tokens[0][i])) {
                        case 'p': pieceType = PieceType.Pawn; break;
                        case 'n': pieceType = PieceType.Knight; break;
                        case 'b': pieceType = PieceType.Bishop; break;
                        case 'r': pieceType = PieceType.Rook; break;
                        case 'q': pieceType = PieceType.Queen; break;
                        case 'k': pieceType = PieceType.King; break;
                        default: Console.WriteLine($"invalid fen string character: {tokens[0][i]}"); return;
                    }
                    board.mailbox[square++] = new Piece(pieceType, color);
                } 
                else if (tokens[0][i] != '/') {
                    Console.WriteLine($"invalid fen string character: {tokens[0][i]}");
                    return;
                }
                if (square > 63) break;
            }

            // 2. ACTIVE COLOR
            // "w" means white to move, "b" means black.

            if (tokens[1] == "w") board.sideToMove = Color.White;
            else if (tokens[1] == "b") board.sideToMove = Color.Black;
            else {
                Console.WriteLine($"invalid side to move {tokens[1]}");
                return;
            }

            // 3. CASTLING RIGHTS
            // If neither side can castle, this is "-". Otherwise, this has one or more letters: "K" (White can castle kingside),
            // "Q"(White can castle queenside), "k"(Black can castle kingside), and / or "q"(Black can castle queenside).



            // 4. EN PASSANT TARGET SQUARE
            // This is a square over which a pawn has just passed while moving two squares; it is given in algebraic
            // notation. If there is no en passant target square, this field uses the character "-".This is recorded
            // regardless of whether there is a pawn in position to capture en passant. An updated version of the
            // spec has since made it so the target square is only recorded if a legal en passant move is possible but
            // the old version of the standard is the one most commonly used.

            if (tokens[3].Length == 2 && char.IsDigit(tokens[3][0]) && char.IsDigit(tokens[3][1]))
                board.enPassantSquare = byte.Parse(tokens[3]);
            else if (tokens[3].Length == 1 && tokens[3][0] == '-')
                board.enPassantSquare = 0;
            else {
                Console.WriteLine($"invalid en passant square {tokens[3]}");
                return;
            }

            // 5. HALFMOVE CLOCK
            // The number of halfmoves since the last capture or pawn advance, used for the fifty-move rule.



            // 6 FULLMOVE NUMBER
            // The number of the full moves. It starts at 1 and is incremented after Black's move.
        }
    }
}
