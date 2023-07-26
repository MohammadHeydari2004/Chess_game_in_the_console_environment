using Chess.Chesspiece;
using Chess.Pieces;
namespace Chess.Board
{
    public class ChessBoard
    {
        public ChessPiece[,] Board { get; set; }

        public Dictionary<Color, List<ChessPiece>> _Pieces { get; set; }

        public ChessBoard()
        {
            Board = new ChessPiece[8, 8];
            _Pieces = new Dictionary<Color, List<ChessPiece>>();
            for (int i = 0; i < 2; i++)
            {
                Color color = i == 0 ? Color.Black : Color.White;
                int row = i == 0 ? 0 : 7;
                List<ChessPiece> pieces = new List<ChessPiece>();
                pieces.Add(new Rook(color, row, 0));
                pieces.Add(new Knight(color, row, 1));
                pieces.Add(new Bishop(color, row, 2));
                pieces.Add(new Queen(color, row, 3));
                pieces.Add(new King(color, row, 4));
                pieces.Add(new Bishop(color, row, 5));
                pieces.Add(new Knight(color, row, 6));
                pieces.Add(new Rook(color, row, 7));
                for (int j = 0; j < 8; j++)
                {
                    pieces.Add(new Pawn(color, row + (i == 0 ? 1 : -1), j));
                }
                _Pieces.Add(color, pieces);
                foreach (ChessPiece piece in pieces)
                {
                    Board[piece.x, piece.y] = piece;
                }
            }
        }

        public ChessPiece Getposition(int x, int y)
        {
            return Board[x, y];
        }

        public void Setposition(int x, int y, ChessPiece piece)
        {
            Board[x, y] = piece;
        }

        public void Movepiece(int x1, int y1, int x2, int y2)
        {
            ChessPiece piece = Getposition(x1, y1);
            ChessPiece captured = Getposition(x2, y2);
            if (captured != null)
            {
                _Pieces[captured.color].Remove(captured);
            }

            piece.x = x2; piece.y = y2; Setposition(x1, y1, null); Setposition(x2, y2, piece);

            if (piece is Pawn p && (p.x == 0 || p.x == 7))
            { // promotion
                ChessPiece queen = new Queen(p.color, p.x, p.y); // always promote to queen
                _Pieces[p.color].Remove(p); _Pieces[p.color].Add(queen); Setposition(p.x, p.y, queen); // replace pawn with queen
            }

        }

        public bool CanBlockChecker(Color color)
        {

            ChessPiece king = _Pieces[color].Find(p => p is King);

            var oppositeColor = color == Color.White ? Color.Black : Color.White;

            var enemyPieces = _Pieces[oppositeColor];

            foreach (var enemy in enemyPieces)
            {

                var moves = enemy.GetPossibleMoves(this);

                foreach (var move in moves)
                {

                    if (move.x == king.x && move.y == king.y)
                    {

                        foreach (var friend in _Pieces[color])
                        {

                            var friendMoves = friend.GetPossibleMoves(this);

                            foreach (var friendMove in friendMoves)
                            {

                                if ((enemy.x == king.x && friendMove.x == enemy.x) ||
                                    (enemy.y == king.y && friendMove.y == enemy.y) ||
                                    (Math.Abs(enemy.x - king.x) == Math.Abs(enemy.y - king.y) &&
                                     Math.Abs(friendMove.x - enemy.x) == Math.Abs(friendMove.y - enemy.y)))
                                {

                                    if ((friendMove.x > Math.Min(enemy.x, king.x) && friendMove.x < Math.Max(enemy.x, king.x)) &&
                                        (friendMove.y > Math.Min(enemy.y, king.y) && friendMove.y < Math.Max(enemy.y, king.y)))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        public bool CanRemoveChecker(Color color)
        {

            ChessPiece king = _Pieces[color].Find(p => p is King);

            var oppositeColor = color == Color.White ? Color.Black : Color.White;

            var enemyPieces = _Pieces[oppositeColor];

            foreach (var enemy in enemyPieces)
            {

                var moves = enemy.GetPossibleMoves(this);

                foreach (var move in moves)
                {

                    if (move.x == king.x && move.y == king.y)
                    {

                        foreach (var friend in _Pieces[color])
                        {

                            var friendMoves = friend.GetPossibleMoves(this);

                            foreach (var friendMove in friendMoves)
                            {

                                if (friendMove.x == enemy.x && friendMove.y == enemy.y)
                                {

                                    return true;
                                }
                            }
                        }

                        return false;
                    }
                }
            }

            return false;
        }

        public bool IsCheckmate(Color color)
        {
            ChessPiece king = _Pieces[color].Find(p => p is King);
            List<(int x, int y)> moves = king.GetPossibleMoves(this);
            foreach ((int x, int y) in moves)
            {
                ChessBoard copy = Clone();
                copy.Movepiece(king.x, king.y, x, y);
                if (!copy.IsCheck(color))
                {
                    return false;
                }
            }
            return IsCheck(color);
        }

        public bool IsCheck(Color color)
        {
            var oppositeColor = color == Color.White ? Color.Black : Color.White;
            var oppositePieces = _Pieces[oppositeColor];
            foreach (var piece in oppositePieces)
            {
                var moves = piece.GetPossibleMoves(this);
                foreach (var move in moves)
                {
                    if (Getposition(move.x, move.y) is King k && k.color == color)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsGameOver()
        {
            if (IsCheckmate(Color.Black) || IsCheckmate(Color.White))
            {
                return true;
            }

            foreach (var pair in _Pieces)
            {
                var color = pair.Key;
                var pieces = pair.Value;
                foreach (var piece in pieces)
                {
                    var moves = piece.GetPossibleMoves(this);
                    foreach (var move in moves)
                    {
                        ChessBoard copy = Clone();
                        copy.Movepiece(piece.x, piece.y, move.x, move.y);
                        if (!copy.IsCheck(color))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public ChessBoard Clone()
        {
            ChessBoard clone = new ChessBoard();
            clone.Board = new ChessPiece[8, 8];
            clone._Pieces = new Dictionary<Color, List<ChessPiece>>();
            foreach (var pair in _Pieces)
            {
                var color = pair.Key;
                var pieces = pair.Value;
                List<ChessPiece> clonedPieces = new List<ChessPiece>();
                foreach (var piece in pieces)
                {
                    ChessPiece clonedPiece = piece.Clone();
                    clonedPieces.Add(clonedPiece);
                    clone.Board[clonedPiece.x, clonedPiece.y] = clonedPiece;
                }
                clone._Pieces.Add(color, clonedPieces);
            }
            return clone;
        }

    }
}