using System;
using System.IO;

namespace Draughts.Library
{

    public static class BoardSerializer
    {

        public static void Serialize(DraughtsGame game, string path)
        {
            if (game == null)
                throw new ArgumentNullException("board");
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException();

            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write((byte)game.CurrentTurn);
                    bw.Write((byte)game.GameBoard.LeftDraughtPlayerOne);
                    bw.Write((byte)game.GameBoard.LeftDraughtPlayerTwo);

                    foreach (Draught item in game.GameBoard.Draughts)
                    {
                        if (item != null)
                        {
                            bw.Write((byte)item.Y);
                            bw.Write((byte)item.X);
                            bw.Write((byte)item.Type);
                            bw.Write((byte)item.Player);
                        }
                    }
                }
            }
        }

        public static DraughtsGame Deserialize(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException();

            Draught[,] draughts = new Draught[8, 8];
            Board board;
            DraughtsGame game;

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    Players currentTurn = (Players)br.ReadByte();
                    int leftOne = br.ReadByte();
                    int leftTwo = br.ReadByte();

                    for (int i = 0; i < leftOne + leftTwo; i++)
                    {
                        int drY = br.ReadByte();
                        int drX = br.ReadByte();
                        DraughtType type = (DraughtType)br.ReadByte();
                        Players player = (Players)br.ReadByte();

                        draughts[drY, drX] = new Draught(new BoardPoint(drY, drX), type, player);
                    }

                    board = new Board(draughts, leftOne, leftTwo);
                    game = new DraughtsGame() { CurrentTurn = currentTurn, GameBoard = board };
                }
            }

            return game;
        }

    }

}
