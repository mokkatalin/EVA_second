using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldType = BombGame.Persistence.FieldType;

namespace BombGame.Model
{
    public class Field
    {
        private FieldType _fieldType;
        private FieldType secondary;
        private int timeLeft = -1;
        private Direction? direction = null;
        private Boolean moved = false;

        public Field(FieldType fieldType)
        {
            _fieldType = fieldType;
            if (_fieldType == FieldType.Bomb) timeLeft = 3;
        }

        public FieldType GetFieldType() { return _fieldType; }

        public FieldType GetSecondary() { return secondary; }

        public void SetFieldType(FieldType f)
        {
            if(_fieldType == FieldType.Wall || f == FieldType.Wall)
            {
                throw new FieldAccessException();
            }else if(f == FieldType.Wall)
            {
                throw new ArgumentException();
            }
            else
            {
                _fieldType = f;
            }
        }

        public void SetSecondary(FieldType f)
        {
            if (_fieldType == FieldType.Wall || f == FieldType.Wall)
            {
                throw new FieldAccessException();
            }
            else if (f == FieldType.Wall)
            {
                throw new ArgumentException();
            }
            else
            {
                secondary = f;
            }
        }

        public int GetTimeLeft()
        {
            if (_fieldType == FieldType.Bomb || secondary == FieldType.Bomb)
            {
                return timeLeft;
            }
            else
            {
                throw new FieldAccessException(); 
            }
        }

        public void SetTimeLeft(int t)
        {
            if (_fieldType != FieldType.Bomb && secondary != FieldType.Bomb)
            {
                throw new FieldAccessException();
            }
            else if (t < 0)
            {
                throw new ArgumentException();
            }
            else
            {
                timeLeft = t;
            }
        }

        public Direction? GetDirection()
        {
            if (_fieldType == FieldType.Enemy)
            {
                return direction;
            }
            else
            {
                throw new FieldAccessException();
            }
        }

        public void SetDirection(Direction d)
        {
            if (_fieldType != FieldType.Enemy)
            {
                throw new FieldAccessException(); 
            }
            else
            {
                direction = d;
            }
        }

        public Boolean Moved { get => moved; set => moved = value; }

    }
}
