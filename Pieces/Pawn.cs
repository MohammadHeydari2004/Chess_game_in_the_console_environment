using Chess.Board;
using Chess.Chesspiece;

namespace Chess.Pieces
{
    public class Pawn : ChessPiece
    {
        public Pawn(Color color, int x, int y) : base(color, x, y)
        {
        }

        public override bool IsValidMove(int x, int y, ChessBoard board)
        {
            if (!base.IsValidMove(x, y, board))
            {
                return false;
            }

            if (x == this.x && y == this.y)
            {
                return false;
            }

            ChessPiece piececheck = board.Getposition(x, y);

            int dx = x - this.x;
            int dy = y - this.y;

            if (dx == (color == Color.Black ? 1 : -1) && dy == 0 && piececheck == null)
            {
                return true;
            }

            if (color == Color.Black && this.x == 1 && x == 3 && piececheck == null)
            {
                int X = 2;
                ChessPiece piececheck1 = board.Getposition(X, y);
                if (piececheck1 == null)
                {
                    return true;
                }
            }

            if (color == Color.White && this.x == 6 && x == 4 && (piececheck == null))
            {
                int X = 5;
                ChessPiece piececheck2 = board.Getposition(X, y);
                if (piececheck2 == null)
                {
                    return true;
                }
            }

            if (dx == (color == Color.Black ? 1 : -1) && Math.Abs(dy) == 1 && piececheck != null && piececheck.color != color)
            {
                return true;
            }

            return false;
        }

        public override List<(int x, int y)> GetPossibleMoves(ChessBoard board)
        {
            List<(int x, int y)> moves = new List<(int x, int y)>();
            int row = this.x;
            int col = this.y;
            int direction = color == Color.White ? -1 : 1;

            if (row + direction >= 0 && row + direction <= 7 && board.Getposition(row + direction, col) == null)
            {
                if (IsValidMove(row + direction, col, board))
                {
                    moves.Add((row + direction, col));
                }

                if ((color == Color.White && row == 6) || (color == Color.Black && row == 1))
                {
                    if (row + 2 * direction >= 0 && row + 2 * direction <= 7 && board.Getposition(row + 2 * direction, col) == null)
                    {
                        if (IsValidMove(row + 2 * direction, col, board))
                        {
                            moves.Add((row + 2 * direction, col));
                        }
                    }
                }
            }

            for (int i = -1; i <= 1; i += 2)
            {
                if (row + direction >= 0 && row + direction <= 7 && col + i >= 0 && col + i <= 7)
                {
                    if (board.Getposition(row + direction, col + i) != null && board.Getposition(row + direction, col + i).color != color)
                    {
                        if (IsValidMove(row + direction, col + i, board))
                        {
                            moves.Add((row + direction, col + i));
                        }
                    }
                }
            }
            return moves;
        }

        public override ChessPiece Clone()
        {
            return new Pawn(color, x, y);
        }

    }
}
