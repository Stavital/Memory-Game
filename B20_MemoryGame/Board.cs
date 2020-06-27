using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_MemoryGame
{
    internal class Board
    {
        private static int m_boardHeight;
        private static int m_boardWidth;
        private static char[,] m_board;
        private static char[,] m_printBoard;
        private static List<string> m_guessedCells;

        // constructor
        internal Board(int i_height, int i_width)
        {
            m_boardHeight = i_height;
            m_boardWidth = i_width;
            m_board = new char[m_boardHeight, m_boardWidth];
            m_printBoard = new char[m_boardHeight, m_boardWidth];
            ClearBoard();
            m_guessedCells = new List<string>();
        }

        internal List<string> GuessedCells
        {
            get
            {
                return m_guessedCells;
            }
        }

        // This function building the board 
        internal string ShowBoard()
        {
            StringBuilder board = new StringBuilder(); // the board 
            char letter = 'A';

            board.Append("    ");
            for (int i = 0; i < m_printBoard.GetLength(1); i++)
            {
                board.Append(letter + "   ");
                letter++;
            }

            board.Append(Environment.NewLine);

            for (int i = 0; i < m_printBoard.GetLength(1) + 1; i++)
            {
                if (i != 0)
                {
                    board.Append("====");
                }
                else
                {
                    board.Append("  ");
                }
            }

            board.Append("=");
            board.Append(Environment.NewLine);

            for (int i = 0; i < m_printBoard.GetLength(0); i++)
            {
                board.Append(i + 1 + " ");

                for (int j = 0; j < m_printBoard.GetLength(1); j++)
                {
                    board.Append("| " + m_printBoard[i, j] + " ");
                }

                board.Append("|  ");
                board.Append(Environment.NewLine);

                for (int j = 0; j < m_printBoard.GetLength(1) + 1; j++)
                {
                    if (j != 0)
                    {
                        board.Append("====");
                    }
                    else
                    {
                        board.Append("  ");
                    }
                }

                board.Append("=");
                board.Append(Environment.NewLine);
            }

            return board.ToString();
        }

        // This function randomly chooses letters between A-Z and set them in the board
        internal void BoardLettersInit()
        {
            HashSet<char> lettersInBoard = new HashSet<char>(); // creating set for the letters we already used
            for (int i = 0; i < m_board.GetLength(0); i++)
            {
                int rowOfTheFirstLocation;
                int colOfTheFirstLocation;
                int rowOfTheSecondLocation;
                int colOfTheSecondLocation;

                for (int j = 0; j < m_board.GetLength(1) / 2; j++)
                {
                    Random rnd = new Random();
                    char randomChar = (char)rnd.Next('A', 'Z'); // choosing a random letter from A-Z 

                    // checkig if the letter is in the set, if it is, we need to choose again
                    while (IsUsed(randomChar, lettersInBoard))
                    {
                        randomChar = (char)rnd.Next('A', 'Z');
                    }

                    rowOfTheFirstLocation = i;
                    colOfTheFirstLocation = j;

                    // choosing first cell for the letter, if the cell is full, choosing again
                    while (!CellIsEmpty(rowOfTheFirstLocation, colOfTheFirstLocation, m_board))
                    {
                        Random cellOne = new Random();
                        rowOfTheFirstLocation = cellOne.Next(m_board.GetLength(0));
                        colOfTheFirstLocation = cellOne.Next(m_board.GetLength(1));
                    }

                    m_board[rowOfTheFirstLocation, colOfTheFirstLocation] = randomChar; // placing the letter in the cell

                    Random cellTwo = new Random();
                    rowOfTheSecondLocation = cellTwo.Next(m_board.GetLength(0));
                    colOfTheSecondLocation = cellTwo.Next(m_board.GetLength(1));

                    // choosing second cell for the letter, if the cell is full, choosing again
                    while (!CellIsEmpty(rowOfTheSecondLocation, colOfTheSecondLocation, m_board))
                    {
                        rowOfTheSecondLocation = cellTwo.Next(m_board.GetLength(0));
                        colOfTheSecondLocation = cellTwo.Next(m_board.GetLength(1));
                    }

                    m_board[rowOfTheSecondLocation, colOfTheSecondLocation] = randomChar; // placing the letter in the cell
                }
            }
        }

        // check if the cell in the matrix is empty
        private static bool CellIsEmpty(int i_row, int i_col, char[,] i_board)
        {
            if (i_board[i_row, i_col] != (char)0)
            {
                return false;
            }

            return true;
        }

        // check if the letter is used
        private static bool IsUsed(char i_letter, HashSet<char> io_lettersInBoard)
        {
            if (io_lettersInBoard.Contains(i_letter))
            {
                return true;
            }

            io_lettersInBoard.Add(i_letter);
            return false;
        }

        // Resets all printBoard values to "  " - empty board
        private static void ClearBoard()
        {
            for (int i = 0; i < m_printBoard.GetLength(0); i++)
            {
                for (int j = 0; j < m_printBoard.GetLength(1); j++)
                {
                    m_printBoard[i, j] = '\0';
                }
            }
        }

        // sets specific cell to be shown
        internal void ShowCell(string i_cell)
        {
            string convertedCell = ConvertStringToMatrixValues(i_cell);
            UpdatePrintBoard("show", convertedCell);
        }

        // sets specific cell to be hidden
        internal void HideCell(string i_cell)
        {
            m_guessedCells.Remove(i_cell);
            string convertedCell = ConvertStringToMatrixValues(i_cell);
            UpdatePrintBoard("hide", convertedCell);
        }

        // updates the values of printBoard by cell and action (show , hide)
        internal void UpdatePrintBoard(string i_action, string i_cell)
        {
            int col = i_cell[0] - '0';
            int row = i_cell[1] - '0';
            row--;
            if (i_action == "show")
            {
                m_printBoard[row, col] = m_board[row, col];
            }

            if (i_action == "hide")
            {
                m_printBoard[row, col] = '\0';
            }
        }

        // checks if the guesses are a match
        internal bool CheckMatch(string i_cellOne, string i_cellTwo)
        {
            string convertedCellOne = ConvertStringToMatrixValues(i_cellOne);
            string convertedCellTwo = ConvertStringToMatrixValues(i_cellTwo);

            int col1 = convertedCellOne[0] - '0';
            int row1 = convertedCellOne[1] - '0';
            row1--;

            int col2 = convertedCellTwo[0] - '0';
            int row2 = convertedCellTwo[1] - '0';
            row2--;
            bool isMatch = false;

            if (m_board[row1, col1] == m_board[row2, col2])
            {
                isMatch = true;
            }
            else
            {
                HideCell(i_cellOne);
                HideCell(i_cellTwo);
            }

            return isMatch;
        }

        // converts cell as giving from the user to a matrix values
        private static string ConvertStringToMatrixValues(string i_cell)
        {
            StringBuilder returnCell = new StringBuilder();
            int charToInt = i_cell[0] - 'A';
            returnCell.Append(charToInt.ToString());
            returnCell.Append(i_cell[1]);
            return returnCell.ToString();
        }
    }
}
