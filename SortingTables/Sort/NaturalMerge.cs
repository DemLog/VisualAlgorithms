using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SortingTables.Sort
{
    public class NaturalMerge
    {
        private const string PATH_LEFT = @"../../../left.txt";
        private const string PATH_RIGHT = @"../../../right.txt";
        private long _segments;
        public List<int> LeftInd = new List<int>();
        public List<int> RightInd = new List<int>();
        public DrawMove DrawMove;
        public StackPanel Logs;
        public NaturalMerge(StackPanel content, StackPanel logs)
        {
            DrawMove = new DrawMove(content);
            Logs = logs;
        }

        public async Task MergeSort(Table[] array, int jump = 1)
        {
            while (_segments != 1)
            {
                SplitToFiles(array);
                await DrawMove.Alg(array, LeftInd, RightInd);
                MergePairs(array);
                await DrawMove.Alg(array, LeftInd, RightInd);
                await DrawMove.Alg2(array, LeftInd, RightInd);
            }
        }
        
        private void SplitToFiles(Table[] array)
        {
            _segments = 1;
            using StreamWriter writerA = new StreamWriter(PATH_LEFT);
            using StreamWriter writerB = new StreamWriter(PATH_RIGHT);
            {
                bool flag = true;
                string str1 = null, str2;
                //Type currentType = types[_columnNumber];
                for (int i = 0; i < array.Length;)
                {
                    if (str1 is null)
                    {
                        str1 = array[i].ToString();
                        writerA.WriteLine(str1);
                        LeftInd.Add(i);
                    }

                    if (i + 1 == array.Length) break;
                    str2 = array[i + 1].ToString();
                    
                    if (str2 is null | String.Compare(str2, "", StringComparison.Ordinal) == 0) break;

                    if (array[i] >= array[i + 1])
                    {
                        flag = !flag;
                        _segments++;
                    }

                    if (flag)
                    {
                        writerA.WriteLine(str2);
                        LeftInd.Add(i + 1);
                    }
                    else
                    {
                        writerB.WriteLine(str2);
                        RightInd.Add(i + 1);
                    }
                    str1 = str2;
                    i++;
                }
            }
        }
        
        private void MergePairs(Table[] array)
        {
            using StreamReader readerA = new StreamReader(PATH_LEFT);
            using StreamReader readerB = new StreamReader(PATH_RIGHT);
            {
                Table elementA = null, elementB = null;
                string strA = null, strB = null;
                bool pickedA = false, pickedB = false, endA = false, endB = false;
                int idx = 0;
                while (!endA || !endB)
                {
                    if (!endA & !pickedA)
                    {
                        strA = readerA.ReadLine();
                        if (strA is null | String.Compare(strA, "", StringComparison.Ordinal) == 0) endA = true;
                        else
                        {
                            elementA = Table.GetTable(strA.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                            pickedA = true;
                        }
                    }

                    if (!endB & !pickedB)
                    {
                        strB = readerB.ReadLine();
                        if (strB is null | String.Compare(strB, "", StringComparison.Ordinal) == 0) endB = true;
                        else
                        {
                            elementB = Table.GetTable(strB.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                            pickedB = true;
                        }
                    }

                    if (pickedA)
                    {
                        if (pickedB)
                        {
                            Logs.Children.Add(GetLog($"{elementA} < {elementB}"));
                            if (elementA < elementB)
                            {
                                array[idx] = elementA;
                                Logs.Children.Add(GetLog($"Запись {elementA}"));
                                pickedA = false;
                            }
                            else
                            {
                                array[idx] = elementB;
                                Logs.Children.Add(GetLog($"Запись {elementB}"));
                                pickedB = false;
                            }
                        }
                        else
                        {
                            array[idx] = elementA;
                            Logs.Children.Add(GetLog($"Запись {elementA}"));
                            pickedA = false;
                        }
                    }
                    else if (pickedB)
                    {
                        array[idx] = elementB;
                        Logs.Children.Add(GetLog($"Запись {elementB}"));
                        pickedB = false;
                    }

                    idx++;
                }
            }
            // File.WriteAllText(PATH_LEFT, "");
            // File.WriteAllText(PATH_RIGHT, "");
        }
        private TextBlock GetLog(string text)
            => new TextBlock
            {
                Text = text,
            };
    }
}