using Chess.Board;
using Chess.Chesspiece;

namespace Chess.Pieces
{
    public class Bishop : ChessPiece
    {
        public Bishop(Color color, int x, int y) : base(color, x, y)
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

            if (Math.Abs(dx) == Math.Abs(dy))
            {
                int directionX = Math.Sign(dx);
                int directionY = Math.Sign(dy);

                for (int i = 1; i < Math.Abs(dx); i++)
                {
                    if (board.Getposition(this.x + i * directionX, this.y + i * directionY) != null)
                    {
                        return false;
                    }
                }

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
                    if (Math.Abs(i) != Math.Abs(j))
                    {
                        continue;
                    }

                    int distance = 1;

                    while (true)
                    {
                        int newRow = row + i * distance;
                        int newCol = col + j * distance;

                        if (newRow < 0 || newRow > 7 || newCol < 0 || newCol > 7)
                        {
                            break;
                        }

                        if (board.Getposition(newRow, newCol) == null || board.Getposition(newRow, newCol).color != color)
                        {
                            if (IsValidMove(newRow, newCol, board))
                            {
                                moves.Add((newRow, newCol));
                            }
                        }

                        if (board.Getposition(newRow, newCol) != null)
                        {
                            break;
                        }

                        distance++;
                    }
                }
            }

            return moves;
        }

        public override ChessPiece Clone()
        {
            return new Bishop(color, x, y);
        }

    }
}

