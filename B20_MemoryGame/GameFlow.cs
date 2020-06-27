using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_MemoryGame
{
    internal class GameFlow
    {
        private static GameInit m_memoryGame;
        private static Board m_gameBoard;
        private static Player m_playerOne;
        private static Player m_playerTwo;
        private static PcPlayer m_pcPlayer;
        private static int m_totalScore;
        private static int m_scoreLimit;
        private static bool m_running;

        internal GameFlow(bool i_isFirstGame)
        {
            // init the game
            m_memoryGame = new GameInit(i_isFirstGame);
            m_gameBoard = new Board(m_memoryGame.BoardHeight, m_memoryGame.BoardWidth);
            m_playerOne = new Player(m_memoryGame.PlayerOne);
            m_scoreLimit = (m_memoryGame.BoardHeight * m_memoryGame.BoardWidth) / 2;
            m_totalScore = 0;

            // Setting rival according to game mode
            if (m_memoryGame.Mode == "player")
            {
                m_playerTwo = new Player(m_memoryGame.PlayerTwo);
            }
            else
            {
                m_pcPlayer = new PcPlayer();
            }

            m_running = true;

            // start game
            StartGame();
        }

        // starting the game
        internal static void StartGame()
        {
            m_gameBoard.BoardLettersInit();
            string currentPlayer = m_playerOne.Name;
            string otherPlayer = string.Empty;
            if (m_memoryGame.Mode == "player")
            {
                otherPlayer = m_playerTwo.Name;
            }
            else
            {
                otherPlayer = "computer";
            }

            // While the game is still running
            while (m_running)
            {
                // Re-Prints the updated board every round
                PrintBoard();

                // making the first guess
                string firstCell = Guess(currentPlayer);

                // show the first guess in the board
                m_gameBoard.ShowCell(firstCell);
                PrintBoard();

                // making the secong guess
                string secondCell = Guess(currentPlayer);

                // showing the second guess in the board
                m_gameBoard.ShowCell(secondCell);
                PrintBoard();

                // Check for match
                bool isMatch = m_gameBoard.CheckMatch(firstCell, secondCell);
                if (!isMatch)
                {
                    System.Threading.Thread.Sleep(2000); // showing the board for 2 seconds
                    m_gameBoard.HideCell(firstCell); // hide first guess
                    m_gameBoard.HideCell(secondCell); // hide second guess
                    SwitchTurns(ref currentPlayer, ref otherPlayer); // switch turns
                }
                else
                {
                    AddPoint(currentPlayer); // adding poing to the current player
                    m_totalScore++; // adding the total points

                    // If the total points eqal to the limit of score, then the game is over
                    if (m_totalScore == m_scoreLimit)
                    {
                        m_running = false;
                    }
                }
            }

            Console.WriteLine(GetWinner()); // showing game summary
        }

        // handling the Guesses of the players
        internal static string Guess(string i_name)
        {
            string cell = string.Empty;

            // if user chose to play againt computer, we use the Guess function at PcPlayer
            if (i_name == "computer")
            {
                System.Console.WriteLine("Computer's turn");
                System.Threading.Thread.Sleep(1200);
                cell = PcPlayer.Guess(m_memoryGame.BoardHeight, m_memoryGame.BoardWidth, m_gameBoard);
            }
            else
            {
                System.Console.WriteLine(i_name + " ,please guess a cell");
                cell = System.Console.ReadLine();

                // check if the guess is valid
                int[] validity = IsValidCell(cell);
                while (validity[0] == 1 || validity[1] == 1)
                {
                    if (validity[1] == 1)
                    {
                        System.Console.WriteLine("The input is illegal, please try again");
                        cell = System.Console.ReadLine();
                        validity = IsValidCell(cell);
                        continue;
                    }

                    if (validity[0] == 1)
                    {
                        System.Console.WriteLine("The input is out of board's bounds");
                        cell = System.Console.ReadLine();
                    }

                    validity = IsValidCell(cell);
                }
            }

            m_gameBoard.GuessedCells.Add(cell);
            return cell;
        }

        // check is the input is valid
        private static int[] IsValidCell(string i_cell)
        {
            int[] validity = new int[2]; // arr for checking what is wrong with the input

            // check if the input is Q for exiting the game
            if (i_cell == "Q")
            {
                System.Console.WriteLine("Thank you for playing, press Any key to exit");
                string finish = Console.ReadLine();
                Environment.Exit(0);
            }

            // if the size of the string is diff then 2, return
            if (i_cell.Length != 2)
            {
                validity[1] = 1;
                return validity;
            }

            // check if the char is upper letter in string[0]
            if (!char.IsUpper(i_cell[0]))
            {
                validity[1] = 1;
            }

            // check if the char is a digit int string[1]
            if (!char.IsDigit(i_cell[1]))
            {
                validity[1] = 1;
            }

            int row = (int)char.GetNumericValue(i_cell[1]);
            int column = i_cell[0] - 'A';

            // check that the input column is between the board bouns
            if (column > m_memoryGame.BoardWidth || column < 0)
            {
                validity[0] = 1;
            }

            // check that the input row is between the board bouns
            if (row > m_memoryGame.BoardHeight || row <= 0)
            {
                validity[0] = 1;
            }

            // check if the input is not used
            if (m_gameBoard.GuessedCells.Contains(i_cell))
            {
                validity[1] = 1;
            }

            return validity;
        }

        // prints the board
        internal static void PrintBoard()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine(m_gameBoard.ShowBoard());
        }

        // Prints the winner of the game
        internal static string GetWinner()
        {
            StringBuilder winnerMessage = new StringBuilder();

            // Checks winner according to Game mode
            if (m_memoryGame.Mode == "player")
            {
                winnerMessage.Append(string.Format("The Winner is {0} With {1} Points", m_playerOne.Name, m_playerOne.Score));
                winnerMessage.Append(Environment.NewLine);
                winnerMessage.Append(string.Format("The Loser is {0} With {1} Points", m_playerTwo.Name, m_playerTwo.Score));

                if (m_playerOne.Score < m_playerTwo.Score)
                {
                    winnerMessage.Clear();
                    winnerMessage.Append(string.Format("The Winner is {0} With {1} Points", m_playerTwo.Name, m_playerTwo.Score));
                    winnerMessage.Append(Environment.NewLine);
                    winnerMessage.Append(string.Format("The Loser is the {0} With {1} Points", m_playerOne.Name, m_playerOne.Score));
                }
            }
            else if (m_memoryGame.Mode == "computer")
            {
                winnerMessage.Append(string.Format("The Winner is {0} With {1} Points", m_playerOne.Name, m_playerOne.Score));
                winnerMessage.Append(Environment.NewLine);
                winnerMessage.Append(string.Format("The Loser is {0} With {1} Points", m_pcPlayer.Name, m_pcPlayer.Score));

                if (m_playerOne.Score < m_pcPlayer.Score)
                {
                    winnerMessage.Clear();
                    winnerMessage.Append(string.Format("The Winner is {0} With {1} Points", m_pcPlayer.Name, m_pcPlayer.Score));
                    winnerMessage.Append(Environment.NewLine);
                    winnerMessage.Append(string.Format("The Loser is {0} With {1} Points", m_pcPlayer.Name, m_playerOne.Score));
                }
            }

            if (m_playerOne.Score == m_scoreLimit / 2)
            {
                winnerMessage.Clear();
                winnerMessage.Append(string.Format("Both players have the same score of {0}", m_playerOne.Score));
            }

            return winnerMessage.ToString();
        }

        // Adding point to the current player for a right  guess
        internal static void AddPoint(string i_player)
        {
            if (m_memoryGame.Mode == "player")
            {
                if (i_player == m_playerOne.Name)
                {
                    m_playerOne.Score++;
                }
                else
                {
                    m_playerTwo.Score++;
                }
            }
            else
            {
                if (i_player == m_playerOne.Name)
                {
                    m_playerOne.Score++;
                }
                else
                {
                    m_pcPlayer.Score++;
                }
            }
        }

        // Switch turns between players
        internal static void SwitchTurns(ref string io_currentTurn, ref string io_otherTurn)
        {
            string tempTurn = io_currentTurn;
            io_currentTurn = io_otherTurn;
            io_otherTurn = tempTurn;
        }
    }
}
