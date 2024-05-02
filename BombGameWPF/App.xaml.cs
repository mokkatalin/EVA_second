using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BombGame.Model;
using BombGame.Persistence;
using BombGameWPF.ViewModel;
using BombGameWPF;
using System.Windows.Threading;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Xml.Linq;

namespace BombGameWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private BombModel _model = null!;
        private BombViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private DispatcherTimer _timer = null!;

        #endregion

        #region Constructors

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            //modell letrehozasa
            _model = new BombModel(new TextFilePersistence());
            _model.GameOver += new EventHandler<GameOverEventArgs>(Mode_Gameover);
            //_model.LoadGame(); (!nem itt)

            //nezetmodell letrehozasa
            _viewModel = new BombViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
            _viewModel.StopGameEvent += new EventHandler(ViewModel_StopGame);

            //nezet letrehozasa
            _view = new MainWindow();
            _view.DataContext = _viewModel;
            //_view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing);

            _view.ControlP.Visibility = Visibility.Hidden;
            _view.EnemyTime.Visibility = Visibility.Hidden;
            _view.ResumeStack.Visibility = Visibility.Hidden;
            _view.GameBorder.Visibility = Visibility.Hidden;
            _view.Show();

            _view.ControlP.KeyDown += new KeyEventHandler(MovePlayer);

            //idozito letrehozasa
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);
            //_timer.Start(); (!nem itt)
        }

        private void MovePlayer(object? sender, KeyEventArgs e)
        {
            int xo = _model.getPlayerX();
            int yo = _model.getPlayerY();
            switch (e.Key)
            {
                case Key.Left:
                    _model.MovePlayer(Direction.Left);
                    break;
                case Key.Right:
                    _model.MovePlayer(Direction.Right);
                    break;
                case Key.Up:
                    _model.MovePlayer(Direction.Up);
                    break;
                case Key.Down:
                    _model.MovePlayer(Direction.Down);
                    break;
                case Key.B:
                    _model.BombDown(_model.getPlayerX(), _model.getPlayerY());
                    _viewModel.Fields[_model.getPlayerX() * _model.TableSize + _model.getPlayerY()].Text = "\U0001F4A3";
                    break;
            }
            if (xo != _model.getPlayerX() || yo != _model.getPlayerY())
            {
                if (_model[xo, yo].GetFieldType() != FieldType.Bomb) _viewModel.Fields[xo*_model.TableSize + yo].Text = "";
                _viewModel.Fields[_model.getPlayerX() * _model.TableSize + _model.getPlayerY()].Text = "💃";
            }
            
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _model.ReLoad();
        }

        #endregion

        #region View event handlers

        /*
        private void View_Closing(object? sender, CancelEventArgs e)
        {
            throw new NotImplementedException();
        }*/

        #endregion

        #region ViewModel event handlers
        private void ViewModel_StopGame(object? sender, EventArgs e)
        {
            if(_viewModel.StopResume == "Stop")
            {
                _viewModel.StopResume = "Resume";
                _timer.Stop();
                _view.ControlP.KeyDown -= new KeyEventHandler(MovePlayer);

            }
            else if(_viewModel.StopResume == "Resume")
            {
                _viewModel.StopResume = "Stop";
                _timer.Start();
                _view.ControlP.Focus();
                _view.ControlP.KeyDown += new KeyEventHandler(MovePlayer);

            }
        }

        private void ViewModel_ExitGame(object? sender, EventArgs e)
        {
            _view.Close();
        }

        private void ViewModel_NewGame(object? sender, EventArgs e)
        {
            if(_viewModel.Path != null)
            {
                _view.ControlP.Visibility = Visibility.Visible;
                _view.EnemyTime.Visibility = Visibility.Visible;
                _view.ResumeStack.Visibility = Visibility.Visible;
                _view.GameBorder.Visibility = Visibility.Visible;

                _timer.Start();

                _view.ControlP.Focus();

                _model.LoadGame(_viewModel.Path);

                _viewModel.Size = _model.TableSize;
                _viewModel.StopResume = "Stop";
                _view.ControlP.KeyDown -= new KeyEventHandler(MovePlayer);
                _view.ControlP.KeyDown += new KeyEventHandler(MovePlayer);
                //_model.setTime(0);
                //_model.SetEnemiesDown(0);

                // _viewModel.NewGameStart(_model.TableSize);
            }
        }

        #endregion

        #region Model event handlers

        private void Mode_Gameover(object? sender, GameOverEventArgs e)
        {
            _timer.Stop();
            switch (e.Ftype)
            {
                case FieldType.Bomb:
                    MessageBox.Show("Jaj ne, felrobbantál!", "Játék vége",
                        MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    break;
                case FieldType.Enemy:
                    MessageBox.Show("Jaj ne, találkoztál egy ellenféllel!", "Játék vége",
                        MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    break;
                case FieldType.Empty:
                    MessageBox.Show("Hurrá, felrobbantottad az összes ellenfelet!", "Játék vége",
                        MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    break;
            }
           
            _view.ControlP.Visibility = Visibility.Hidden;
            _view.EnemyTime.Visibility = Visibility.Hidden;
            _view.ResumeStack.Visibility = Visibility.Hidden;
            _view.GameBorder.Visibility = Visibility.Hidden;
        }

        #endregion

       

       


    }
}
