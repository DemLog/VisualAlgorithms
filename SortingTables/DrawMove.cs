using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace SortingTables
{
    public class DrawMove
    {
        public StackPanel Content;
        public static TextBox Ping
        {
            get
            {
                return ping;
            }
            set
            {
                if (value.Text == "" || int.Parse(value.Text) < 0)
                    ping.Text = 0.ToString();
                else
                    ping.Text = value.Text;
            }
        }

        public static TextBox ping;
        public DrawMove(StackPanel content)
        {
            Content = content;
        }
        public async Task Alg(Table[] Elements, List<int> leftind, List<int> rightind)
        {
            Content.Children.Clear();

            for(int i = 0; i < Elements.Length; i++)
            {
                Brush br;
                if (leftind.Contains(i))
                    br = Brushes.Red;
                else if (rightind.Contains(i))
                    br = Brushes.Green;
                else
                    br = Brushes.White;

                Content.Children.Add(Table.GetStack(Elements[i], br));
            }            
           
            await Task.Delay(int.Parse(Ping.Text));
        }
        public async Task Alg2 (Table[] array, List<int> leftind, List<int> rightind)
        {
            Content.Children.Clear();
            leftind.Clear();
            rightind.Clear();

            foreach (var el in array)
                Content.Children.Add(Table.GetStack(el, Brushes.White));

            await Task.Delay(int.Parse(Ping.Text));
        }
    }
}
