using BombGame.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombGame.Model
{
    public class GameOverEventArgs : EventArgs
    {
        public FieldType Ftype { get; private set; }

        public GameOverEventArgs(FieldType ft)
        {
            Ftype = ft;
        }

    }
}
