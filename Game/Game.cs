using Chess.Board;
using Chess.Chesspiece;
using Chess.Pieces;

namespace Chess.Game
{
    public class Game
    {
        public ChessBoard Board { get; set; }
        public Color Turn { get; set; }
        public List<string> History { get; set; }

        public Game()
        {
            Board = new ChessBoard();
            Turn = Color.White;
            History = new List<string>();
        }

        public void Start()
        {
            Console.WriteLine("Welcome to Chess!");
            Console.WriteLine();
            Console.WriteLine();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ChessPiece piece = Board.Getposition(i, j);
                    if (piece == null)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        Console.WriteLine(piece + "  ," + piece.color + "(" + piece.x + " ," + piece.y + ")");
                    }
                }
                Console.WriteLine();
            }
        }

        public void Play()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Enter your moves in algebraic notation, such as g2f2, or type 'quit' to exit.\n(Row: A to H) (Column: 1 to 8) \n " +
                "If you want to make a castle move, enter \"castle move\".");
                Console.Write($"\n{Turn} to move:  ");
                string input = Console.ReadLine();

                if (input == "quit")
                {
                    End();
                    return;
                }

                if (input == "castle move")
                {
                    switch (Turn)
                    {
                        case Color.White:

                            if (CastleMove())
                            {
                                Console.WriteLine();
                                Console.WriteLine("castle move is did ");
                                Console.WriteLine();
                                for (int i = 0; i < 8; i++)
                                {
                                    for (int j = 0; j < 8; j++)
                                    {
                                        ChessPiece pieceOnBoard = Board.Getposition(i, j);
                                        if (pieceOnBoard == null)
                                        {
                                            Console.Write("  ");
                                        }
                                        else
                                        {
                                            Console.WriteLine(pieceOnBoard + "  ," + pieceOnBoard.color + "(" + pieceOnBoard.x + " ," + pieceOnBoard.y + ")");
                                        }
                                    }
                                    Console.WriteLine();
                                }
                                Turn = Turn == Color.White ? Color.Black : Color.White;
                                continue;
                            }
                            Console.WriteLine();
                            Console.WriteLine("You can't do this move. Please make another move.");
                            continue;

                        case Color.Black:

                            if (CastleMove())
                            {
                                Console.WriteLine();
                                Console.WriteLine("castle move is did ");
                                Console.WriteLine();
                                for (int i = 0; i < 8; i++)
                                {
                                    for (int j = 0; j < 8; j++)
                                    {
                                        ChessPiece pieceOnBoard = Board.Getposition(i, j);
                                        if (pieceOnBoard == null)
                                        {
                                            Console.Write("  ");
                                        }
                                        else
                                        {
                                            Console.WriteLine(pieceOnBoard + "  ," + pieceOnBoard.color + "(" + pieceOnBoard.x + " ," + pieceOnBoard.y + ")");
                                        }
                                    }
                                    Console.WriteLine();
                                }
                                Turn = Turn == Color.White ? Color.Black : Color.White;
                                continue;
                            }
                            Console.WriteLine();
                            Console.WriteLine("You can't do this move. Please make another move.");
                            continue;
                    }
                }

                if (input == null || input.Length != 4) // Check if the input is valid
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input. Please enter a valid move.");
                    continue; // Skip the rest of the loop and ask for another input
                }

                int sourceX = input[0] - 'a';
                int sourceY = input[1] - '1';
                int destinationX = input[2] - 'a';
                int destinationY = input[3] - '1';

                if (sourceX < 0 || sourceX > 7 || sourceY < 0 || sourceY > 7 || destinationX < 0 || destinationX > 7 || destinationY < 0 || destinationY > 7)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid input. Please enter a valid move.");
                    continue;
                }

                ChessPiece piece = Board.Getposition(sourceX, sourceY);

                if (piece == null || piece.color != Turn)
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid move. Please select a piece of your color.");
                    continue;
                }

                if (!piece.IsValidMove(destinationX, destinationY, Board))
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid move. Please select a valid destination.");
                    continue;
                }

                Board.Movepiece(sourceX, sourceY, destinationX, destinationY);

                if (piece is King)
                {
                    if (Board.IsCheck(Turn))
                    {
                        Board.Movepiece(destinationX, destinationY, sourceX, sourceY);
                        Console.WriteLine("{0} king will check (or is check), so you can't make this move.", Turn);
                        continue;
                    }
                }

                bool Checkpoint()
                {
                    if (History != null && History.Count > 0)
                    {
                        char x2 = input[2];
                        char y2 = input[3];
                        char[] chars = { x2, y2 };
                        string nowmove = new string(chars);
                        string lastmove = History.Last();
                        char x1 = lastmove[2];
                        char y1 = lastmove[3];
                        char[] chars2 = { x1, y1 };
                        string _lastmove = new string(chars2);

                        if (nowmove != _lastmove)
                        {
                            return true;
                        }
                    }
                    return false;
                }

                if (Board.IsCheck(Turn) && Checkpoint())
                {
                    Board.Movepiece(destinationX, destinationY, sourceX, sourceY);
                    Console.WriteLine("{0} king will check (or is check), so you can't make this move.", Turn);
                    continue;
                }

                Console.WriteLine();
                Console.WriteLine();

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        ChessPiece pieceOnBoard = Board.Getposition(i, j);
                        if (pieceOnBoard == null)
                        {
                            Console.Write("  ");
                        }
                        else
                        {
                            Console.WriteLine(pieceOnBoard + "  ," + pieceOnBoard.color + "(" + pieceOnBoard.x + " ," + pieceOnBoard.y + ")");
                        }
                    }
                    Console.WriteLine();
                }

                if (Turn == Color.White)
                {
                    Console.WriteLine();
                    Console.WriteLine("{0} is moved", Turn);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("{0} is moved", Turn);
                }

                Turn = Turn == Color.White ? Color.Black : Color.White;

                if (Board.IsCheck(Turn))
                {
                    Console.WriteLine();
                    Console.WriteLine("{0} is check ", Turn);
                }

                History.Add(input);

                if (Board.IsGameOver() && !Board.CanRemoveChecker(Turn) && !Board.CanBlockChecker(Turn))
                {
                    Console.WriteLine();
                    Color Turn1 = Turn == Color.Black ? Color.White : Color.Black;
                    Console.WriteLine($"{Turn1} wins!");
                    End();
                    break;
                }
            }
        }

        public bool CastleMove()
        {
            string king = "h5";
            string rook = "h8";
            int findKing = History.IndexOf(king);
            int findRook = History.IndexOf(rook);
            // right white
            if (Board.Getposition(7, 5) == null && Board.Getposition(7, 6) == null &&
                Board.Getposition(7, 4) is King && Board.Getposition(7, 7) is Rook && findKing == -1 && findRook == -1
                && Turn == Color.White)
            {
                Board.Movepiece(7, 4, 7, 5);
                if (!Board.IsCheck(Color.White))
                {
                    Board.Movepiece(7, 5, 7, 6);
                    if (!Board.IsCheck(Color.White))
                    {
                        Board.Movepiece(7, 7, 7, 5);
                        return true;
                    }
                    else
                    {
                        Board.Movepiece(7, 6, 7, 4);
                    }
                }
                else
                {
                    Board.Movepiece(7, 5, 7, 4);
                }
            }


            string king1 = "h5";
            string rook1 = "h1";
            int findKing1 = History.IndexOf(king1);
            int findRook1 = History.IndexOf(rook1);
            // left white
            if (Board.Getposition(7, 1) == null && Board.Getposition(7, 2) == null && Board.Getposition(7, 3) == null
                && Board.Getposition(7, 4) is King && Board.Getposition(7, 0) is Rook && findKing1 == -1 && findRook1 == -1
                && Turn == Color.White)
            {
                Board.Movepiece(7, 4, 7, 3);
                if (!Board.IsCheck(Color.White))
                {
                    Board.Movepiece(7, 3, 7, 2);
                    if (!Board.IsCheck(Color.White))
                    {
                        Board.Movepiece(7, 0, 7, 3);
                        return true;
                    }
                    else
                    {
                        Board.Movepiece(7, 2, 7, 4);
                    }
                }
                else
                {
                    Board.Movepiece(7, 3, 7, 4);
                }
            }


            string king2 = "a5";
            string rook2 = "a8";
            int findKing2 = History.IndexOf(king2);
            int findRook2 = History.IndexOf(rook2);
            // right black
            if (Board.Getposition(0, 5) == null && Board.Getposition(0, 6) == null &&
                Board.Getposition(0, 4) is King && Board.Getposition(0, 7) is Rook && findKing2 == -1 && findRook2 == -1
                && Turn == Color.Black)
            {
                Board.Movepiece(0, 4, 0, 5);
                if (!Board.IsCheck(Color.White))
                {
                    Board.Movepiece(0, 5, 0, 6);
                    if (!Board.IsCheck(Color.White))
                    {
                        Board.Movepiece(0, 7, 0, 5);
                        return true;
                    }
                    else
                    {
                        Board.Movepiece(0, 6, 0, 4);
                    }
                }
                else
                {
                    Board.Movepiece(0, 5, 0, 4);
                }
            }


            string king3 = "a5";
            string rook3 = "a1";
            int findKing3 = History.IndexOf(king3);
            int findRook3 = History.IndexOf(rook3);
            // left black
            if (Board.Getposition(0, 1) == null && Board.Getposition(0, 2) == null && Board.Getposition(0, 3) == null
                && Board.Getposition(0, 4) is King && Board.Getposition(0, 0) is Rook && findKing3 == -1 && findRook3 == -1
                && Turn == Color.Black)
            {
                Board.Movepiece(0, 4, 0, 3);
                if (!Board.IsCheck(Color.White))
                {
                    Board.Movepiece(0, 3, 0, 2);
                    if (!Board.IsCheck(Color.White))
                    {
                        Board.Movepiece(0, 0, 0, 3);
                        return true;
                    }
                    else
                    {
                        Board.Movepiece(0, 2, 0, 4);
                    }
                }
                else
                {
                    Board.Movepiece(0, 3, 0, 4);
                }
            }

            return false;
        }

        public void End()
        {
            Console.WriteLine();
            Console.WriteLine("Thank you for playing Chess!");
            Console.WriteLine("Here are your moves:");
            foreach (string move in History)
            {
                Console.WriteLine(move);
            }
        }

    }
}

