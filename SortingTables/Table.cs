using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using OfficeOpenXml;

namespace SortingTables
{
    public class Table
    {
        public Table(string[] row)
        {
            Column = row;
        }
        public string[] Column { get; set; }
        public override string ToString()
        {
            StringBuilder row = new StringBuilder();
            foreach (var col in Column)
            {
                row.Append(col + ";");
            }

            return row.ToString();
        }
        public static Table GetTable(string[] array)
        {
            return new Table(array);
        }
        public static bool operator <(Table left, Table right)
        {
            return Sorting(right, left);
        }
        public static bool operator >(Table left, Table right)
        {
            return Sorting(left, right);
        }

        public static bool operator ==(Table left, Table right)
        {
            return !Sorting(left, right) && !Sorting(right, left);
        }

        public static bool operator !=(Table left, Table right)
        {
            return Sorting(left, right) && Sorting(right, left);
        }

        public static bool operator >=(Table left, Table right)
        {
            return left > right || left == right;
        }

        public static bool operator <=(Table left, Table right)
        {
            return left < right || left == right;
        }

        private static bool Sorting(Table left, Table right)
        {
            foreach(var el in MainWindow.SortColumns)
            {
                if (!GetColumn(left, el).Equals(GetColumn(right, el)))
                {
                    if (long.TryParse(GetColumn(left, el), out var a) && long.TryParse(GetColumn(right, el), out var b))
                    {
                        return a > b;
                    }
                    else if (double.TryParse(GetColumn(left, el).Replace('.', ','), out var c) &&
                             double.TryParse(GetColumn(right, el).Replace('.', ','), out var d))
                    {
                        return c > d;
                    }
                    else
                    {
                        return CompareStrings(GetColumn(left, el), GetColumn(right, el));
                    }

                }
            }

            return false;
        }
        private static string GetColumn(Table tbl, int i)
        {
            return tbl.Column[i];
        }
        
        private static bool CompareStrings(string left, string right)
        {
            for(int i = 0; i < Math.Min(left.Length, right.Length); i++)
            {
                if (left[i] > right[i])
                    return true;
                else if (left[i] < right[i])
                    return false;
            }

            if (left.Length > right.Length)
                return true;
            else
                return false;
        }
        public static StackPanel GetStack(Table table, Brush color)
        {
            var st = new StackPanel();
            st.Orientation = Orientation.Horizontal;
            st.Background = color;
            st.Margin = new System.Windows.Thickness(5, 5, 5, 5);
            foreach (var col in table.Column)
            {
                st.Children.Add(new TextBlock() { Text = col + ";" });
            }
            return st;
        }
    }
}
