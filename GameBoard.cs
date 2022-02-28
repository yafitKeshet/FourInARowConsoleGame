namespace C21_Ex02_YafitMizrahi_318861960_NivGorsky_206094914
{
    public class GameBoard
    {
        private Chip[,] m_Board;
        private int[] m_FirstEmptyRowInTheCol;

        public GameBoard(int i_Rows, int i_Cols)
        {
            InitBoard(i_Rows, i_Cols);
        }

        public int NumberOfRows
        {
            get
            {
                return m_Board.GetLength(0);
            }
        }

        public int NumberOfCols
        {
            get
            {
                return m_Board.GetLength(1);
            }
        }

        public void InitBoard(int i_Rows, int i_Cols)
        {
            m_Board = new Chip[i_Rows, i_Cols];
            m_FirstEmptyRowInTheCol = new int[i_Cols];

            for (int i = 0; i < i_Cols; ++i)
            {
                m_FirstEmptyRowInTheCol[i] = i_Rows - 1;
            }
        }

        public void ClearBoard()
        {
            for (int i = 0; i < m_Board.GetLength(0); i++)
            {
                for (int j = 0; j < m_Board.GetLength(1); j++)
                {
                    m_Board[i, j].Type = ' ';
                }
            }

            for (int j = 0; j < m_Board.GetLength(1); j++)
            {
                m_FirstEmptyRowInTheCol[j] = m_Board.GetLength(0) - 1;
            }
        }

        public bool IsColToInsertWithinColsRange(int i_ColToInsert)
        {
            bool result = false;
            int numberOfColsInBoard = m_Board.GetLength(1);

            if (i_ColToInsert >= 0 && i_ColToInsert < numberOfColsInBoard)
            {
                result = true;
            }

            return result;
        }

        public bool IsThereEmptyRow(int i_ColToInsert)
        {
            return m_FirstEmptyRowInTheCol[i_ColToInsert] >= 0;
        }

        public void InsertNewChip(int i_Col, Chip i_ChipToInsert)
        {
            int rowToPlaceTheNewChip = m_FirstEmptyRowInTheCol[i_Col];

            m_FirstEmptyRowInTheCol[i_Col]--;
            m_Board[rowToPlaceTheNewChip, i_Col] = i_ChipToInsert;
        }

        public void DeleteChipFromCol(int i_Col)
        {
            m_FirstEmptyRowInTheCol[i_Col]++;
            int rowToRemoveTheChip = m_FirstEmptyRowInTheCol[i_Col];
            m_Board[rowToRemoveTheChip, i_Col].Type = ' ';

        }
        public bool IsBoardFull()
        {
            bool result = true;

            foreach (int firstEmptyRow in m_FirstEmptyRowInTheCol)
            {
                if (firstEmptyRow >= 0) 
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public int GetFirstEmptyRowInACol(int i_Col)
        {
            return m_FirstEmptyRowInTheCol[i_Col];
        }

        public Chip GetChipInPosition(BoardCoordinates i_Position)
        {
            return m_Board[i_Position.Row, i_Position.Col];
        }

        public bool IsPositionInBoard(BoardCoordinates i_Position)
        {
            bool isWithinColsRange = i_Position.Col >= 0 && i_Position.Col < m_Board.GetLength(1);
            bool isWithinRowsRange = i_Position.Row >= 0 && i_Position.Row < m_Board.GetLength(0);

            return isWithinRowsRange && isWithinColsRange;
        }

    }
}
