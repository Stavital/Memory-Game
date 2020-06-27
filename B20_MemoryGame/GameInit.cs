using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_MemoryGame
{
    internal class GameInit
    {
        private static int m_boardHeight;
        private static int m_boardWidth;
        private static string m_gameMode;
        private static string m_playerOne;
        private static string m_playerTwo;

        internal GameInit(bool i_isFirstGame)
        {
            if (i_isFirstGame == true)
            {
                m_playerOne = PlayerName();
                m_gameMode = GameMode();
            }

            BoardSize();
        }

        internal int BoardHeight
        {
            get
            {
                return m_boardHeight;
            }
        }

        internal int BoardWidth
        {
            get
            {
                return m_boardWidth;
            }
        }

        internal string Mode
        {
            get
            {
                return m_gameMode;
            }
        }

        internal string PlayerOne
        {
            get
            {
                return m_playerOne;
            }
        }

        internal string PlayerTwo
        {
            get
            {
                return m_playerTwo;
            }
        }

        // choosing player namr for the game
        private static string PlayerName()
        {
            System.Console.WriteLine("Please enter player's name");
            string playerName = System.Console.ReadLine();

            return playerName;
        }

        // choosing board size
        private static void BoardSize()
        {
            System.Console.WriteLine("please choose 4 or 6 for the board height");
            string boardHeight = System.Console.ReadLine();

            while (!IsValidNumber(boardHeight))
            {
                System.Console.WriteLine("Invalid number, try again");
                boardHeight = System.Console.ReadLine();
            }

            m_boardHeight = int.Parse(boardHeight);

            System.Console.WriteLine("please choose 4 or 6 for the board width");
            string boardWidth = System.Console.ReadLine();

            while (!IsValidNumber(boardWidth))
            {
                System.Console.WriteLine("Invalid number, try again");
                boardWidth = System.Console.ReadLine();
            }

            m_boardWidth = int.Parse(boardWidth);
        }

        // choosing game mode - playing agaisnt a player or againt the computer
        internal static string GameMode()
        {
            System.Console.WriteLine(Environment.NewLine + "Choose game mode:");
            System.Console.WriteLine("Versus Computer press S");
            System.Console.WriteLine("Versus another player press M");
            string GameMode = System.Console.ReadLine();

            while (!IsVaildMode(GameMode))
            {
                System.Console.WriteLine("Invaild number, please try again");
                GameMode = System.Console.ReadLine();
            }

            string playAgainst = string.Empty;
            if (GameMode == "M")
            {
                m_playerTwo = PlayerName();
                playAgainst = "player";
            }
            else
            {
                playAgainst = "computer";
            }

            return playAgainst;
        }

        // this function check if the mode of the game is either 1 or 0
        internal static Boolean IsVaildMode(string i_number)
        {
            if (i_number != "M" && i_number != "S")
            {
                return false;
            }

            return true;
        }

        // this function check if the numbers for the board height and width is either 4 or 6 
        private static Boolean IsValidNumber(string i_number)
        {
            int currentNumber = 0;
            bool isNumber = int.TryParse(i_number, out currentNumber);

            if (!isNumber)
            {
                return false;
            }

            if (currentNumber != 4 && currentNumber != 6)
            {
                return false;
            }

            return true;
        }
    }
}
