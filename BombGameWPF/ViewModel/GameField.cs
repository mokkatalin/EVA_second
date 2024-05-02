using BombGame.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombGameWPF.ViewModel
{
    public class GameField : ViewModelBase
    {
     
        private Boolean player = false;
        private String _color;
        private String _text;

        public Boolean IsPlayer
        {
            get { return player; }
            set
            {
                if(player != value)
                {
                    player = value;
           
                }
            }
        }

        
      
        public String Color
        {
            get { return _color; }
            set
            {
                if(_color != value)
                {
                    _color = value;
                    OnPropertChanged();
                }
            }
        }

        public String Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertChanged();
                }
            }
        }

        public Int32 X { get; set; }

        public Int32 Y { get; set; }

    }
}
