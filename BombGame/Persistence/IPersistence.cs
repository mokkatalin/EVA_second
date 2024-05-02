using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombGame.Persistence
{
    public interface IPersistence
    {
        public FieldType[] Load(String path);
    }
}
