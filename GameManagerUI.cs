using System;
using System.Diagnostics.CodeAnalysis;
using Ex02.ConsoleUtils;
using  System.Text;

namespace C21_Ex02_YafitMizrahi_318861960_NivGorsky_206094914
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GameManagerUI
    {
        private enum eGameModes
        {
            Play,
            Tie,
            Win,
            Quit
        }

        public void PlayFourInARow()
        {
            printWelcome();
            int rowSize = getValidRowsSizeFromUser();
            int colSize = getValidColsSizeFromUser();
            Player player1 = getPlayerDataFromUser('X');
            int userEnemy = getValidUserChoiceForAnEnemy();
            bool isSecondPlayerHuman = userEnemy != 1;
            Player player2 = isSecondPlayerHuman ? getPlayerDataFromUser('O') : new Player("Computer", false, 'O');
            GameLogic gameRound = new GameLogic(rowSize, colSize);

            startGame(gameRound, player1, player2);
        }

        private void startGame(GameLogic i_GameRound, Player i_Player1, Player i_Player2)
        {
           playUntilGameIsOver(i_GameRound, i_Player1, i_Player2);
           printGameStatistics(i_Player1, i_Player2);
            if(isWantsOneMoreGame())
            {
                i_GameRound.NewGame();
                i_Player1.IsPlayed = false;
                i_Player2.IsPlayed = false;
                startGame(i_GameRound, i_Player1, i_Player2);
            }

            printGoodBye();
        }

        private void printGoodBye()
        {
            Console.WriteLine(@"Thanks for playing,
GoodBye :)");
        }

        private void gameStatesMenu(eGameModes i_GameOverMode, Player i_CurrentPlayer, Player i_NotCurrentPlayer)
        {
            switch(i_GameOverMode)
            {
                case eGameModes.Quit:
                    Console.WriteLine("{0} quit, {1} win!", i_CurrentPlayer.Name, i_NotCurrentPlayer.Name);
                    i_NotCurrentPlayer.Win();
                    break;
                case eGameModes.Win:
                    Console.WriteLine("{0} win!", i_CurrentPlayer.Name);
                    i_CurrentPlayer.Win();
                    break;
                case eGameModes.Tie:
                    Console.WriteLine("There is a tie!");
                    break;
            }
        }

        private void playUntilGameIsOver(GameLogic i_GameRound, Player i_Player1, Player i_Player2)
        {
            eGameModes stateOfGame = eGameModes.Play;

            while (stateOfGame == eGameModes.Play)
            {
                Screen.Clear();
                printBoard(i_GameRound.Board);
                Player currentPlayerTurn = i_Player1.IsPlayed ? i_Player2 : i_Player1;
                Player playerDidNotTurn = i_Player1.IsPlayed ? i_Player1 : i_Player2;
                int col;
                bool isQuit = promptUserForValidMoveOrQuit(i_GameRound, currentPlayerTurn, out col);
                ifPlayerDidNotQuitInsertHisMove(i_GameRound, isQuit, currentPlayerTurn, col);
                Screen.Clear();
                printBoard(i_GameRound.Board);
                updateStateOfTheGame(isQuit, ref stateOfGame, i_GameRound, col);
                gameStatesMenu(stateOfGame, currentPlayerTurn, playerDidNotTurn);
                currentPlayerTurn.IsPlayed = true;
                playerDidNotTurn.IsPlayed = false;
            }
        }

        private void updateStateOfTheGame(bool i_IsQuit, ref eGameModes io_StateOfTheGame,
                                          GameLogic i_GameRound, int i_Col)
        {
            if (i_IsQuit)
            {
                io_StateOfTheGame = eGameModes.Quit;
            }
            else
            {
                if (i_GameRound.IsTie(i_Col))
                {
                    io_StateOfTheGame = eGameModes.Tie;
                }
                else
                {
                    io_StateOfTheGame = i_GameRound.IsAWin(i_Col) ? eGameModes.Win : eGameModes.Play;
                }
            }
        }

        private void printGameStatistics(Player i_Player1, Player i_Player2)
        {
            Console.WriteLine(@"{0} score: {1}
{2} score: {3}
", i_Player1.Name,i_Player1.Score,i_Player2.Name,i_Player2.Score);
        }

        private void ifPlayerDidNotQuitInsertHisMove(GameLogic i_GameRound,bool i_IsQuit, Player i_Player, int i_Col)
        {
            if (!i_IsQuit)
            {
                i_GameRound.InsertNewChip(i_Col, i_Player.Chip);
            }
        }

        private bool promptUserForValidMoveOrQuit(GameLogic i_GameRound, Player i_Player, out int o_Col)
        {
            bool isValidInput = false;
            bool isQuit = false;
            o_Col = 0;

            if (!i_Player.IsHuman)
            {
                o_Col = i_GameRound.GetComputerMove();
            }
            else
            {
                Console.WriteLine(
                    "It is {0} turn, please choose col to insert chip or enter Q to quit", i_Player.Name);
                while(!isValidInput && !isQuit)
                {
                    string stringColInput = Console.ReadLine();
                    isQuit = getValidIntInputFromUserAndCheckIfHeQuit(ref stringColInput, out o_Col);

                    if(!isQuit && i_GameRound.IsColInRangeOfBoard(o_Col))
                    {
                        isValidInput = i_GameRound.IsAvailabilityInsertColumn(o_Col);
                        if(!isValidInput)
                        {
                            Console.WriteLine("You chose a col that is full. please try again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("You chose a col that is not in the range. please try again.");
                    }
                }
            }

            return isQuit;
        }

        private bool isWantsOneMoreGame()
        {
            Console.WriteLine(@"would you like to continue to play?
1.Yes
2.No");
            int userInput = getValidUserChoice1Or2();

            return userInput == 1;
        }

        private bool getValidIntInputFromUserAndCheckIfHeQuit(ref string io_UserInput, out int o_IntValue)
        {
            bool isQuit = false;

            while (!int.TryParse(io_UserInput, out o_IntValue) && !isQuit )
            {
                if(checkIfPlayerQuit(io_UserInput))
                {
                    isQuit = true;
                }
                else
                {
                    printInputIsNotADigitAndReadAgain(ref io_UserInput);
                }
            }

            return isQuit;
        }

        private void printInputIsNotADigitAndReadAgain(ref string io_UserInput)
        {
            Console.WriteLine("Not a number try again");
            io_UserInput = Console.ReadLine();
        }

        private bool checkIfPlayerQuit(string i_UserChoise)
        {
            return i_UserChoise == "Q";
        }

        private void printBoard(GameBoard i_Board)
        {
            int numOfCols = i_Board.NumberOfCols;
            int numOfRows = i_Board.NumberOfRows;
            StringBuilder boardOutput = new StringBuilder();

            boardOutput.Append(makeTopLabel(numOfCols));
            boardOutput.Append(makeMainBoard(i_Board, numOfRows, numOfCols));
            Console.WriteLine(boardOutput);
        }

        private StringBuilder makeMainBoard(GameBoard i_Board, int i_NumOfRows, int i_NumOfCols)
        {
            StringBuilder mainBoard = new StringBuilder();

            for (int i = 0 ; i < i_NumOfRows ; i++)
            {
                for (int j = 0; j< i_NumOfCols ; j++)
                {
                    Chip chipInPosition = i_Board.GetChipInPosition(new BoardCoordinates(i, j));
                    mainBoard.AppendFormat(@"| {0} ", chipInPosition.Type);
                }

                mainBoard.Append("|");
                mainBoard.AppendFormat("{0}=", Environment.NewLine);
                for (int k = 0; k < i_NumOfCols; k++)
                {
                    mainBoard.Append("====");
                }

                mainBoard.AppendFormat("{0}", Environment.NewLine);
            }

            return mainBoard;
        }

        private StringBuilder makeTopLabel(int i_NumOfRows)
        {
            StringBuilder topLabel = new StringBuilder();
           topLabel.Append(" ");

            for (int i = 1; i <= i_NumOfRows; i++)
            {
                topLabel.AppendFormat(" {0}  ", i );
            }

            topLabel.AppendFormat("{0}", Environment.NewLine);

            return topLabel;
        }

        private Player getPlayerDataFromUser(char i_ChipType)
        {
            Console.WriteLine("Please enter your name");
            string playerName = Console.ReadLine();

            return new Player(playerName, true, i_ChipType);
        }

        private int getValidUserChoice1Or2()
        {
            bool isValidInput = false;
            const int k_DefaultValue = 1;
            int userInput = k_DefaultValue;
            string textInput = Console.ReadLine();

            while (!isValidInput)
            {
                bool isNum = int.TryParse(textInput, out userInput);

                if (isNum)
                {
                    if (userInput == 1 || userInput == 2)
                    {
                        isValidInput = true;
                    }
                }

                if (!isValidInput)
                {
                    Console.WriteLine("Please enter a valid selection. Either 1 or 2 ");
                    textInput = Console.ReadLine();
                }
            }

            return userInput;
        }

        private int getValidUserChoiceForAnEnemy()
        {
            Console.WriteLine(
                @"Against whom you want to play?
1.The computer
2.Another player");

            return getValidUserChoice1Or2();
        }

        private int getValidColsSizeFromUser()
        {
            Console.WriteLine("Please enter num of cols - a single integer to indicate board dimensions - an integer between 4 to 8");
            string stringSizeInput = Console.ReadLine();

            return checkIfSizeIsValidAndReceivesASizeFromUserUntilTheSizeIsValidAndConvertToInt(stringSizeInput);
        }

        private int getValidRowsSizeFromUser()
        {
            Console.WriteLine("Please enter num of rows - a single integer to indicate board dimensions - an integer between 4 to 8");
            string stringSizeInput = Console.ReadLine();

            return checkIfSizeIsValidAndReceivesASizeFromUserUntilTheSizeIsValidAndConvertToInt(stringSizeInput);
        }

        private int checkIfSizeIsValidAndReceivesASizeFromUserUntilTheSizeIsValidAndConvertToInt(string i_Size)
        {
            bool isValidBoardSize = false;
            const int k_MinBoardSize = 4;
            const int k_MaxBoardSize = 8;
            int userInput = 0;

            while (!isValidBoardSize)
            {
                bool isNum = int.TryParse(i_Size, out userInput);

                if (isNum)
                {
                    if (userInput >= k_MinBoardSize && userInput <= k_MaxBoardSize)
                    {
                        isValidBoardSize = true;
                    }
                }

                if (!isValidBoardSize)
                {
                    Console.WriteLine("Please enter a single integer to indicate board dimensions - an integer between 4 to 8");
                    i_Size = Console.ReadLine();
                }
            }

            return userInput;
        }

        private void printWelcome()
        {
            string outPutScreen = @"
  ,---. .---.  .-. .-.,---.       ,-..-. .-.      .--.      ,---.    .---.  .-.  .-. 
 | .-'/ .-. ) | | | || .-.\      |(||  \| |     / /\ \     | .-.\  / .-. ) | |/\| | 
 | `-.| | |(_)| | | || `-'/      (_)|   | |    / /__\ \    | `-'/  | | |(_)| /  \ | 
 | .-'| | | | | | | ||   (       | || |\  |    |  __  |    |   (   | | | | |  /\  | 
 | |  \ `-' / | `-')|| |\ \      | || | |)|    | |  |)|    | |\ \  \ `-' / |(/  \ | 
 )\|   )---'  `---(_)|_| \)\     `-'/(  (_)    |_|  (_)    |_| \)\  )---'  (_)   \| 
(__)  (_)                (__)      (__)                        (__)(_)              
  ";
            Console.WriteLine(outPutScreen);
        }

    }
}
