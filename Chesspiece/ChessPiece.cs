using Chess.Board;

namespace Chess.Chesspiece
{
    public enum Color { White, Black }

    public abstract class ChessPiece
    {
        public Color color { get; set; }

        public int x { get; set; }

        public int y { get; set; }

        public ChessPiece(Color color, int x, int y)
        {
            this.color = color;
            this.x = x;
            this.y = y;
        }

        public virtual bool IsValidMove(int x, int y, ChessBoard board)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7)
            {
                return false;
            }

            ChessPiece piece = board.Getposition(x, y);

            if (piece != null && piece.color == color)
            {
                return false;
            }

            return true;
        }

        public abstract List<(int x, int y)> GetPossibleMoves(ChessBoard board);

        public bool Move(int x, int y, ChessBoard board)
        {
            if (IsValidMove(x, y, board))
            {
                board.Movepiece(this.x, this.y, x, y);
                return true;
            }
            else
            {
                return false;
            }
        }

        public abstract ChessPiece Clone();

    }
}
