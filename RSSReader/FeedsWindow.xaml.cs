using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RSSReader
{
    /// <summary>
    /// Interaction logic for FeedsWindow.xaml
    /// </summary>
    public partial class FeedsWindow : Window
    {
        public int SelectedIndex { get; set; }
        public List<string> Feeds { get; set; }

        public FeedsWindow(List<string> feeds, int index)
        {
            InitializeComponent();
            foreach (var item in feeds)
                FeedsBox.Items.Add(item);
            FeedsBox.SelectedIndex = index;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (InputBox.Text != "")
                FeedsBox.Items.Add(InputBox.Text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SelectedIndex = FeedsBox.SelectedIndex;
            Feeds = new List<string>();
            foreach (string item in FeedsBox.Items)
                Feeds.Add(item);
        }
    }
}
