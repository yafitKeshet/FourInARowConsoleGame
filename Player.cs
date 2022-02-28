namespace C21_Ex02_YafitMizrahi_318861960_NivGorsky_206094914
{
    public class Player
    {
        private string m_PlayerName;
        private int m_PlayerScore;
        private bool m_IsHuman;
        private Chip m_Chip;
        private bool m_IsPlayed;

        public Player(string i_Name, bool i_IsHuman, char i_Chip) 
        {
            m_PlayerName = i_Name;
            m_Chip.Type = i_Chip;
            m_IsHuman = i_IsHuman;
            m_PlayerScore = 0;
            m_IsPlayed = false;
        }

        public string Name
        {
            get
            {
                return m_PlayerName;
            }

            set
            {
                m_PlayerName = value;
            }
        }

        public int Score
        {
            get
            {
                return m_PlayerScore;
            }
        }

        public bool IsHuman
        {
            get
            {
                return m_IsHuman;
            }

            set
            {
                m_IsHuman = value;
            }
        }

        public Chip Chip
        {
            get
            {
                return m_Chip;
            }

            set
            {
                m_Chip = value;
            }
        }

        public void Win()
        {
            m_PlayerScore++;
        }

        public bool IsPlayed
        {
            get
            {
                return m_IsPlayed;
            }

            set
            {
                m_IsPlayed = value;
            }
        }
    }
}
