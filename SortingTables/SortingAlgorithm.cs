using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;

namespace SortingTables
{
    public class SortingAlgorithm
    {
        private const string PATH_LEFT = @"../../../left.txt";
        private const string PATH_RIGHT = @"../../../right.txt";
        private const string PATH_MID = @"../../../mid.txt";
        public List<int> LeftInd = new List<int>();
        public List<int> RightInd = new List<int>();
        public DrawMove DrawMove;
        public StackPanel Logs;
        public SortingAlgorithm(StackPanel content, StackPanel logs)
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
            
            var left = File.ReadAllLines(PATH_LEFT);
            var right = File.ReadAllLines(PATH_RIGHT);
            int l = 0, r = 0;
        
        
            while (l < left.Length && r < right.Length)
            {
                var leftT = Table.GetTable(left[l].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                var rightT = Table.GetTable(right[r].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
        
                if (leftT < rightT)
                {
                    Logs.Children.Add(GetLog($"{leftT} < {rightT}"));
                    Logs.Children.Add(GetLog($"{k} позиция -> {leftT.ToString()}"));
                    array[k] = leftT;
                    l++;
                }
                else
                {
                    Logs.Children.Add(GetLog($"{leftT} > {rightT}"));
                    Logs.Children.Add(GetLog($"{k} позиция -> {right}"));
                    array[k] = rightT;
                    r++;
                }
        
                k++;
            }
        
            while (l != left.Length || r != right.Length)
            {
                if (l == left.Length)
                {
                    Logs.Children.Add(GetLog($"{k} позиция -> {Table.GetTable(right[r].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))}"));
                    array[k] = Table.GetTable(right[r].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                    r++;
                }
                else
                {
                    Logs.Children.Add(GetLog($"{k} позиция -> {Table.GetTable(left[l].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))}"));
                    array[k] = Table.GetTable(left[l].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                    l++;
                }
        
                k++;
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
