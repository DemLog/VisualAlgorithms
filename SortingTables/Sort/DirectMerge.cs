using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SortingTables.Sort
{
    public class DirectMerge
    {
        private const string PATH_LEFT = @"../../../left.txt";
        private const string PATH_RIGHT = @"../../../right.txt";
        private const string PATH_MID = @"../../../mid.txt";
        public List<int> LeftInd = new List<int>();
        public List<int> RightInd = new List<int>();
        public DrawMove DrawMove;
        public StackPanel Logs;
        public DirectMerge(StackPanel content, StackPanel logs)
        {
            DrawMove = new DrawMove(content);
            Logs = logs;
        }
        public async Task MergeSort(Table[] array, int jump = 1)
        {
            File.WriteAllText(PATH_LEFT, "");
            File.WriteAllText(PATH_RIGHT, "");
            
        
            for (int i = 0; i< array.Length;)
            {
                OneComb(array, jump, ref i, PATH_LEFT, LeftInd);
        
                if (i >= array.Length) break;
        
                OneComb(array, jump, ref i, PATH_RIGHT, RightInd);
        
                await DrawMove.Alg(array, LeftInd, RightInd);
        
                Sort1(array, i - jump*2);
        
                await DrawMove.Alg(array, LeftInd, RightInd);
        
                await DrawMove.Alg2(array, LeftInd, RightInd);
            }
        
        
            if (!IsSort(array))
                await MergeSort(array, jump*2);
        }
        private bool IsSort(Table[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] > array[i])
                {
                    return false;
                }
            }
        
            return true;
        }
        private void OneComb(Table[] array, int jump, ref int i, string path, List<int> inds)
        {
            for (int j = 0; j < jump; j++)
            {
                if (i + j >= array.Length) break;
                WriteIntoFile(path, array[i + j].ToString());
                inds.Add(i + j);
            }

            i += jump;
        }
        private void WriteIntoFile(string path, string word)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(path, true);
            writer.WriteLine(word);
            writer.Close();
        }
        
        private void Sort1(Table[] array, int k)
        {
            using (StreamReader readerLeft = new StreamReader(PATH_LEFT))
            using (StreamReader readerRight = new StreamReader(PATH_RIGHT))
            {
                string lineLeft = readerLeft.ReadLine(), lineRight = readerRight.ReadLine();
                
                while (lineLeft != null && lineRight != null)
                {
                    var leftT = Table.GetTable(lineLeft.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                    var rightT = Table.GetTable(lineRight.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
        
                    if (leftT < rightT)
                    {
                        Logs.Children.Add(GetLog($"{leftT} < {rightT}"));
                        Logs.Children.Add(GetLog($"{k} позиция -> {leftT.ToString()}"));
                        array[k] = leftT;
                        lineLeft = readerLeft.ReadLine();
                    }
                    else
                    {
                        Logs.Children.Add(GetLog($"{leftT} > {rightT}"));
                        Logs.Children.Add(GetLog($"{k} позиция -> {rightT.ToString()}"));
                        array[k] = rightT;
                        lineRight = readerRight.ReadLine();
                    }
                    k++;
                }
        
                while (lineLeft != null || lineRight != null)
                {
                    if (lineLeft == null)
                    {
                        Logs.Children.Add(GetLog($"{k} позиция -> {Table.GetTable(lineRight.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))}"));
                        array[k] = Table.GetTable(lineRight.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                        lineRight = readerRight.ReadLine();
                    }
                    else
                    {
                        Logs.Children.Add(GetLog($"{k} позиция -> {Table.GetTable(lineLeft.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))}"));
                        array[k] = Table.GetTable(lineLeft.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                        lineLeft = readerLeft.ReadLine();
                    }
                    k++;
                }
            }
            
            
            File.WriteAllText(PATH_LEFT, "");
            File.WriteAllText(PATH_RIGHT, "");
        }
        private TextBlock GetLog(string text)
            => new TextBlock
            {
                Text = text,
            };
    }
}
