using Chess.Board;
using Chess.Chesspiece;

namespace Chess.Pieces
{
    public class King : ChessPiece
    {
        public King(Color color, int x, int y) : base(color, x, y)
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

            int dx = Math.Abs(x - this.x);
            int dy = Math.Abs(y - this.y);

            if (dx <= 1 && dy <= 1)
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

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
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
            return new King(color, x, y);
        }

    }
}
