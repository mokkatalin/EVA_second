using BombGame.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using FieldType = BombGame.Persistence.FieldType;

namespace BombGameWPF.ViewModel
{
    public class BombViewModel : ViewModelBase
    {
        #region Fields

        private BombModel _model;
        private String _path;
        private String _stopResume;
        private Int32 _size = 0;

        #endregion

        #region Properties

        public String Path { get { return _path; } private set { } }


        public Int32 Size { get { return _size; } set { if (_size != value) { _size = value; OnPropertChanged(nameof(Size)); } } }

        public String StopResume { get { return _stopResume; }
            set
            { 
                if(_stopResume != value)
                {
                    _stopResume = value;
                    OnPropertChanged(nameof(StopResume));
                }
            } 
        }

        public DelegateCommand ChangePathCommand { get; private set; }
            
        public DelegateCommand NewGameCommand { get; private set; }

        public DelegateCommand ExitGameCommand { get; private set; }

        public DelegateCommand StopGameCommand { get; private set; }

        public ObservableCollection<GameField> Fields { get; private set; } 

        public String GameTime { get { return TimeSpan.FromSeconds(_model.getTime()).ToString("g"); } }

        public Int32 EnemiesDown { get { return _model.GetEnemiesDown(); } }

        #endregion

        #region Events

        public event EventHandler? NewGame;

        public event EventHandler? ExitGame;

        public event EventHandler? StopGameEvent;

        #endregion

        #region Constructors

        public BombViewModel(BombModel model)
        {
            _model = model;
            _model.TableUpdate += new EventHandler(Model_TableUpdate);
          
            _model.GenerateTable += new EventHandler<GenerateTableEventArgs>(Model_GenerateTable);

            //parancsok kezelese
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            ExitGameCommand = new DelegateCommand(param => OnExitGame());
            StopGameCommand = new DelegateCommand(param => OnStopGame());

            ChangePathCommand = new DelegateCommand(param => OnChangePath(param.ToString()));
        }

        #endregion

        #region Private methods

        private void RefreshTable()
        {
            foreach(GameField field in Fields)
            {
                
                switch (_model[field.X, field.Y].GetFieldType())
                {
                    case FieldType.Bomb: field.Color = "Pink";
                        field.Text = "💣";
                        break;
                    case FieldType.Empty: field.Color = "White";
                        field.Text = "";
                        break;
                    case FieldType.Enemy: field.Color = "White";
                        field.Text = "👽";
                        break;
                    case FieldType.Wall: field.Color = "Black";
                        field.Text = "";
                        break;
                    
                }
                
                field.IsPlayer = (_model.getPlayerX() == field.X && _model.getPlayerY() == field.Y);
                if (field.IsPlayer) field.Text = "💃";
           
            }

            for (int i = 0; i < _model.TableSize; i++)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                    if (_model[i, j].GetFieldType() == FieldType.Bomb || _model[i, j].GetSecondary() == FieldType.Bomb)
                    {
                        for (int a = i - 3; a <= i + 3; a++)
                        {
                            for (int b = j - 3; b <= j + 3; b++)
                            {
                                if ((b >= 0) && (b < _model.TableSize) && (a >= 0) && (a < _model.TableSize))
                                {
                                    if (Fields[a * _model.TableSize + b].Color != "Black")
                                    {
                                        Fields[a * _model.TableSize + b].Color = "Pink";
                                    }
                                }
                            }
                        }
                    }
                }
            }

            OnPropertChanged(nameof(Fields));
            OnPropertChanged(nameof(GameTime));
            OnPropertChanged(nameof(EnemiesDown));
            
        }

        #endregion

        #region Game event handlers

        private void Model_GenerateTable(object? sender, GenerateTableEventArgs e)
        {
            Fields = new ObservableCollection<GameField>();
            for (Int32 i = 0; i < e.size; i++)
            {
                for (Int32 j = 0; j < e.size; j++)
                {
                    Fields.Add(new GameField
                    {
                        IsPlayer = false,
                        X = i,
                        Y = j,
                    });
                    
                }
            }
            RefreshTable();
        }

        private void Model_TableUpdate(object? sender, EventArgs e)
        {
            OnPropertChanged(nameof(GameTime));
            RefreshTable();
        }


        #endregion

        #region Event methods

        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnStopGame()
        {
            StopGameEvent?.Invoke(this, EventArgs.Empty);
        }

        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
        }

        private void OnChangePath(String path)
        {
            _path = path;
        }

        #endregion

    }
}
