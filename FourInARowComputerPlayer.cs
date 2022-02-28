using System;
using System.Collections.Generic;

namespace C21_Ex02_YafitMizrahi_318861960_NivGorsky_206094914
{
    public class FourInARowComputerPlayer
    {
        private enum eInsertOptionsNulls
        {
            WinCol = -5,
            DefenceCol,
            LongestSequence = 1
        }

        public static int CalculateAColToInsertAChip(GameLogic i_GameLogic)
        {
           return pickBestMove(i_GameLogic);
        }

        private static  List<int> getValidLocationsToInsertChipInBoard(GameBoard i_GameBoard)
        {
            List<int> validLocations = new List<int>();
            for(int i = 0; i < i_GameBoard.NumberOfCols; i++)
            {
                if(i_GameBoard.IsThereEmptyRow(i))
                {
                    validLocations.Add(i+1);
                }
            }

            return validLocations;
        }

        private static Chip createCumputerChip()
        {
            Chip chip = new Chip();
            chip.Type = 'O';

            return chip;
        }

        private static int pickBestMove(GameLogic i_GameBoard)
        {
            Chip computerChip = createCumputerChip();
            List<int> validLocationsToInsertComputerChipInBoard = getValidLocationsToInsertChipInBoard(i_GameBoard.Board);
            List<int> validLocationsToInsertComputerChipInBoardThatNotHelpsTheEnemyWin = new List<int>();
            int winCol = (int)eInsertOptionsNulls.WinCol;
            int longestSequence = (int)eInsertOptionsNulls.LongestSequence;
            int defenceCol = tryToDefence(i_GameBoard, validLocationsToInsertComputerChipInBoard);
            List<int> longestSequenceLocations = new List<int>();

            foreach(int col in validLocationsToInsertComputerChipInBoard)
            {
                i_GameBoard.InsertNewChip(col, computerChip);
                if(i_GameBoard.IsAWin(col))
                {
                    winCol = col;
                    i_GameBoard.DeleteChipFromCol(col);
                    break;
                }

                if(!isLastInsertMakeTheEnemyWin(i_GameBoard))
                {
                    validLocationsToInsertComputerChipInBoardThatNotHelpsTheEnemyWin.Add(col);
                    int lengthSequence = getLongestSequnceInCol(i_GameBoard, col - 1);

                    if(lengthSequence > longestSequence)
                    {
                        longestSequenceLocations = new List<int>();

                        longestSequenceLocations.Add(col);
                        longestSequence = lengthSequence;
                    }
                    else if(lengthSequence == longestSequence &&
                            lengthSequence != (int)eInsertOptionsNulls.LongestSequence)
                    {
                        longestSequenceLocations.Add(col);
                    }
                }

                i_GameBoard.DeleteChipFromCol(col);
            }

            return chooseTheBestMove(winCol, defenceCol, longestSequence, 
                validLocationsToInsertComputerChipInBoardThatNotHelpsTheEnemyWin, 
                longestSequenceLocations);
        }

        private static bool isLastInsertMakeTheEnemyWin(GameLogic i_GameBoard)
        {
            List<int> validLocations = getValidLocationsToInsertChipInBoard(i_GameBoard.Board);
            
            return (tryToDefence(i_GameBoard,validLocations) != (int)eInsertOptionsNulls.DefenceCol);
        }

        private static int chooseTheBestMove(int i_WinCol, int i_DefenceCol, int i_LongestSequence, List<int> i_ValidLocations, List<int> i_LongestSequenceLocations)
        {
            int choosenCol;
            Random randomCalculator = new Random();

            if (i_WinCol != (int)eInsertOptionsNulls.WinCol)
            {
                choosenCol = i_WinCol;
            }
            else if(i_DefenceCol != (int)eInsertOptionsNulls.DefenceCol)
            {
                choosenCol = i_DefenceCol;
            }
            else if(i_LongestSequence != (int)eInsertOptionsNulls.LongestSequence)
            {
                int index = randomCalculator.Next(0, i_LongestSequenceLocations.Count);
                choosenCol = i_LongestSequenceLocations[index];
            }
            else
            {
                int index = randomCalculator.Next(0, i_ValidLocations.Count);
                choosenCol = i_ValidLocations[index];
            }

            return choosenCol;
        }

