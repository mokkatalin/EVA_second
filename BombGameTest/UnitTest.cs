using Moq;
using BombGame.Persistence;
using BombGame.Model;
using System.Diagnostics;

namespace BombGameTest
{
    [TestClass]
    public class UnitTest
    {

        private BombModel _model = null!;
        private FieldType[] _mockedFields = null!;
        private Mock<IPersistence> _mock = null!;

        private Field[,] rightfields = new Field[6, 6];

        [TestInitialize]
        public void InitBombGameTest()
        {
            FieldType[] mF = { FieldType.Empty, FieldType.Empty, FieldType.Empty, FieldType.Empty, FieldType.Empty, FieldType.Empty,
                    FieldType.Empty, FieldType.Wall, FieldType.Empty, FieldType.Wall, FieldType.Empty,FieldType.Empty,
                    FieldType.Empty, FieldType.Wall, FieldType.Empty, FieldType.Enemy, FieldType.Wall,FieldType.Empty,
                    FieldType.Empty, FieldType.Empty, FieldType.Empty, FieldType.Empty, FieldType.Empty, FieldType.Empty,
                    FieldType.Enemy, FieldType.Wall, FieldType.Wall, FieldType.Wall, FieldType.Empty, FieldType.Wall,
                    FieldType.Empty,FieldType.Empty,FieldType.Empty,FieldType.Empty,FieldType.Empty,FieldType.Wall};

            _mockedFields = new FieldType[36];
            _mockedFields = mF;

            _mock = new Mock<IPersistence>();
            _mock.Setup(mock => mock.Load("anypath")).Returns(_mockedFields);

            _model = new BombModel(_mock.Object);

            _model.GameOver += new EventHandler<GameOverEventArgs>(Model_GameOver);

            rightfields[0, 0] = new Field(FieldType.Empty);
            rightfields[0, 1] = new Field(FieldType.Empty);
            rightfields[0, 2] = new Field(FieldType.Empty);
            rightfields[0, 3] = new Field(FieldType.Empty);
            rightfields[0, 4] = new Field(FieldType.Empty);
            rightfields[0, 5] = new Field(FieldType.Empty);
            rightfields[1, 0] = new Field(FieldType.Empty);
            rightfields[1, 1] = new Field(FieldType.Wall);
            rightfields[1, 2] = new Field(FieldType.Empty);
            rightfields[1, 3] = new Field(FieldType.Wall);
            rightfields[1, 4] = new Field(FieldType.Empty);
            rightfields[1, 5] = new Field(FieldType.Empty);
            rightfields[2, 0] = new Field(FieldType.Empty);
            rightfields[2, 1] = new Field(FieldType.Wall);
            rightfields[2, 2] = new Field(FieldType.Empty);
            rightfields[2, 3] = new Field(FieldType.Enemy);
            rightfields[2, 4] = new Field(FieldType.Wall);
            rightfields[2, 5] = new Field(FieldType.Empty);
            rightfields[3, 0] = new Field(FieldType.Empty);
            rightfields[3, 1] = new Field(FieldType.Empty);
            rightfields[3, 2] = new Field(FieldType.Empty);
            rightfields[3, 3] = new Field(FieldType.Empty);
            rightfields[3, 4] = new Field(FieldType.Empty);
            rightfields[3, 5] = new Field(FieldType.Empty);
            rightfields[4, 0] = new Field(FieldType.Enemy);
            rightfields[4, 1] = new Field(FieldType.Wall);
            rightfields[4, 2] = new Field(FieldType.Wall);
            rightfields[4, 3] = new Field(FieldType.Wall);
            rightfields[4, 4] = new Field(FieldType.Empty);
            rightfields[4, 5] = new Field(FieldType.Wall);
            rightfields[5, 0] = new Field(FieldType.Empty);
            rightfields[5, 1] = new Field(FieldType.Empty);
            rightfields[5, 2] = new Field(FieldType.Empty);
            rightfields[5, 3] = new Field(FieldType.Empty);
            rightfields[5, 4] = new Field(FieldType.Empty);
            rightfields[5, 5] = new Field(FieldType.Wall);
        }

        

        [TestMethod]
        public void BombGameModelLoadGameTest()
        {
            _model.LoadGame("anypath");

            //alap ertekek ellenorzese
            Assert.AreEqual(2, _model.EnemyCount);
            Assert.AreEqual(0, _model.GetEnemiesDown());
            Assert.AreEqual(6, _model.TableSize);
            Assert.AreEqual(0, _model.getPlayerX());
            Assert.AreEqual(0, _model.getPlayerY());

            //mezok ellenorzese
            for(int i = 0; i < _model.TableSize; i++)
            {
                for(int j = 0; j < _model.TableSize; j++)
                {
                    Assert.AreEqual(rightfields[i, j].GetFieldType(), _model[i, j].GetFieldType());
                }
            }

        }

