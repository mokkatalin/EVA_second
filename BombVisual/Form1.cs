using BombGame.Model;
using BombGame.Persistence;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows.Forms.VisualStyles;

namespace BombVisual
{
    public partial class Form1 : Form
    {
        #region Private fields

        private BombModel _model;

        private IPersistence _persistence;

        string? path = null;

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        #endregion

        #region Constructors
        public Form1()
        {
            InitializeComponent();
            _persistence = new TextFilePersistence();
            _model = new BombModel(_persistence);

            _model.TableUpdate += new EventHandler(Model_TableUpdate);
            _model.GameOver += new EventHandler<GameOverEventArgs>
                (Model_GameOver);
            _model.GenerateTable += new EventHandler<GenerateTableEventArgs>
                (Model_GenerateTable);

            timer.Interval = 1000;

            this.panel2.Location = new Point(0, 0);
            SetBounds(Top, Left, this.panel2.Width + 40, this.panel2.Height+40);

        }

        #endregion

        #region Model event handlers

        private void Model_GameOver(object? sender, GameOverEventArgs e)
        {
            timer.Stop();
            timer.Tick -= StepUpdate;
            switch (e.Ftype)
            {
                case FieldType.Bomb:
                    MessageBox.Show("Jaj ne, felrobbantál!", "Játék vége!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    break;
                case FieldType.Enemy:
                    this.labels[_model.getPlayerX(), _model.getPlayerY()].ForeColor = Color.Brown;
                    MessageBox.Show("Jaj ne, találkoztál egy ellenféllel!", "Játék vége!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    break;
                case FieldType.Empty:
                    MessageBox.Show("Hurrá, felrobbantottad az összes ellenfelet!", "Játék vége!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    break;
                default:
                    break;
            }
            
            this.panel1.Controls.Remove(statusStrip1);
            this.Controls.Remove(this.panel1);
            this.panel3.Controls.Remove(StopResumeBttn);
            SetBounds(Top, Left, this.panel2.Width, this.panel2.Height + 40);

        }   

        private void Model_GenerateTable(object? sender, GenerateTableEventArgs e)
        {
            this.labels = new Label[e.size,e.size];
            for (int j = 0; j < e.size; j++)
            {
                for (int i = 0; i < e.size; i++)
                {
                    this.labels[i,j] = new Label();
                    this.labels[i, j].Name = "label" + i + "_" + j;
                    this.labels[i,j].Size = new Size(50,50);
                    this.labels[i, j].Font = new Font("Segoe UI Symbol", 22);
                    this.labels[i, j].Margin = new Padding(0);
                    this.labels[i,j].Location = new Point(i*50,j*50);
                    this.labels[i,j].BorderStyle = BorderStyle.FixedSingle;
                    panel1.Controls.Add(this.labels[i, j]);
                }

            }
        }

        private void Model_TableUpdate(object? sender, EventArgs e)
        {
            for(int i = 0; i < _model.TableSize; i++)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                    ChangeField(i, j, _model[j, i].GetFieldType());
                    
                }
            }

            for (int i = 0; i < _model.TableSize; i++)
            {
                for(int j = 0; j < _model.TableSize; j++)
                {
                    if (_model[j, i].GetFieldType() == FieldType.Bomb || _model[j, i].GetSecondary() == FieldType.Bomb)
                    {
                        for (int a = i - 3; a <= i + 3; a++)
                        {
                            for (int b = j - 3; b <= j + 3; b++)
                            {
                                if ((b >= 0) && (b < _model.TableSize) && (a >= 0) && (a < _model.TableSize))
                                {
                                    if (this.labels[a, b].BackColor != Color.Black)
                                    {
                                        this.labels[a, b].BackColor = Color.Pink;
                                    }
                                }
                            }
                        }
                    }
                }    
            }

            if (_model[_model.getPlayerX(), _model.getPlayerY()].GetFieldType() != FieldType.Bomb) {
                this.labels[_model.getPlayerY(), _model.getPlayerX()].Text = "\U0001F483";
            } 
        }

        private void ChangeField(int x, int y, FieldType ft)
        {
            switch (ft)
            {
                case FieldType.Empty:
                    this.labels[x, y].Text = "";
                    this.labels[x, y].BackColor = Color.White;
                    break;
                case FieldType.Wall:
                    this.labels[x, y].BackColor = Color.Black;

                    break;
                case FieldType.Bomb:
                    this.labels[x,y].Text = "\U0001F4A3";
                    this.labels[x, y].ForeColor = Color.Black;
                    this.labels[x, y].BackColor = Color.White;
                    break;
                case FieldType.Enemy:
                    this.labels[x,y].Text = "\U0001F47D";
                    this.labels[x, y].ForeColor = Color.Black;
                    this.labels[x, y].BackColor = Color.White;
                    break;
                default:
                    break;
            }

        }

        #endregion

        #region Panel23 event handlers

        private void Rbtn_CheckedChanged(object sender, EventArgs e)
        {
            var rb = sender as RadioButton;
            path = rb.Tag.ToString();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            if (path != null)
            {
                this.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
                timer.Tick -= StepUpdate;

                if (this.labels != null) ClearPanel();
                _model.LoadGame(path);
                this.labels[0, 0].Text = "\U0001F483";
                this.panel1.Size = new System.Drawing.Size(_model.TableSize * 55, _model.TableSize * 55);
                this.panel1.Location = new Point(this.panel2.Location.X + this.panel2.Width, 0);
                this.Controls.Add(this.panel1);

                _model.setTime(0);
                _model.SetEnemiesDown(0);
                this.enemyLabel.Text = "Enemies down: " + _model.GetEnemiesDown();
                this.timeLabel.Text = this.timeLabel.Text = "Time passed: " + (_model.getTime());
                this.panel1.Controls.Add(statusStrip1);
                this.StopResumeBttn.Text = "Stop";
                this.panel3.Controls.Add(this.StopResumeBttn);
                this.panel3.Location = new Point(this.panel1.Location.X + this.panel1.Width, 0);
                this.Controls.Add(this.panel3);

                this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
                timer.Tick += StepUpdate;

                if (!timer.Enabled) timer.Enabled = true;
                this.panel1.Focus();

                SetBounds(Top, Left, this.panel2.Width + this.panel1.Width
                    + this.panel3.Width, this.panel1.Height + 40);

            }
        }

        private void StopResumeBttn_Click(object sender, EventArgs e)
        {
            if (StopResumeBttn.Text == "Stop")
            {
                StopResumeBttn.Text = "Resume";
                timer.Stop();
                this.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);

            }
            else
            {
                StopResumeBttn.Text = "Stop";
                timer.Start();
                this.panel1.Focus();
                this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);

            }

        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Form/Panel1 event handlers

