using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Game : Form
    {
        const int count = 8;
        const int clientX = 1200;
        const int clientY = 800;

        Size spriteSize;
        Point[] chessPoint = new Point[64];

        Dictionary<int, PictureBox> blockList = new Dictionary<int, PictureBox>();
        Bitmap[] whiteChessSource = new Bitmap[6];
        Bitmap[] blackChessSource = new Bitmap[6];
        Label[] possibleLabel = new Label[64];

        Bitmap sprite;
        PictureBox backupGroundChess = new PictureBox();
        PictureBox targetChess = new PictureBox();
        Socket client;

        string possibleColor;
        string address;
        int[] backupPossibleCount = new int[20];
        int bsWidth;
        int bsHeight;
        int userCounter = -1;
        bool isMyOrder;
        bool isMulti;

        public Game(string address, bool isMulti)
        {
            InitializeComponent();
            this.address = address;
            this.isMulti = isMulti;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 블럭 한칸당 사이즈
            bsWidth = clientX / count;
            bsHeight = clientY / count;
            SetClientSizeCore(clientX, clientY);

            // 스프라이트를 자름
            sprite = new Bitmap(Properties.Resources.ChessSprite);
            sprite = new Bitmap(sprite, new Size(clientX, clientY / 4));
            Point spriteSizePoint = new Point(sprite.Width / 8, sprite.Height / 2);
            spriteSize = new Size(spriteSizePoint);

            Point start = new Point();
            for (int i = 0; i < 12; i++)
            {
                // 체스말들을 배열에 넣음
                if (i < 6)
                {
                    start.X = spriteSizePoint.X * i;
                    start.Y = spriteSizePoint.Y;
                    whiteChessSource[i] = AddImage(start, false);
                }
                else
                {
                    int newX = i - 6;
                    start.X = spriteSizePoint.X * newX;
                    start.Y = 0;
                    blackChessSource[newX] = AddImage(start, true);
                }
            }

            // 말판에 좌표를 가져온다
            SetPoint();

            if (isMulti)
            {
                Thread t1 = new Thread(ServerJoin);
                t1.IsBackground = true;
                t1.Start();
            }
        }

        private void ServerJoin()
        {
            Judgement judge = new Judgement(blockList);

            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(new IPEndPoint(IPAddress.Parse(address), 8000));
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
                Application.Exit();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
                Application.Exit();
            }

            byte[] buffer = new byte[1024];
            int length = 0;
            string msg = string.Empty;
            while (true)
            {
                try
                {
                    length = client.Receive(buffer);
                    msg = Encoding.UTF8.GetString(buffer, 0, length);
                    if (msg.Contains("PlayerNumber"))
                    {
                        string[] cutMsg = msg.Split('_');
                        if (userCounter < 0)
                        {
                            userCounter = Int32.Parse(cutMsg[1]);
                        }
                        if (userCounter == 0)
                        {
                            isMyOrder = true;
                            possibleColor = "Black";
                        }
                        else
                        {
                            isMyOrder = false;
                            possibleColor = "White";
                        }
                        MessageBox.Show("누군가가 들어왔습니다.");
                    }

                    if (msg.Contains("SendGround"))
                    {
                        string[] cutMsg = msg.Split('_');
                        backupGroundChess = judge.GetDictionaryValue(Int32.Parse(cutMsg[1]));
                    }
                    if (msg.Contains("SendTarget"))
                    {
                        string[] cutMsg = msg.Split('_');
                        targetChess = judge.GetDictionaryValue(Int32.Parse(cutMsg[1]));
                        MoveChess(backupGroundChess, false);
                        backupGroundChess = new PictureBox();
                    }
                }
                catch (SocketException se)
                {
                    MessageBox.Show(se.Message);
                    Application.Exit();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    Application.Exit();
                }
            }
        }

        private void SetTargetChess(PictureBox chessImg)
        {
            if (isMyOrder)
            {
                // 같은 말 고를시 취소
                if (chessImg == targetChess)
                {
                    int key = blockList.FirstOrDefault(x => x.Value == targetChess).Key;
                    LabelVisibleActive(targetChess, key, false);
                    targetChess = new PictureBox();
                }
                // 말 이동
                else if (!chessImg.Name.All(char.IsDigit) && targetChess.Image != null)
                {
                    MoveChess(chessImg, isMulti);
                }
                else if (chessImg.Name.All(char.IsDigit) && targetChess.Image != null)
                {
                    MoveChess(chessImg, isMulti);
                }
                // 말 선택
                else if (chessImg.Image != null)
                {
                    if (chessImg.Name.Contains(possibleColor))
                    {
                        targetChess = chessImg;
                        int key = blockList.FirstOrDefault(x => x.Value == targetChess).Key;
                        LabelVisibleActive(targetChess, key, true);
                    }
                }
            }
        }

        private void MoveChess(PictureBox ground, bool isSend)
        {
            if (targetChess != null)
            {
                int key = blockList.FirstOrDefault(x => x.Value == targetChess).Key;
                int groundKey = blockList.FirstOrDefault(x => x.Value == ground).Key;
                int[] pos = Judge(targetChess, key);
                Judgement judge = new Judgement(blockList);
                bool isDiffrent = false;

                if (pos.Contains(judge.GetDictionaryKey(ground)))
                {
                    if (!ground.Name.All(char.IsDigit))
                    {
                        string confirmTargetColor = targetChess.Name.Substring(0, 4);
                        string confirmGroundColor = ground.Name.Substring(0, 4);
                        if (!confirmTargetColor.Equals(confirmGroundColor))
                        {
                            if (ground.Name.Contains("King"))
                            {
                                MessageBox.Show(possibleColor + "이 승리하였습니다!");
                            }
                            TurnChange(isSend, judge, ground);
                            FillImage(groundKey, targetChess.Image, targetChess.Name);
                            targetChess.Image = null;
                            targetChess.Name = key.ToString();
                        }
                        else
                        {
                            isDiffrent = true;
                        }
                    }
                    else
                    {
                        TurnChange(isSend, judge, ground);
                        FillImage(groundKey, targetChess.Image, targetChess.Name);
                        targetChess.Image = null;
                        targetChess.Name = key.ToString();
                    }

                    if (!isDiffrent)
                    {
                        LabelVisibleActive(targetChess, key, false);
                        targetChess = new PictureBox();
                    }
                }
            }
        }

        private void TurnChange(bool isSend, Judgement judge, PictureBox ground)
        {
            if (isSend) // 보낸다면
            {
                SendData(judge, ground);
                isMyOrder = false;
            }
            else // 받는다면
            {
                isMyOrder = true;
            }
        }

        private void SendData(Judgement judge, PictureBox ground)
        {
            string ground_Data = judge.GetDictionaryKey(ground).ToString();
            byte[] buffer = Encoding.UTF8.GetBytes("SendGround_" + ground_Data);
            string Target_Data = judge.GetDictionaryKey(targetChess).ToString();
            byte[] buffer2 = Encoding.UTF8.GetBytes("SendTarget_" + Target_Data);
            client.Send(buffer);
            client.Send(buffer2);
        }

        private void FillImage(int count, Image chessSource, string chessName)
        {
            PictureBox box;
            if (blockList.TryGetValue(count, out box))
            {
                if (box.InvokeRequired)
                {
                    box.Invoke(new MethodInvoker(delegate ()
                    {
                        box.Name = count.ToString();
                        box.Image = chessSource;
                        box.Name = chessName;
                    }));
                }
                else
                {
                    box.Name = count.ToString();
                    box.Image = chessSource;
                    box.Name = chessName;
                }
            }
        }

        public int[] Judge(PictureBox targetChess, int counter)
        {
            int[] allowNum = new int[20];
            Judgement judge = new Judgement(blockList);

            if (targetChess.Name == "BlackPawn")
            {
                allowNum = judge.BlackPawn(counter);
            }
            else if (targetChess.Name == "BlackRook")
            {
                allowNum = judge.BlackRook(counter);
            }
            else if (targetChess.Name == "BlackKnight")
            {
                allowNum = judge.BlackKnight(counter);
            }
            else if (targetChess.Name == "BlackBishop")
            {
                allowNum = judge.BlackBishop(counter);
            }
            else if (targetChess.Name == "BlackQueen")
            {
                allowNum = judge.BlackQueen(counter);
            }
            else if (targetChess.Name == "BlackKing")
            {
                allowNum = judge.BlackKing(counter);
            }
            else if (targetChess.Name == "WhitePawn")
            {
                allowNum = judge.WhitePawn(counter);
            }
            else if (targetChess.Name == "WhiteRook")
            {
                allowNum = judge.WhiteRook(counter);
            }
            else if (targetChess.Name == "WhiteKnight")
            {
                allowNum = judge.WhiteKinight(counter);
            }
            else if (targetChess.Name == "WhiteBishop")
            {
                allowNum = judge.WhiteBishop(counter);
            }
            else if (targetChess.Name == "WhiteQueen")
            {
                allowNum = judge.WhiteQueen(counter);
            }
            else if (targetChess.Name == "WhiteKing")
            {
                allowNum = judge.WhiteKing(counter);
            }

            return allowNum;
        }

        private void LabelVisibleActive(PictureBox targetChess, int counter, bool isVisible)
        {
            int[] allowNum = Judge(targetChess, counter);
            int[] newAllowArray;

            newAllowArray = allowNum;
            if (!isVisible)
            {
                newAllowArray = backupPossibleCount;
            }
            for (int i = 0; i < backupPossibleCount.Length; i++)
            {
                if (newAllowArray[i] == 0)
                {
                    break;
                }
                possibleLabel[newAllowArray[i]].Visible = isVisible;
                backupPossibleCount[i] = newAllowArray[i];
            }
        }

        private void SetPoint()
        {
            int y = 0;
            int minusX = 0;

            for (int i = 0; i < 64; i++)
            {
                int x = i - minusX;

                if (i % 8 == 0 && i != 0) // y축내리기
                {
                    y += 1;
                    minusX = i;
                    x = i - minusX;
                }

                chessPoint[i] = new Point((bsWidth * x), (bsHeight * y));

                InstallLabel(i, chessPoint[i]);

                if (i < 16 && i >= 8)
                {
                    InstallChess(i, chessPoint[i], whiteChessSource[5], "BlackPawn");
                }
                else if (i < 56 && i >= 48)
                {
                    InstallChess(i, chessPoint[i], blackChessSource[5], "WhitePawn");
                }
                else
                {
                    InstallChess(i, chessPoint[i], Properties.Resources.EmptyImage, i.ToString());
                }

            }

            //블랙 체스
            FillImage(0, whiteChessSource[4], "BlackRook"); // 성
            FillImage(1, whiteChessSource[3], "BlackKnight"); // 말
            FillImage(2, whiteChessSource[2], "BlackBishop"); // 비숍
            FillImage(3, whiteChessSource[1], "BlackQueen");
            FillImage(4, whiteChessSource[0], "BlackKing");
            FillImage(5, whiteChessSource[2], "BlackBishop");
            FillImage(6, whiteChessSource[3], "BlackKnight");
            FillImage(7, whiteChessSource[4], "BlackRook");

            //화이트 체스
            FillImage(64 - 1, blackChessSource[4], "WhiteRook");
            FillImage(64 - 2, blackChessSource[3], "WhiteKnight");
            FillImage(64 - 3, blackChessSource[2], "WhiteBishop");
            FillImage(64 - 4, blackChessSource[0], "WhiteQueen");
            FillImage(64 - 5, blackChessSource[1], "WhiteKing");
            FillImage(64 - 6, blackChessSource[2], "WhiteBishop");
            FillImage(64 - 7, blackChessSource[3], "WhiteKnight");
            FillImage(64 - 8, blackChessSource[4], "WhiteRook");
        }

        private void InstallChess(int count, Point chessPoint, Image chessSource, string chessName)
        {
            PictureBox add = new PictureBox
            {
                Size = new Size(spriteSize.Width, spriteSize.Height),
                Location = chessPoint,
                Image = chessSource,
                Name = chessName
            };

            add.Click += (sender, e) => { SetTargetChess(add); };
            Controls.Add(add);

            blockList.Add(count, add);
        }

        private void InstallLabel(int count, Point chessPoint)
        {
            Label label = new Label
            {
                Size = new Size(130, 80),
                Location = chessPoint,
                Name = count.ToString(),
                Text = count.ToString(),
                AutoSize = true,
                Font = new System.Drawing.Font("바탕", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point),
                Visible = false
            };

            possibleLabel[count] = label;
            Controls.Add(label);
        }

        private Bitmap AddImage(Point cutLoc, bool isBlack)
        {
            Bitmap source = new Bitmap(sprite);
            Bitmap target = new Bitmap(spriteSize.Width, spriteSize.Height);
            Graphics g = Graphics.FromImage(target);
            if (isBlack)
            {
                g.DrawImage(source, 0, 0, new Rectangle(cutLoc.X + 18, cutLoc.Y + 8, spriteSize.Width, spriteSize.Height),
                GraphicsUnit.Point);
            }
            else
            {
                g.DrawImage(source, 0, 0, new Rectangle(cutLoc.X + 18, cutLoc.Y - 30, spriteSize.Width, spriteSize.Height),
                GraphicsUnit.Point);
            }
            return target;
        }   

        //private void ScreenVertical(Graphics g)
        //{
        //    Point start = new Point();
        //    Point end = new Point();

        //    for (int i = 0; i < count; i++)
        //    {
        //        start.X = 0;
        //        start.Y = bsHeight * i;
        //        end.X = clientX;
        //        end.Y = start.Y;
        //        g.DrawLine(Pens.Black, start, end);
        //    }

        //    Console.WriteLine(start);
        //}

        //private void ScreenHorizontal(Graphics g)
        //{
        //    Point start = new Point();
        //    Point end = new Point();

        //    for (int i = 0; i < count; i++)
        //    {
        //        start.X = bsWidth * i;
        //        start.Y = 0;
        //        end.X = start.X;
        //        end.Y = clientY;
        //        g.DrawLine(Pens.Black, start, end);
        //    }
        //}
    }
}
