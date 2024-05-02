using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombGame.Model
{
    public class GenerateTableEventArgs
    {
        public int size { get; private set; }

        public GenerateTableEventArgs(int size)
        {
            this.size = size;
        }
    }
}
