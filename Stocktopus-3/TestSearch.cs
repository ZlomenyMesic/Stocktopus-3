﻿//
// Stocktopus 3 chess engine
//      by ZlomenyMesic
//

namespace Stocktopus_3 {
    internal static class TestSearch {
        internal static int depth = 4;
        internal static int maxqdepth = 2;

        internal static int nodes = 0;
        internal static int transpositions = 0;

        internal static Move GetIterativeSearchBestmove(Board board, int depth) {
            Move[] moves = Movegen.GetLegalMoves(board, Core.engineColor);
            (Move, int)[] possible = new (Move, int)[moves.Length];
            for (int i = 0; i < moves.Length; i++) {
                possible[i] = (moves[i], 0);
            }

            List<Move> best = new();
            int maxeval, currdepth = 0;
            while (currdepth++ < depth && /* is time left */ true) {
                Transpositions.Clear();
                maxeval = int.MinValue;
                best.Clear();

                nodes = 0;
                transpositions = 0;

                Move[] sorted = Move.SortMoves(possible);

                for (int i = 0; i < possible.Length; i++) {
                    Board temp = Board.Clone(board);
                    Board.PerformMove(temp, sorted[i]);

                    int addDepth = i < 4 && currdepth >= 4 ? 1 : 0;
                    int eval = Minimax(temp, currdepth - 1 + addDepth, Core.playerColor, int.MinValue, int.MaxValue);
                    possible[i].Item2 = eval;

                    Console.WriteLine($"{Move.ToString(sorted[i])} eval: {eval} depth: {currdepth + addDepth}");

                    if (eval > maxeval) {
                        best.Clear();
                        best.Add(sorted[i]);
                        maxeval = eval;
                    } else if (eval == maxeval) best.Add(sorted[i]);
                }
                Console.WriteLine("\n\n");
            }
            return best[new Random().Next(0, best.Count)];
        }

        internal static int Minimax(Board board, int depth, Color color, int alpha, int beta) {
            if (Transpositions.TryRetrieveEval(board, depth, out int eval)) return eval;

            if (depth == 0 || /* is draw */ false) {
                int qeval = QuiescenceSearch(board, color, 1);
                eval = Math.Max(qeval, Evaluation.Evaluate(board));
                Transpositions.Add(board, 0, eval);
                return eval;
            }

            eval = depth * 100000 * (color == Core.engineColor ? -1 : 1);
            Board[] children = Board.GetChildren(board, color);
            if (children.Length == 0 && !Board.IsCheck(board, color)) return 0; // stalemate

            for (int i = 0; i < children.Length; i++) {
                int nextSearch = Minimax(children[i], depth - 1, color == Color.White ? Color.Black : Color.White, alpha, beta);
                if (color == Core.engineColor) {
                    eval = Math.Max(eval, nextSearch);
                    if (eval > beta) break;
                    alpha = Math.Max(alpha, eval);
                } else {
                    eval = Math.Min(eval, nextSearch);
                    if (eval < alpha) break;
                    beta = Math.Min(beta, eval);
                }
            }

            Transpositions.Add(board, depth, eval);
            return eval;
        }

        internal static int QuiescenceSearch(Board board, Color color, int qdepth) {
            if (qdepth >= maxqdepth) return Evaluation.Evaluate(board);

            Board[] children = Board.GetChildren(board, color);
            //bool isCheck = Board.IsCheck(board, color);
            Board[] reduced = ReduceChildren(board, children, out bool isHopeless);

            int eval = depth * 100000 * (color == Core.engineColor ? -1 : 1);
            if (children.Length == 0 && !Board.IsCheck(board, color)) return 0; // stalemate

            //Console.WriteLine(isHopeless);
            //Board.Print(board);
            //Console.WriteLine(Move.ToString(board.lastMove));
            //Console.WriteLine(Evaluation.Evaluate(board));
            if (isHopeless) return Evaluation.Evaluate(board);

            for (int i = 0; i < reduced.Length; i++) {
                //Console.WriteLine(Move.ToString(reduced[i].lastMove));
                int nextSearch = QuiescenceSearch(reduced[i], color == Color.White ? Color.Black : Color.White, qdepth + 1);
                if (color == Core.engineColor) eval = Math.Max(eval, nextSearch);
                else eval = Math.Min(eval, nextSearch);
            }

            return eval;
        }

        internal static Board[] ReduceChildren(Board board, Board[] children, out bool isHopeless) {
            Board[] reduced = new Board[children.Length];
            int reducedCount = 0;
            for (int i = 0; i < children.Length; i++)
                if (!Move.IsHopeless(children[i].lastMove))
                    reduced[reducedCount++] = children[i];

            isHopeless = reducedCount == 0;
            Board[] final = new Board[reducedCount];
            for (int i = 0; i < reducedCount; i++)
                final[i] = reduced[i];
            return final;
        }
    }
}
