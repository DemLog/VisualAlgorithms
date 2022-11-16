using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.Threading.Tasks;
using SortingTables.Sort;

namespace SortingTables
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DrawMove.ping = ping;
        }
        public List<Table> Rows;
        public Func<Table[], int, Task> Algorithm;
        private void tablesgr_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void SortClock(object sender, RoutedEventArgs e)
        {
            // var sa = new DirectMerge(content, logs);
            // sa.MergeSort(Rows.ToArray());
            Algorithm(Rows.ToArray(), 1);
            GetTable();
        }
        
        private void ComboBox_Selected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int selectedItem = comboBox.SelectedIndex;
            switch (selectedItem)
            {
                case 0:
                    var sa = new DirectMerge(content, logs);
                    Algorithm = sa.MergeSort;
                    break;
                case 1:
                    var sb = new NaturalMerge(content, logs);
                    Algorithm = sb.MergeSort;
                    break;
                case 2:
                    var sc = new NaturalMerge(content, logs);
                    Algorithm = sc.MergeSort;
                    break;
            }
        }
        
        public static bool checkBox = false;
        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            checkBox = true;

        }
        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            checkBox = false;

        }

        private void LoadedFile(string path)
        {
            string[] tv = new string[0];
            columns.Children.Clear();
            SortColumns.Clear();
            logs.Children.Clear();
                
            if (checkBox)
                tv = File.ReadAllText(@path).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            else
                tv = File.ReadAllLines(@path);
            
            var cl = tv[0].Split(new char[] { ';' });

            if (cl.Length == 1)
            {
                columns.Children.Add(GetBt("Текст", 0));
            }
            else
            {
                for (int i = 0; i < cl.Length; i++)
                {
                    columns.Children.Add(GetBt(cl[i], i));
                }
            }

            Rows = new List<Table>();

            for (var i = 1; i < tv.Length; i++)
            {
                var row = tv[i].Split(new char[] { ';' });
                Rows.Add(Table.GetTable(row));
            }

            SortColumns.OrderBy(el => el);
            GetTable();
        }
        private void GetTable()
        {
            content.Children.Clear();
            foreach(var el in Rows)
            {
                content.Children.Add(Table.GetStack(el, Brushes.White));
            }
        }
        private void Minus100Click(object sender, RoutedEventArgs e)
        {
            ping.Text = (int.Parse(ping.Text) - 100).ToString();
        }

        private void Minus10Click(object sender, RoutedEventArgs e)
        {
            ping.Text = (int.Parse(ping.Text) - 10).ToString();
        }

        private void Plus10Click(object sender, RoutedEventArgs e)
        {
            ping.Text = (int.Parse(ping.Text) + 10).ToString();
        }

        private void Plus100Click(object sender, RoutedEventArgs e)
        {
            ping.Text = (int.Parse(ping.Text) + 100).ToString();
        }
        private Button GetBt(string name, int i)
        {
            var bt = new Button();
            bt.Content = name;
            bt.Click += Bt_Click;
            bt.Uid = i.ToString();

            return bt;
        }
        public static List<int> SortColumns = new List<int>();
        private void Bt_Click(object sender, RoutedEventArgs e)
        {
            int id = Int32.Parse(((Button)sender).Uid);
            
            if (SortColumns.Contains(id))
                SortColumns.Remove(id);
            else
                SortColumns.Add(id);
            
            if(((Button)sender).Background == Brushes.Red)
            {
                ((Button)sender).Background = default(Brush);
            }
            else
            {
                ((Button)sender).Background = Brushes.Red;
            }
        }
        private void SelectFileClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Text documents (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.FilterIndex = 2;

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                ((Button)sender).Content = Path.GetFileName(filename);
                this.LoadedFile(filename);
            }
        }
    }
}