        [TestMethod]
        public void BombGameModelMovePlayerTest()
        {
            //jol mozgott a jatekos jo iranyba is
            //meg rossz iranyba is (nem ment at a falon)
            _model.LoadGame("anypath");
            _model.MovePlayer(Direction.Right);
            _model.MovePlayer(Direction.Down);
            Assert.AreEqual(_model.getPlayerX(), 0);
            Assert.AreEqual(_model.getPlayerY(), 1);

            //ellenfelbe utkozik
            _model.MovePlayer(Direction.Right);
            _model.MovePlayer(Direction.Down);
            _model.MovePlayer(Direction.Down);
            _model.MovePlayer(Direction.Right);
        }

        [TestMethod]
        public void BombGameModelReLoadTest()
        {
            // 1. jatek //
            _model.LoadGame("anypath");
            for (int i = 0; i < 3; i++) _model.ReLoad();

            //a reload vegen az enemy-k moved property-je
            //ujbol false
            int enemies = 0;
            int walls = 0;
            for (int i = 0; i <_model.TableSize; i++)
            {
                for(int j = 0; j < _model.TableSize; j++)
                {
                    if (_model[i, j].GetFieldType() == FieldType.Enemy)
                    {
                        Assert.IsFalse(_model[i, j].Moved);
                        enemies++;
                    }
                    else if (_model[i, j].GetFieldType() == FieldType.Wall) walls++;
                }
            }

            //jol valtozik az ido, ellenfelek nem
            //tunnek el maguktol, sem a falak
            Assert.AreEqual(enemies, _model.EnemyCount);
            Assert.AreEqual(walls, 9);
            Assert.AreEqual(3, _model.getTime());
            Assert.IsTrue(_model.GetEnemiesDown() <= enemies);


            // 2. jatek //
            _model.LoadGame("anypath");
            //csak addig megy jol mig nincs game over
            try
            {
                for (int i = 0; i < 8; i++) _model.ReLoad();
                Assert.IsFalse(false);
            }
            catch(Exception e)
            {
                Assert.IsTrue(true);
            }

            // 3. jatek //
            _model.LoadGame("anypath");

            Random r = new Random(DateTime.Now.Millisecond);
            int d;

            //random lefutas generalasa
            while (_model.GameOverEventArgs.Ftype == FieldType.Wall)
            {
                _model.ReLoad();
                d = r.Next(0, 3);
                switch (d)
                {
                    case 0:
                        _model.MovePlayer(Direction.Up);
                        break;
                    case 1:
                        _model.MovePlayer(Direction.Down);
                        break;
                    case 2:
                        _model.MovePlayer(Direction.Right);
                        break;
                    case 3:
                        _model.MovePlayer(Direction.Left);
                        break;
                    case 4:
                        _model.BombDown(_model.getPlayerX(), _model.getPlayerY());
                        break;
                }
                
            }
            
            

        }

        [TestMethod]
        public void BombDownGameTest()
        {
            // 1. jatek //
            //felrobbant a jatekos, vagy talalkozik egy ellenfellel
            _model.LoadGame("anypath");
            _model.BombDown(0, 2);

            Assert.AreEqual(FieldType.Bomb, _model[0, 2].GetFieldType());

            _model.ReLoad();

            Assert.AreEqual(2, _model[0, 2].GetTimeLeft());

            _model.ReLoad();
            _model.ReLoad();

            Assert.AreEqual(0, _model[0, 2].GetTimeLeft());
          
            // 2. jatek //
            //felrobbant egy ellenfel, eltunt a bomba
            _model.LoadGame("anypath");
            _model.BombDown(0, 5);
            _model.ReLoad();
            _model.ReLoad();
            _model.ReLoad();
            _model.ReLoad();

            Assert.AreEqual(FieldType.Empty, _model[0, 5].GetFieldType());

        }

        private void Model_GameOver(Object? sender, GameOverEventArgs e)
        {
            switch (e.Ftype)
            {
                case FieldType.Enemy:
                    Assert.AreEqual(FieldType.Enemy,_model[_model.getPlayerX(), _model.getPlayerY()].GetFieldType());

                    /*
                    Debug.WriteLine(_model.getPlayerX() + " "+ _model.getPlayerY());
                    
                    for (int i = 0; i < _model.TableSize; i++)
                    {
                        for (int j = 0; j < _model.TableSize; j++)
                        {
                            Debug.WriteLine(_model[i,j].GetFieldType() +" ");
                        }
                        Debug.WriteLine("\n");
                    }
                   */
                    break;
                case FieldType.Bomb:
                    bool isBomb = false;
                    for(int i = _model.getPlayerX()-3; i <= _model.getPlayerX()+3; i++)
                    {
                        for (int j = _model.getPlayerY() - 3; j <= _model.getPlayerY() + 3; j++)
                        {
                            if(i >=0 && i <_model.TableSize && j >= 0 && j < _model.TableSize)
                            {
                                if (_model[i,j].GetFieldType() == FieldType.Bomb)
                                {
                                    isBomb = true;
                                }
                            }
                        }
                    }
                    Assert.IsTrue(isBomb);
                    break;
                case FieldType.Empty:
                    Assert.AreEqual(_model.EnemyCount, _model.GetEnemiesDown());
                    break;
            }
        }
    }
}