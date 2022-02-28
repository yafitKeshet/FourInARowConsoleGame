namespace C21_Ex02_YafitMizrahi_318861960_NivGorsky_206094914
{
    public struct Chip
    {
        private char m_TypeChip;

        public char Type
        {
            get
            {
                return m_TypeChip;
            }

            set
            {
                m_TypeChip = value;
            }
        }

        public static bool operator ==(Chip i_CompareChip1, Chip i_CompareChip2)
        {
            return i_CompareChip1.Type == i_CompareChip2.Type;
        }

        public static bool operator !=(Chip i_CompareChip1, Chip i_CompareChip2)
        {
            return !(i_CompareChip1==i_CompareChip2);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    } 
}