        private static int getLongestSequnceInCol(GameLogic i_GameBoard, int i_Col)
        {
            BoardCoordinates currentPositionToCheck= new BoardCoordinates(i_GameBoard.Board.GetFirstEmptyRowInACol(i_Col)+1,i_Col);
            Chip playerChipToCount = new Chip();
            playerChipToCount.Type = 'O';
            int maxSequanceDiagonal = maxSequencesInDiagonal(i_GameBoard, currentPositionToCheck);
            int maxSequenceHorizon= maxSequencesHorizon(i_GameBoard, currentPositionToCheck);
            int maxSequanceVertical= maxSequencesVertical(i_GameBoard, currentPositionToCheck);

            return Math.Max(Math.Max(maxSequanceVertical, maxSequanceDiagonal), maxSequenceHorizon);
        }

        private static int maxSequencesVertical(GameLogic i_GameBoard, BoardCoordinates i_CurrentPositionToCheck)
        {
            int sequenceLength = 1;
            int sizeOfMaxSequence = i_GameBoard.SequenceLengthForWinning - 2;
            Chip playerChipToCount = i_GameBoard.Board.GetChipInPosition(i_CurrentPositionToCheck);

            i_GameBoard.CountSequenceInTheDownDirection(ref sequenceLength, i_CurrentPositionToCheck, playerChipToCount, sizeOfMaxSequence);
            i_GameBoard.CountSequenceInTheUpDirection(ref sequenceLength, i_CurrentPositionToCheck, playerChipToCount, sizeOfMaxSequence);

            return sequenceLength;
        }

        private static int maxSequencesHorizon(GameLogic i_GameBoard, BoardCoordinates i_CurrentPositionToCheck)
        {
            int sequenceLength = 1;
            int sizeOfMaxSequence = i_GameBoard.SequenceLengthForWinning - 2;
            Chip playerChipToCount = i_GameBoard.Board.GetChipInPosition(i_CurrentPositionToCheck);

            i_GameBoard.CountSequenceInTheRightDirection(ref sequenceLength, i_CurrentPositionToCheck, playerChipToCount, sizeOfMaxSequence);
            i_GameBoard.CountSequenceInTheLeftDirection(ref sequenceLength, i_CurrentPositionToCheck, playerChipToCount, sizeOfMaxSequence);

            return sequenceLength; 
        }

        private static int maxSequencesInDiagonal(GameLogic i_GameBoard , BoardCoordinates i_CurrentPositionToCheck)
        {
            int sequenceLengthTopRightTDownLeft = 1;
            int sequenceLengthTopLeftToDoenRight = 1;
            int sizeOfMaxSequence = i_GameBoard.SequenceLengthForWinning - 2;
            Chip playerChipToCount = i_GameBoard.Board.GetChipInPosition(i_CurrentPositionToCheck);

            i_GameBoard.CountSequenceInTheTopRightDirection(ref sequenceLengthTopRightTDownLeft, i_CurrentPositionToCheck, playerChipToCount, sizeOfMaxSequence);
            i_GameBoard.CountSequenceInTheDownLeftDirection(ref sequenceLengthTopRightTDownLeft, i_CurrentPositionToCheck, playerChipToCount, sizeOfMaxSequence);
            i_GameBoard.CountSequenceInTheTopLeftDirection(ref sequenceLengthTopLeftToDoenRight, i_CurrentPositionToCheck, playerChipToCount, sizeOfMaxSequence);
            i_GameBoard.CountSequenceInTheDownRightDirection(ref sequenceLengthTopLeftToDoenRight, i_CurrentPositionToCheck, playerChipToCount, sizeOfMaxSequence);

            return Math.Max(sequenceLengthTopLeftToDoenRight, sequenceLengthTopRightTDownLeft);
        }

        private static int tryToDefence(GameLogic i_GameBoard, List<int> i_ValidLocations)
        {
            Chip chip = new Chip();
            chip.Type = 'X';
            int bestCol = (int)eInsertOptionsNulls.DefenceCol;

            foreach (int col in i_ValidLocations)
            {
                if(i_GameBoard.Board.IsThereEmptyRow(col-1))
                {
                    i_GameBoard.InsertNewChip(col, chip);

                    if(i_GameBoard.IsAWin(col))
                    {
                        bestCol = col;
                        i_GameBoard.DeleteChipFromCol(col);
                        break;
                    }

                    i_GameBoard.DeleteChipFromCol(col);
                }
            }

            return bestCol;
        }

    }
}