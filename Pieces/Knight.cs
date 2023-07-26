using Chess.Board;
using Chess.Chesspiece;

namespace Chess.Pieces
{
    public class Knight : ChessPiece
    {
        public Knight(Color color, int x, int y) : base(color, x, y)
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

            int dx = x - this.x;
            int dy = y - this.y;

            if ((Math.Abs(dx) == 2 && Math.Abs(dy) == 1) || (Math.Abs(dx) == 1 && Math.Abs(dy) == 2))
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

            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    if (Math.Abs(i) == Math.Abs(j) || i == 0 || j == 0)
                    {
                        continue;
                    }

                    int newRow = row + i;
                    int newCol = col + j;

                    if (IsValidMove(newRow, newCol, board))
                    {
                        moves.Add((newRow, newCol));
                    }
                }
            }
            return moves;
        }

        public override ChessPiece Clone()
        {
            return new Knight(color, x, y);
        }

    }
}
