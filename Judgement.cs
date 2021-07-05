using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public class Judgement
    {
        Dictionary<int, PictureBox> blockList = new Dictionary<int, PictureBox>();
        int[] allowNum = new int[30];
        int length = 0;

        public Judgement(Dictionary<int, PictureBox> blockList)
        {
            this.blockList = blockList;
        }

        public int GetDictionaryKey(PictureBox targetChess)
        {
            int key = blockList.FirstOrDefault(x => x.Value == targetChess).Key;
            return key;
        }

        public PictureBox GetDictionaryValue(int count)
        {
            PictureBox box;

            if (blockList.TryGetValue(count, out box))
            {
                return box;
            }
            return null;
        }

        public int[] BlackPawn(int counter)
        {
            if (GetDictionaryValue(counter + 8).Name.All(char.IsDigit))
            {
                allowNum[length] = counter + 8;
                length++;
            }
            if (!GetDictionaryValue(counter + 7).Name.All(char.IsDigit))
            {
                allowNum[length] = counter + 7;
                length++;
            }
            if (!GetDictionaryValue(counter + 9).Name.All(char.IsDigit))
            {
                allowNum[length] = counter + 9;
                length++;
            }
            return allowNum;
        }

        public int[] BlackRook(int counter)
        {
            int newCounter = counter - 7;

            if (counter > 7)
            {
                newCounter = (counter % 7) - 7;
            }

            for (int i = counter + 8; i <= 63 + newCounter; i += 8)
            {
                if (GetDictionaryValue(i).Name.All(char.IsDigit))
                {
                    allowNum[length] = i;
                    length++;
                }
                else
                {
                    allowNum[length] = i;
                    length++;
                    break;
                }
            }
            for (int i = counter - 8; i >= -newCounter; i -= 8)
            {
                if (GetDictionaryValue(i).Name.All(char.IsDigit))
                {
                    allowNum[length] = i;
                    length++;
                }
                else
                {
                    allowNum[length] = i;
                    length++;
                    break;
                }
            }

            //옆
            //newCounter = counter % 8;
            //counter -= newCounter;
            //Console.WriteLine(counter);
            //Console.WriteLine(counter + 8);

            //for (int i = counter; i < counter + 8; i++)
            //{
            //    if (GetDictionaryValue(i).Name.All(char.IsDigit))
            //    {
            //        allowNum[length] = i;
            //        length++;
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            return allowNum;
        }

        public int[] BlackKnight(int counter)
        {
            KnightJudge(counter, 6);
            KnightJudge(counter, 10);
            KnightJudge(counter, 15);
            KnightJudge(counter, 17);
            KnightJudge(counter, -6);
            KnightJudge(counter, -10);
            KnightJudge(counter, -15);
            KnightJudge(counter, -17);
            return allowNum;
        }

        public int[] BlackBishop(int counter)
        {
            for (int i = counter + 9; i <= 63; i += 9)
            {
                if (GetDictionaryValue(i).Name.All(char.IsDigit))
                {
                    allowNum[length] = i;
                    length++;
                }
                else
                {
                    allowNum[length] = i;
                    length++;
                    break;
                }

                if (i > 56)
                {
                    break;
                }
                else if (i % 7 == 0)
                {
                    break;
                }
            }
            for (int i = counter - 9; i >= 0; i -= 9)
            {
                if (GetDictionaryValue(i).Name.All(char.IsDigit))
                {
                    allowNum[length] = i;
                    length++;
                }
                else
                {
                    allowNum[length] = i;
                    length++;
                    break;
                }

                if (i < 8)
                {
                    break;
                }
                else if (i % 8 == 0)
                {
                    break;
                }
            }
            for (int i = counter + 7; i <= 63; i += 7)
            {
                if (GetDictionaryValue(i).Name.All(char.IsDigit))
                {
                    allowNum[length] = i;
                    length++;
                }
                else
                {
                    allowNum[length] = i;
                    length++;
                    break;
                }

                if (i > 56)
                {
                    break;
                }
                else if (i % 8 == 0)
                {
                    break;
                }
            }
            for (int i = counter - 7; i >= 0; i -= 7)
            {
                if (GetDictionaryValue(i).Name.All(char.IsDigit))
                {
                    allowNum[length] = i;
                    length++;
                }
                else
                {
                    allowNum[length] = i;
                    length++;
                    break;
                }

                if (i < 8)
                {
                    break;
                }
                else if (i % 7 == 0)
                {
                    break;
                }
            }
            return allowNum;
        }

        public int[] BlackQueen(int counter)
        {
            int[] rookNum = new int[10];
            rookNum = BlackRook(counter);
            int[] bishopNum = new int[10];
            bishopNum = BlackBishop(counter);

            var list = new List<int>();
            list.AddRange(rookNum);
            list.AddRange(bishopNum);

            int[] newAllowNum = list.ToArray();
            return newAllowNum;
        }

        public int[] BlackKing(int counter)
        {
            KingJudge(counter, 1);
            KingJudge(counter, 7);
            KingJudge(counter, 8);
            KingJudge(counter, 9);
            KingJudge(counter, -1);
            KingJudge(counter, -7);
            KingJudge(counter, -8);
            KingJudge(counter, -9);

            return allowNum;
        }

        public int[] WhitePawn(int counter)
        {
            if (GetDictionaryValue(counter - 8).Name.All(char.IsDigit))
            {
                allowNum[length] = counter - 8;
                length++;
            }
            if (!GetDictionaryValue(counter - 7).Name.All(char.IsDigit))
            {
                allowNum[length] = counter - 7;
                length++;
            }
            if (!GetDictionaryValue(counter - 9).Name.All(char.IsDigit))
            {
                allowNum[length] = counter - 9;
                length++;
            }
            return allowNum;
        }

        public int[] WhiteRook(int counter)
        {
            int newCounter = (counter % 8) - 8;

            for (int i = counter - 8; i >= 8 + newCounter; i -= 8)
            {
                if (GetDictionaryValue(i).Name.All(char.IsDigit))
                {
                    allowNum[length] = i;
                    length++;
                }
                else
                {
                    allowNum[length] = i;
                    length++;
                    break;
                }
            }

            int targetNum = 0;
            if(-newCounter < 4)
            {
                targetNum = 63;
            }
            else if(-newCounter >= 4)
            {
                targetNum = 55;
            }

            for (int i = counter + 8; i <= targetNum - newCounter; i += 8)
            {
                if (GetDictionaryValue(i).Name.All(char.IsDigit))
                {
                    allowNum[length] = i;
                    length++;
                }
                else
                {
                    allowNum[length] = i;
                    length++;
                    break;
                }
            }

            return allowNum;
        }

        public int[] WhiteKinight(int counter)
        {
            KnightJudge(counter, 6);
            KnightJudge(counter, 10);
            KnightJudge(counter, 15);
            KnightJudge(counter, 17);
            KnightJudge(counter, -6);
            KnightJudge(counter, -10);
            KnightJudge(counter, -15);
            KnightJudge(counter, -17);
            return allowNum;
        }

        public int[] WhiteBishop(int counter)
        {
            for (int i = counter + 9; i <= 63; i += 9)
            {
                if (GetDictionaryValue(i).Name.All(char.IsDigit))
                {
                    allowNum[length] = i;
                    length++;
                }
                else
                {
                    allowNum[length] = i;
                    length++;
                    break;
                }

                if (i > 56)
                {
                    break;
                }
                else if (i % 7 == 0)
                {
                    break;
                }
            }
            for (int i = counter - 9; i >= 0; i -= 9)
            {
                if (GetDictionaryValue(i).Name.All(char.IsDigit))
                {
                    allowNum[length] = i;
                    length++;
                }
                else
                {
                    allowNum[length] = i;
                    length++;
                    break;
                }

                if (i < 8)
                {
                    break;
                }
                else if (i % 8 == 0)
                {
                    break;
                }
            }
            for (int i = counter + 7; i <= 63; i += 7)
            {
                if (GetDictionaryValue(i).Name.All(char.IsDigit))
                {
                    allowNum[length] = i;
                    length++;
                }
                else
                {
                    allowNum[length] = i;
                    length++;
                    break;
                }

                if (i > 56)
                {
                    break;
                }
                else if (i % 8 == 0)
                {
                    break;
                }
            }
            for (int i = counter - 7; i >= 0; i -= 7)
            {
                if (GetDictionaryValue(i).Name.All(char.IsDigit))
                {
                    allowNum[length] = i;
                    length++;
                }
                else
                {
                    allowNum[length] = i;
                    length++;
                    break;
                }

                if (i < 8)
                {
                    break;
                }
                else if (i % 7 == 0)
                {
                    break;
                }
            }

            return allowNum;
        }

        public int[] WhiteQueen(int counter)
        {
            int[] rookNum = new int[10];
            rookNum = WhiteRook(counter);
            int[] bishopNum = new int[10];
            bishopNum = WhiteBishop(counter);

            var list = new List<int>();
            list.AddRange(rookNum);
            list.AddRange(bishopNum);

            int[] newAllowNum = list.ToArray();
            return newAllowNum;
        }

        public int[] WhiteKing(int counter)
        {
            KingJudge(counter, -1);
            KingJudge(counter, -7);
            KingJudge(counter, -8);
            KingJudge(counter, -9);
            KingJudge(counter, 1);
            KingJudge(counter, 7);
            KingJudge(counter, 8);
            KingJudge(counter, 9);

            return allowNum;
        }

        private void KnightJudge(int counter, int next)
        {
            if ((counter == 6 || counter == 14 || counter == 22 || counter == 30 ||
                counter == 38 || counter == 46 || counter == 54 || counter == 62) && (next == -6 || next == 10))
            {
                return;
            }
            else if ((counter == 1 || counter == 9 || counter == 17 || counter == 25 ||
                counter == 33 || counter == 41 || counter == 49 || counter == 57) && (next == 6 || next == -10))
            {
                return;
            }
            else if ((counter > 55) && (next == 10 || next == 6 || next == 15 || next == 17))
            {
                return;
            }
            else if ((counter < 8) && (next == -10 || next == -6 || next == -15 || next == -17))
            {
                return;
            }
            else
            {
                allowNum[length] = counter + next;
                length++;
            }
        }

        private void KingJudge(int counter, int next)
        {
            if(next == -1 || next == 1)
            {
                allowNum[length] = counter + next;
                length++;
            }
            else if (counter < 8 && next < 0)
            {
                return;
            }
            else if (counter > 55 && next > 0)
            {
                return;
            }
            else
            {
                allowNum[length] = counter + next;
                length++;
            }
        }
    }
}
