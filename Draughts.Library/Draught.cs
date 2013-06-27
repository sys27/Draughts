using System;

namespace Draughts.Library
{

    public class Draught
    {

        private BoardPoint point;
        private DraughtType type;
        private Players player;

        public Draught(int x, int y, Players player) : this(x, y, DraughtType.None, player) { }

        public Draught(int x, int y, DraughtType type, Players player) : this(new BoardPoint(y, x), type, player) { }

        public Draught(BoardPoint point, DraughtType type, Players player)
        {
            this.point = point;
            this.type = type;
            this.player = player;
        }

        public BoardPoint Point
        {
            get { return point; }
            set { point = value; }
        }

        public int X
        {
            get
            {
                return point.X;
            }
            set
            {
                point.X = value;
            }
        }

        public int Y
        {
            get
            {
                return point.Y;
            }
            set
            {
                point.Y = value;
            }
        }

        public DraughtType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public Players Player
        {
            get
            {
                return player;
            }
        }

    }

}
