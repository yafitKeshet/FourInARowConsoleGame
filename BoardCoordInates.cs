namespace C21_Ex02_YafitMizrahi_318861960_NivGorsky_206094914
{
    public struct BoardCoordinates
    {
        private int m_Row;
        private int m_Col;

        public BoardCoordinates(int i_Row, int i_Col)
        {
            m_Row = i_Row;
            m_Col = i_Col;
        }

        public int Row
        {
            get
            {
                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }

        public int Col
        {
            get
            {
                return m_Col;
            }

            set
            {
                m_Col = value;
            }
        }

    }
}