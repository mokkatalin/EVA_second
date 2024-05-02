using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombGame.Model
{
    public class Player
    {
        private Int32 X;
        private Int32 Y;

        public int GetX { get => X; set => X = value; }
        public int GetY { get => Y; set => Y = value; }

        public Player(Int32 i, Int32 j)
        {
            X = i;
            Y = j;
        }
    }
}
