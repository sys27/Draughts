using System;

namespace Draughts.Library
{

    public struct BoardPoint
    {

        private int x;
        private int y;

        public BoardPoint(int y, int x)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(BoardPoint left, BoardPoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BoardPoint left, BoardPoint right)
        {
            return !left.Equals(right);
        }

        public bool Equals(BoardPoint other)
        {
            return other.x == x && other.y == y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof(BoardPoint))
                return false;

            return Equals((BoardPoint)obj);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", y, x);
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

    }

}
