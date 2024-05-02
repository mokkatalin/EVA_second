using BombGame.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BombGame.Model
{
    public class BombModel
    {
        #region Private fields

        private Field[,] _gameTable;
        private Player player;

        private IPersistence _persistence;

        private Int32 enemyCount = 0;
        private Int32 enemiesDown = 0;
        private Int32 time = 0;

        GameOverEventArgs gameOverEventArgs;

        #endregion

        #region Public properties/ Getters,setters

        public int EnemyCount { get { return enemyCount; } }

        public int TableSize
        {
            get { if (_gameTable == null) return 0;
                else return _gameTable.GetLength(0) - 6; }
        }

        public GameOverEventArgs GameOverEventArgs { get { return gameOverEventArgs; } }

        public int GetEnemiesDown()
        {
            return enemiesDown;
        }

        public void SetEnemiesDown(int e)
        {
            enemiesDown = e;
        }

        public int getTime()
        {
            return time;
        }

        public void setTime(int t)
        {
            time = t;
        }

        public Field this[Int32 x, Int32 y]
        {
            get
            {
                if (x < 0 || x >= TableSize)
                    throw new ArgumentException("Bad column index", nameof(x));
                if (y < 0 || y >= TableSize)
                    throw new ArgumentException("Bad row index", nameof(y));
                return _gameTable[x + 3, y + 3];
            }
        }

        public int getPlayerX()
        {
            return player.GetX-3;
        }
        public int getPlayerY()
        {
            return player.GetY-3;
        }


        #endregion

        #region Events

        public event EventHandler? TableUpdate;

        public event EventHandler<GameOverEventArgs>? GameOver;

        public event EventHandler<GenerateTableEventArgs>? GenerateTable;

        #endregion

        #region Constructors
        public BombModel(IPersistence persistence)
        {
            _persistence = persistence;
        }

        #endregion

        #region Public Methods

        public void LoadGame(string path)
        {
            FieldType[] values = _persistence.Load(path);

            enemyCount = 0;
            enemiesDown = 0;
            time = 0;

            int size = (int)Math.Sqrt(values.Length);
            _gameTable = new Field[size + 6, size + 6];

            if (_gameTable.Length == 0)
                throw new DataException("Error occurred during game loading");
            if (values.Count(value => value == FieldType.Bomb) != 0)
                throw new DataException("Error occurred during game loading");
            
            player = new Player(3, 3);
            

            for (Int32 i = 0; i < _gameTable.GetLength(0); i++)
            {
                for (Int32 j = 0; j < _gameTable.GetLength(1); j++)
                {
                    if (i > 2 && i < _gameTable.GetLength(0) - 3 && j > 2 && j < (_gameTable.GetLength(1) - 3))
                    {
                        Int32 index = (i - 3) * TableSize + (j - 3);
                        _gameTable[i, j] = new Field(values[index]);
                        if (_gameTable[i, j].GetFieldType() == FieldType.Enemy) enemyCount++;                 
                    }
                    else _gameTable[i, j] = new Field(FieldType.Empty);
                }
                
            }

            OnGenerateTable(size);
           // OnTableUpdate();

            gameOverEventArgs = new GameOverEventArgs(FieldType.Wall);
            

        }

        public void ReLoad()
        {
            time++;

            if (GameOverEventArgs.Ftype != FieldType.Wall) throw new Exception("game over");

            for (int i = 3; i < _gameTable.GetLength(0) - 3; i++)
            {
                for (int j = 3; j < _gameTable.GetLength(1) - 3; j++)
                {
                    switch (_gameTable[i, j].GetFieldType())
                    {
                        case FieldType.Empty:
                            break;
                        case FieldType.Wall:
                            break;
                        case FieldType.Bomb:
                            if (_gameTable[i, j].GetTimeLeft() <= 0) BombSetOff(i, j);
                            else _gameTable[i, j].SetTimeLeft(_gameTable[i, j].GetTimeLeft() - 1);
                            break;
                        case FieldType.Enemy:
                            if (!_gameTable[i, j].Moved) MoveEnemy(i, j);
                            break;
                        default:
                            break;
                    }
                    if (_gameTable[i, j].GetSecondary() == FieldType.Bomb)
                    {
                        if (_gameTable[i, j].GetTimeLeft() <= 0) BombSetOff(i, j);
                        else _gameTable[i, j].SetTimeLeft(_gameTable[i, j].GetTimeLeft() - 1);
                    }
                }
            }

            for (int i = 3; i < _gameTable.GetLength(0) - 3; i++)
                for (int j = 3; j < _gameTable.GetLength(1) - 3; j++)
                    _gameTable[i, j].Moved = false;

            CheckGame();
            OnTableUpdate();
            
        }

        public void MovePlayer(Direction d)
        {
            if (CanStepP(d, player.GetX, player.GetY))
            {
                Int32 x = player.GetX;
                Int32 y = player.GetY;
                
                Step(d, ref x, ref y);
                if (_gameTable[x, y].GetFieldType() == FieldType.Enemy)
                {
                    player.GetX = x;
                    player.GetY = y;
                    OnGameOver(FieldType.Enemy);
                    
                }
                else
                {
                    player.GetX = x;
                    player.GetY = y;
                }
            }
        }  

        public void BombDown(Int32 x, Int32 y)
        {
            x += 3;
            y += 3;

            _gameTable[x, y] = new Field(FieldType.Bomb);
        }


        #endregion

        #region Private Methods

        private void MoveEnemy(Int32 x, Int32 y)
        {

            if (x <= 2 || x >= (_gameTable.GetLength(0) - 3) ||
                y <= 2 || y >= ((_gameTable.GetLength(1) - 3)))
            {
                throw new ArgumentOutOfRangeException("Bad column or bad row index");
            }
            if (_gameTable[x, y].GetFieldType() != FieldType.Enemy)
                throw new ArgumentException("Field is not an enemy");

            Field e = _gameTable[x, y];

            if (e.Moved == false)
            {
                Int32 i = 0;
                if (e.GetDirection() == null || !CanStep(e.GetDirection(), x, y))
                {
                    Random r = new Random(DateTime.Now.Millisecond);
                    Int32[] arr = { 0, 1, 2, 3 };
                    arr = arr.OrderBy(x => r.Next()).ToArray();
                    
                    while (i < 4 && (!CanStep(e.GetDirection(), x, y)
                            || e.GetDirection() == null))
                    {
                        e.SetDirection((Direction)arr[i]);
                        i++;

                    }
                }
                if(i < 4)
                {
                    Int32 xo = x;
                    Int32 yo = y;
                    Step(e.GetDirection(), ref x, ref y);

                    if (player.GetX == x && player.GetY == y)
                    {
                        gameOverEventArgs = new GameOverEventArgs(FieldType.Enemy);
                    }

                    if (_gameTable[xo, yo].GetSecondary() == FieldType.Bomb)
                    {
                        int timeleft = _gameTable[xo, yo].GetTimeLeft();
                        if (_gameTable[x, y].GetFieldType() != FieldType.Bomb) _gameTable[xo, yo].SetSecondary(FieldType.Empty);
                        _gameTable[x, y] = _gameTable[xo, yo];
                        _gameTable[xo, yo] = new Field(FieldType.Bomb);
                        _gameTable[xo, yo].SetTimeLeft(timeleft);
                    }
                    else
                    {
                        if (_gameTable[x, y].GetFieldType() == FieldType.Bomb)
                        {
                            _gameTable[xo, yo].SetSecondary(FieldType.Bomb);
                            _gameTable[xo, yo].SetTimeLeft(_gameTable[x, y].GetTimeLeft());
                        }
                        _gameTable[x, y] = _gameTable[xo, yo];
                        _gameTable[xo, yo] = new Field(FieldType.Empty);

                    }
                }
     
                _gameTable[x, y].Moved = true;

            }

        }

        private void BombSetOff(Int32 x, Int32 y)
        {
            for (int i = x - 3; i <= x + 3; i++)
            {
                for (int j = y - 3; j <= y + 3; j++)
                {
                    if (_gameTable[i, j].GetFieldType() == FieldType.Enemy)
                    {
                        enemiesDown++;
                        _gameTable[i, j] = new Field(FieldType.Empty);

                    }
                    if (player.GetX == i && player.GetY == j)
                    {
                        gameOverEventArgs = new GameOverEventArgs(FieldType.Bomb);
                    }

                }
            }
            if(gameOverEventArgs.Ftype != FieldType.Bomb) _gameTable[x, y] = new Field(FieldType.Empty);
        }

        private void CheckGame()
        {
            if(gameOverEventArgs.Ftype == FieldType.Enemy)
            {
                OnGameOver(FieldType.Enemy);
            }
            else if(gameOverEventArgs.Ftype == FieldType.Bomb)
            {
                OnGameOver(FieldType.Bomb);
            }else if (enemiesDown == enemyCount)
            {
                //OnGameWon();
                OnGameOver(FieldType.Empty);
            }
        }

        private Boolean CanStepP(Direction? d, Int32 x, Int32 y)
        {
            Step(d, ref x, ref y);
            return !(x <= 2 || x >= (_gameTable.GetLength(0) - 3) ||
                y <= 2 || y >= ((_gameTable.GetLength(1) - 3))) &&
                _gameTable[x, y].GetFieldType() != FieldType.Wall;
        }

        private Boolean CanStep(Direction? d, Int32 x, Int32 y)
        {
            Step(d, ref x, ref y);
            return !(x <= 2 || x >= (_gameTable.GetLength(0) - 3) ||
                y <= 2 || y >= ((_gameTable.GetLength(1) - 3))) &&
                _gameTable[x, y].GetFieldType() != FieldType.Wall &&
                _gameTable[x, y].GetFieldType() != FieldType.Enemy;
        }

        private void Step(Direction? d, ref Int32 x, ref Int32 y)
        {
            switch (d)
            {
                case Direction.Up:
                    x -= 1;
                    break;
                case Direction.Down:
                    x += 1;
                    break;
                case Direction.Left:
                    y -= 1;
                    break;
                case Direction.Right:
                    y += 1;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Event Triggers

        private void OnGameOver(FieldType ft)
        {
            GameOver?.Invoke(this, new GameOverEventArgs(ft));
        }

        private void OnTableUpdate()
        {
            TableUpdate?.Invoke(this, EventArgs.Empty);
        }

        private void OnGenerateTable(Int32 size)
        {
            GenerateTable?.Invoke(this, new GenerateTableEventArgs(size));
        }

        #endregion
    }
}
