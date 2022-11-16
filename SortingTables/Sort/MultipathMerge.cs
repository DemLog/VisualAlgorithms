using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SortingTables.Sort
{
    public enum StreamNumber
    {
        StreamA,
        StreamB,
        StreamC

    }

    public class MultipathMerge
    {
        private string FileInput { get; }
        private long _iterations, _segments;
        public List<int> LeftInd = new List<int>();
        public List<int> RightInd = new List<int>();
        public DrawMove DrawMove;
        public StackPanel Logs;
        
        public MultipathMerge(StackPanel content, StackPanel logs)
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
            using StreamWriter writerA = new StreamWriter(File.Create("a.txt", 65536));
            using StreamWriter writerB = new StreamWriter(File.Create("b.txt", 65536));
            using StreamWriter writerC = new StreamWriter(File.Create("c.txt", 65536));
            long counter = 0;
            StreamNumber streamNumber = StreamNumber.StreamA;
            for (int i = 0; i < array.Length; i++)
            {
                if (counter == _iterations)
                {
                    if (streamNumber == StreamNumber.StreamA) streamNumber = StreamNumber.StreamB;
                    else if (streamNumber == StreamNumber.StreamB) streamNumber = StreamNumber.StreamC;
                    else if (streamNumber == StreamNumber.StreamC) streamNumber = StreamNumber.StreamA;
                    _segments++;
                    counter = 0;
                }

                string element = array[i].ToString();
                if (element is null) break;
                if (streamNumber == StreamNumber.StreamA)
                {
                    writerA.WriteLine(element);
                    LeftInd.Add(i);
                }
                else if (streamNumber == StreamNumber.StreamB)
                {
                    writerB.WriteLine(element);
                    LeftInd.Add(i);
                }
                else if (streamNumber == StreamNumber.StreamC)
                {
                    writerC.WriteLine(element);
                    RightInd.Add(i);
                }

                counter++;
            }
            writerA.Close();
            writerB.Close();
        }
        
        private void MergePairs(Table[] array)
        {
            _iterations = 1;
            using StreamReader readerA = new StreamReader(File.OpenRead("a.txt"));
            using StreamReader readerB = new StreamReader(File.OpenRead("b.txt"));
            using StreamReader readerC = new StreamReader(File.OpenRead("c.txt"));
            long counterA = _iterations, counterB = _iterations, counterC = _iterations;
            Table elementA = null, elementB = null, elementC = null;
            string strA = null, strB = null, strC = null;
            bool pickedA = false, pickedB = false, pickedC = false, endA = false, endB = false, endC = false;
            int idx = 0;
            while (!endA || !endB || !endC)
            {
                if (counterA == 0 && counterB == 0 && counterC == 0)
                {
                    counterA = _iterations;
                    counterB = _iterations;
                    counterC = _iterations;
                }
                if (!readerA.EndOfStream)
                {
                    if (counterA > 0 && !pickedA)
                    {
                        strA = readerA.ReadLine();
                        elementA = Table.GetTable(strA.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                        pickedA = true;
                    }
                }
                else
                {
                    endA = true;
                }

                if (!readerB.EndOfStream)
                {
                    if (counterB > 0 && !pickedB)
                    {
                        strB = readerB.ReadLine();
                        elementB = Table.GetTable(strB.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                        pickedB = true;
                    }
                }
                else
                {
                    endB = true;
                }

                if (!readerC.EndOfStream)
                {
                    if (counterC > 0 && !pickedC)
                    {
                        strC = readerC.ReadLine();
                        elementC = Table.GetTable(strC.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                        pickedC = true;
                    }
                }
                else
                {
                    endC = true;
                }

                if (pickedA)
                {
                    if (pickedB)
                    {
                        if (pickedC)
                        {
                            Logs.Children.Add(GetLog($"{elementA} < {elementB}"));
                            if (elementA < elementB)
                            {
                                Logs.Children.Add(GetLog($"{elementA} < {elementC}"));
                                if (elementA < elementC)
                                {
                                    Logs.Children.Add(GetLog($"Запись {elementA}"));
                                    array[idx] = elementA;
                                    counterA--;
                                    pickedA = false;
                                }
                                else
                                {
                                    Logs.Children.Add(GetLog($"Запись {elementC}"));
                                    array[idx] = elementC;
                                    counterC--;
                                    pickedC = false;
                                }
                            }
                            else
                            {
                                Logs.Children.Add(GetLog($"{elementB} < {elementC}"));
                                if (elementB < elementC)
                                {
                                    Logs.Children.Add(GetLog($"Запись {elementB}"));
                                    array[idx] = elementB;
                                    counterB--;
                                    pickedB = false;
                                }
                                else
                                {
                                    Logs.Children.Add(GetLog($"Запись {elementC}"));
                                    array[idx] = elementC;
                                    counterC--;
                                    pickedC = false;
                                }
                            }
                        }
                        else
                        {
                            Logs.Children.Add(GetLog($"{elementA} < {elementB}"));
                            if (elementA < elementB)
                            {
                                array[idx] = elementA;
                                Logs.Children.Add(GetLog($"Запись {elementA}"));
                                counterA--;
                                pickedA = false;
                            }
                            else
                            {
                                array[idx] = elementB;
                                Logs.Children.Add(GetLog($"Запись {elementB}"));
                                counterB--;
                                pickedB = false;
                            }
                        }
                    }
                    else if (pickedC)
                    {
                        Logs.Children.Add(GetLog($"{elementA} < {elementC}"));
                        if (elementA < elementC)
                        {
                            array[idx] = elementA;
                            Logs.Children.Add(GetLog($"Запись {elementA}"));
                            counterA--;
                            pickedA = false;
                        }
                        else
                        {
                            array[idx] = elementC;
                            Logs.Children.Add(GetLog($"Запись {elementC}"));
                            counterC--;
                            pickedC = false;
                        }
                    }
                    else
                    {
                        array[idx] = elementA;
                        Logs.Children.Add(GetLog($"Запись {elementA}"));
                        counterA--;
                        pickedA = false;
                    }
                }
                else if (pickedB && pickedC)
                {
                    Logs.Children.Add(GetLog($"{elementB} < {elementC}"));
                    if (elementB < elementC)
                    {
                        Logs.Children.Add(GetLog($"Запись {elementB}"));
                        array[idx] = elementB;
                        counterB--;
                        pickedB = false;
                    }
                    else
                    {
                        Logs.Children.Add(GetLog($"Запись {elementC}"));
                        array[idx] = elementC;
                        counterC--;
                        pickedC = false;
                    }
                }
                else if (pickedC)
                {
                    Logs.Children.Add(GetLog($"Запись {elementC}"));
                    array[idx] = elementC;
                    counterC--;
                    pickedC = false;
                }
                else if (pickedB)
                {
                    Logs.Children.Add(GetLog($"Запись {elementB}"));
                    array[idx] = elementB;
                    counterB--;
                    pickedB = false;
                }

                idx++;
            }
            _iterations *= 2;
        }
        private TextBlock GetLog(string text)
            => new TextBlock
            {
                Text = text,
            };

    }
}