        private void StepUpdate(object? sender, EventArgs e)
        {
            _model.ReLoad();
            this.timeLabel.Text = "Time passed: " + (_model.getTime());
            this.enemyLabel.Text = "Enemies down: " + _model.GetEnemiesDown();
        }

        private void ClearPanel()
        {
            for (int i = 0; i < _model.TableSize; i++)
            {
                for (int j = 0; j < _model.TableSize; j++)
                {
                    this.panel1.Controls.Remove(this.labels[i, j]);
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(_model.getTime() != 0)
            {
                int xo = _model.getPlayerX();
                int yo = _model.getPlayerY();
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        _model.MovePlayer(Direction.Up);
                        break;
                    case Keys.Down:
                        _model.MovePlayer(Direction.Down);
                        break;
                    case Keys.Right:
                        _model.MovePlayer(Direction.Right);
                        break;
                    case Keys.Left:
                        _model.MovePlayer(Direction.Left);
                        break;
                    case Keys.B:
                        _model.BombDown(_model.getPlayerX(), _model.getPlayerY());
                        this.labels[_model.getPlayerY(), _model.getPlayerX()].Text = "\U0001F4A3";
                        break;
                    default:
                        break;
                }

                if (xo != _model.getPlayerX() || yo != _model.getPlayerY())
                {
                    if (_model[xo, yo].GetFieldType() != FieldType.Bomb) this.labels[yo, xo].Text = "";
                    this.labels[_model.getPlayerY(), _model.getPlayerX()].Text = "\U0001F483";
                }
            }
            
        }


        #endregion

        
    }

}