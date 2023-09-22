// Eric Sällström .NET23

using System.Data.Common;

namespace ChessBoard2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // setting a unicode standard output
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            bool buildingChessBoard = true;

            // as long as the chess board have not been drawn the loop keeps going
            while (buildingChessBoard)
            {
                Console.WriteLine("Välkommen till schackbrädet!" +
                    "\nVar god mata in en siffra för att bestämma storleken schackbrädet.");

                int.TryParse(Console.ReadLine(), out int boardSize);

                // if user input is not a number between 1-26 or consists of a whitespace the variable boardSize returns 0
                // else, it returns the number deciding how many rows/columns the chess board will have
                if (boardSize <= 0 || boardSize > 26)
                {
                    Console.WriteLine("Fel input! Du måste ange storleken på brädet med ett " +
                        "\npositivt heltal och du kan inte ange ett tal större än 26." +
                        "\nTryck \"ENTER\" och försök igen.");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    // a chess board consists of as many rows as columns, therefore am I assigning the value 
                    // of boardSize to both int variables, rows and columns
                    int rows = boardSize;
                    int columns = boardSize;

                    // variables rows and columns then decides the size of the 2D string-array chessBoard, in 
                    // which the building blocks of the chess board will be stored, and ultimately, drawn from
                    string[,] chessBoard = new string[rows, columns];

                    Console.WriteLine("Nu är det dags att välja hur schackbrädet ska se ut." +
                        "\nOm ett fält lämnas tomt kommer schackbrädets defaultvärde att skrivas ut.");

                    // I'm using Thread.Sleep on numerous occasions throughout my 
                    // code to promt the user to read all the instructions
                    Thread.Sleep(1500);

                    Console.WriteLine("Hur ska vita rutor se ut?");
                    string whiteSq = Console.ReadLine();

                    Console.WriteLine("Hur ska svarta rutor se ut?");
                    string blackSq = Console.ReadLine();

                    // checks if the user input is valid and then stores the chess boards 
                    // black and white squares, based on the symbols the user entered
                    GetBoardSizeAndSqSymbols(chessBoard, whiteSq, blackSq);

                    // assigning the return value of GetChessPiece to chessPiece
                    string chessPiece = GetChessPiece();

                    while (true)
                    {
                        // assigning the return value of GetChessPilacement to piecePlacement
                        string piecePlacement = GetChessPiecePlacement();

                        // piecePlacement is then used as a parameter in both methods below whereas
                        // variables row and column is assigned the return value from respectively method
                        int row = PlaceChessPieceRow(piecePlacement);
                        int column = PlaceChessPieceColumn(piecePlacement);

                        // if variables column or row is assigned -1 then the coordinates was not in a 
                        // correct format and the user is prompt to try again
                        if (column == -1 || row == -1)
                        {
                            Console.WriteLine($"Fel input! Koordinaten måste bestå av en bokstav följt av en siffra.");
                            Thread.Sleep(1500);
                            continue;
                        }
                        else if (row + 1 > boardSize || column > boardSize)
                        {
                            Console.WriteLine("Fel input! Du kan inte placera pjäsen utanför schackbrädet.");
                            Thread.Sleep(1500);
                            continue;
                        }
                        else
                        {
                            // finally the DrawChessBoard-method is used to print the chesss board to the console,
                            // it receives four variables as in-parameters 
                            DrawChessBoard(chessPiece, row, column, chessBoard);
                            break;
                        }
                    }

                    // when the chess board has been printed we jump out of the while-loop
                    // and the program is terminated
                    buildingChessBoard = false;
                }
            }
        }

        static string GetChessPiecePlacement()
        {
            string piecePlacement = "";

            // infinite loop to prompt the user to enter the right value
            while (true)
            {
                Console.WriteLine("Var vill du placera din pjäs? (Ex. D6)");
                piecePlacement = Console.ReadLine();

                // if the user enters the right value it's assigned to piecePlacement and we break out of the while-loop
                // else we contiune to ask the user for the desired value
                // the input cannot be empty/null and no less than 1 and no more than 3 charachters long
                if (!String.IsNullOrWhiteSpace(piecePlacement) && piecePlacement.Length > 1 && piecePlacement.Length < 4)
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Fel input! Koordinaterna kunde inte indexeras." +
                        $"\nPositionen anges genom att skriva en bokstav följt av ett tal.");
                    Thread.Sleep(1500);
                    continue;
                }
            }

            return piecePlacement;
        }

        static string GetChessPiece()
        {
            string chessPiece = "";

            // infinite loop to prompt the user to enter the right value
            while (true)
            {
                Console.WriteLine("Hur ska din pjäs se ut? Du kan mata in ett valfritt tecken.");
                chessPiece = Console.ReadLine();

                // if the user enters an invalid value, we contiune to ask the user for a valid one
                // else, the user enters the right value and break out of the while-loop                
                if (String.IsNullOrWhiteSpace(chessPiece) || chessPiece.Length != 1)
                {
                    Console.WriteLine($"Fel input! Du måste mata in ett tecken för att din schackpjäs ska kunna användas.");
                    Thread.Sleep(1500);
                    continue;
                }
                else
                {
                    break;
                }
            }

            return chessPiece;
        }

        static int PlaceChessPieceColumn(string piecePlacement)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // removes all the charachters in piecePlacement, 
            // starting at index 1 (leaves the first char intact)
            string columnPlacement = piecePlacement.Remove(1);

            // checks the remaining charachter's index against the
            // alphabet, and returns an integer based on 
            int column = alphabet.IndexOf(columnPlacement.ToUpper());

            return column;
        }

        static int PlaceChessPieceRow(string piecePlacement)
        {
            // creates a substring of the piecePlacement's second charachter in the string
            // and it's then assigned to rowPlacement
            string rowPlacement = piecePlacement.Substring(1);

            // if converting of rowPlacement to an int succeds, row is assigned the correct 
            // row-placement of the chess piece
            // else row will be assigned 0
            int.TryParse(rowPlacement, out int row);

            // returns value of row - 1 because the index of the chessBoard array starts with 0
            return row - 1;
        }

        static void DrawChessBoard(string chessPiece, int row, int column, string[,] chessBoard)
        {
            // outer for loop responsible for iterating through the chess boards rows
            for (int rows = 0; rows < chessBoard.GetLength(0); rows++)
            {
                // inner for loop responsible for iterating through the chess boards columns
                for (int columns = 0; columns < chessBoard.GetLength(1); columns++)
                {
                    // user's chess piece is written to the console based on 
                    // its coordinates from row and column
                    if (rows == row && columns == column)
                    {
                        Console.Write(chessPiece + " ");
                    }
                    // rest of chess board is being written to the console
                    else
                    {
                        Console.Write(chessBoard[rows, columns] + " ");
                    }
                }
                Console.WriteLine();
            }
        }

        static void GetBoardSizeAndSqSymbols(string[,] chessBoard, string whiteSq, string blackSq)
        {
            // if one of the string variables is null or contains any white space
            // a default value for that strings is assigned
            if (String.IsNullOrWhiteSpace(whiteSq))
            {
                whiteSq = "□";
            }
            if (String.IsNullOrWhiteSpace(blackSq))
            {
                blackSq = "■";
            }

            // outer for loop responsible for iterating through the chess boards rows
            for (int rows = 0; rows < chessBoard.GetLength(0); rows++)
            {
                // inner for loop responsible for iterating through the chess boards columns
                for (int columns = 0; columns < chessBoard.GetLength(1); columns++)
                {
                    // if rows + columns = even number, the value of whiteSquare is assigned
                    if ((rows + columns) % 2 == 0)
                    {
                        chessBoard[rows, columns] = whiteSq;
                    }
                    // if rows + columns = odd number, the value of blackSquares is assigned
                    else
                    {
                        chessBoard[rows, columns] = blackSq;
                    }
                }
            }
        }
    }
}