using System.Diagnostics.CodeAnalysis;

namespace C21_Ex02_YafitMizrahi_318861960_NivGorsky_206094914
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GameLogic
    {
        private readonly int r_SequenceLengthForWinning = 4;
        private readonly GameBoard r_Board;

        public GameLogic(int i_NumberOfRows, int i_NumberOfCols)
        {
            r_Board = new GameBoard(i_NumberOfRows, i_NumberOfCols);
        }

        public int SequenceLengthForWinning
        {
            get
            {
                return r_SequenceLengthForWinning;
            }
        }

        public GameBoard Board
        {
            get
            {
                return r_Board;
            }
        }

        public void NewGame()
        {
            r_Board.ClearBoard();
        }

        public bool IsColInRangeOfBoard(int i_ColToInsert)
        {
            i_ColToInsert--;

            return r_Board.IsColToInsertWithinColsRange(i_ColToInsert);
        }

        public bool IsAvailabilityInsertColumn(int i_ColToInsert)
        {
            i_ColToInsert--;

            return r_Board.IsThereEmptyRow(i_ColToInsert);
        }

        public void InsertNewChip(int i_ColToInsert, Chip i_ChipToInsert)
        {
            i_ColToInsert--;
            r_Board.InsertNewChip(i_ColToInsert, i_ChipToInsert);
        }

        public void DeleteChipFromCol(int i_ColToDelete)
        {
            i_ColToDelete--;
            r_Board.DeleteChipFromCol(i_ColToDelete);
        }

        public bool IsTie(int i_Col)
        {
            return r_Board.IsBoardFull() && IsAWin(i_Col) == false;
        }

        public bool IsAWin(int i_LastColInserted)
        {
            i_LastColInserted--;
            BoardCoordinates position = getPositionOfLastInsertedChip(i_LastColInserted);

            return isAWinningSequence(position);
        }

        public int GetComputerMove()
        {
            int colToInsert = FourInARowComputerPlayer.CalculateAColToInsertAChip(this);

            while (!IsAvailabilityInsertColumn(colToInsert))
            {
                colToInsert = FourInARowComputerPlayer.CalculateAColToInsertAChip(this);
            }

            return colToInsert;
        }

        private BoardCoordinates getPositionOfLastInsertedChip(int i_LastColInserted)
        {
            BoardCoordinates position = new BoardCoordinates();
            position.Col = i_LastColInserted;
            position.Row = r_Board.GetFirstEmptyRowInACol(i_LastColInserted) + 1;

            return position;
        }

        private bool isAWinningSequence(BoardCoordinates i_Position)
        {
            return isAPartOfHorizontalSequenceInAnyDirection(i_Position,r_SequenceLengthForWinning-1)
                   || isAPartOfVerticalSequenceInAnyDirection(i_Position, r_SequenceLengthForWinning-1)
                   || isAPartOfDiagonalSequenceInAnyDirection(i_Position, r_SequenceLengthForWinning-1);
        }

        private bool isAPartOfHorizontalSequenceInAnyDirection(BoardCoordinates i_PositionOfBeginningOfSequence,int i_LengthOfMaxSequence)
        {
            int sequenceLength = 1;
            Chip playerChipToCount = r_Board.GetChipInPosition(i_PositionOfBeginningOfSequence);

            CountSequenceInTheRightDirection(ref sequenceLength, i_PositionOfBeginningOfSequence, playerChipToCount, i_LengthOfMaxSequence);
            CountSequenceInTheLeftDirection(ref sequenceLength, i_PositionOfBeginningOfSequence, playerChipToCount, i_LengthOfMaxSequence);

            return sequenceLength >= r_SequenceLengthForWinning;
        }

        private bool isAPartOfVerticalSequenceInAnyDirection(BoardCoordinates i_PositionOfBeginningOfSequence, int i_LengthOfMaxSequence)
        {
            int sequenceLength = 1;
            Chip playerChipToCount = r_Board.GetChipInPosition(i_PositionOfBeginningOfSequence);

            CountSequenceInTheDownDirection(ref sequenceLength, i_PositionOfBeginningOfSequence, playerChipToCount, i_LengthOfMaxSequence);
            CountSequenceInTheUpDirection(ref sequenceLength, i_PositionOfBeginningOfSequence, playerChipToCount, i_LengthOfMaxSequence);

            return sequenceLength >= r_SequenceLengthForWinning;
        }

        private bool isAPartOfDiagonalSequenceInAnyDirection(BoardCoordinates i_PositionOfLastChipInserted, int i_LengthOfMaxSequence)
        {
            int sequenceLengthTopRightToLeft = 1;
            int sequenceLengthTopLeftToRight = 1;
            Chip playerChipToCount = r_Board.GetChipInPosition(i_PositionOfLastChipInserted);

            CountSequenceInTheTopRightDirection(ref sequenceLengthTopRightToLeft, i_PositionOfLastChipInserted, playerChipToCount, i_LengthOfMaxSequence);
            CountSequenceInTheDownLeftDirection(ref sequenceLengthTopRightToLeft, i_PositionOfLastChipInserted, playerChipToCount, i_LengthOfMaxSequence);
            CountSequenceInTheTopLeftDirection(ref sequenceLengthTopLeftToRight, i_PositionOfLastChipInserted, playerChipToCount, i_LengthOfMaxSequence);
            CountSequenceInTheDownRightDirection(ref sequenceLengthTopLeftToRight, i_PositionOfLastChipInserted, playerChipToCount, i_LengthOfMaxSequence);

            return sequenceLengthTopLeftToRight >= r_SequenceLengthForWinning
                   || sequenceLengthTopRightToLeft >= r_SequenceLengthForWinning;
        }

        private bool increaseLengthAndSaysIfAPartOfSequnce(ref int io_SequenceLength, BoardCoordinates i_CurrentPositionToCheck, Chip i_PlayerChipToCount)
        {
            bool ans = false;

            if(r_Board.IsPositionInBoard(i_CurrentPositionToCheck)
               && i_PlayerChipToCount == r_Board.GetChipInPosition(i_CurrentPositionToCheck))
            {
                io_SequenceLength++;
                ans = true;
            }

            return ans;
        }

        public void CountSequenceInTheRightDirection(ref int io_SequenceLength, BoardCoordinates i_CurrentPositionToCheck, Chip i_PlayerChipToCount, int i_SequenceLengthRange)
        {
            for (int i = 1; i <= i_SequenceLengthRange; i++)
            {
                i_CurrentPositionToCheck.Col++;

                if (!increaseLengthAndSaysIfAPartOfSequnce(ref io_SequenceLength,
                        i_CurrentPositionToCheck, i_PlayerChipToCount))
                {
                    break;
                }
            }
        }

        public void CountSequenceInTheLeftDirection(ref int io_SequenceLength, BoardCoordinates i_CurrentPositionToCheck, Chip i_PlayerChipToCount, int i_SequenceLengthRange)
        {
            for (int i = 1; i <=  i_SequenceLengthRange; i++)
            {
                i_CurrentPositionToCheck.Col--;

                if (!increaseLengthAndSaysIfAPartOfSequnce(ref io_SequenceLength,
                         i_CurrentPositionToCheck, i_PlayerChipToCount))
                {
                    break;
                }
            }
        }

        public void CountSequenceInTheDownDirection(ref int io_SequenceLength, BoardCoordinates i_CurrentPositionToCheck, Chip i_PlayerChipToCount, int i_SequenceLengthRange)
        {
            for (int i = 1; i <= i_SequenceLengthRange; i++)
            {
                i_CurrentPositionToCheck.Row++;

                if (!increaseLengthAndSaysIfAPartOfSequnce(ref io_SequenceLength,
                        i_CurrentPositionToCheck, i_PlayerChipToCount))
                {
                    break;
                }
            }
        }

        public void CountSequenceInTheUpDirection(ref int io_SequenceLength, BoardCoordinates i_CurrentPositionToCheck, Chip i_PlayerChipToCount, int i_SequenceLengthRange)
        {
            for (int i = 1; i <= i_SequenceLengthRange; i++)
            {
                i_CurrentPositionToCheck.Row--;

                if (!increaseLengthAndSaysIfAPartOfSequnce(ref io_SequenceLength,
                        i_CurrentPositionToCheck, i_PlayerChipToCount))
                {
                    break;
                }
            }
        }

        public void CountSequenceInTheTopRightDirection(ref int io_SequenceLength, BoardCoordinates i_CurrentPositionToCheck, Chip i_PlayerChipToCount, int i_SequenceLengthRange)
        {
            for (int i = 1; i <= i_SequenceLengthRange; i++)
            {
                i_CurrentPositionToCheck.Col++;
                i_CurrentPositionToCheck.Row--;

                if (!increaseLengthAndSaysIfAPartOfSequnce(ref io_SequenceLength,
                        i_CurrentPositionToCheck, i_PlayerChipToCount))
                {
                    break;
                }
            }
        }

        public void CountSequenceInTheDownLeftDirection(ref int io_SequenceLength, BoardCoordinates i_CurrentPositionToCheck, Chip i_PlayerChipToCount, int i_SequenceLengthRange)
        {
            for (int i = 1; i <= i_SequenceLengthRange; i++)
            {
                i_CurrentPositionToCheck.Col--;
                i_CurrentPositionToCheck.Row++;

                if (!increaseLengthAndSaysIfAPartOfSequnce(ref io_SequenceLength,
                        i_CurrentPositionToCheck, i_PlayerChipToCount))
                {
                    break;
                }
            }
        }

        public void CountSequenceInTheTopLeftDirection(ref int io_SequenceLength, BoardCoordinates i_CurrentPositionToCheck, Chip i_PlayerChipToCount, int i_SequenceLengthRange)
        {
            for (int i = 1; i <= i_SequenceLengthRange; i++)
            {
                i_CurrentPositionToCheck.Col--;
                i_CurrentPositionToCheck.Row--;

                if (!increaseLengthAndSaysIfAPartOfSequnce(ref io_SequenceLength,
                        i_CurrentPositionToCheck, i_PlayerChipToCount))
                {
                    break;
                }
            }
        }

        public void CountSequenceInTheDownRightDirection(ref int io_SequenceLength, BoardCoordinates i_CurrentPositionToCheck, Chip i_PlayerChipToCount, int i_SequenceLengthRange)
        {
            for (int i = 1; i <= i_SequenceLengthRange; i++)
            {
                i_CurrentPositionToCheck.Col++;
                i_CurrentPositionToCheck.Row++;

                if (!increaseLengthAndSaysIfAPartOfSequnce(ref io_SequenceLength,
                        i_CurrentPositionToCheck, i_PlayerChipToCount))
                {
                    break;
                }
            }
        }

    }
}
