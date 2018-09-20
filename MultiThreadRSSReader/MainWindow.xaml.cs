using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace MultiThreadRSSReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> feeds = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox c in checkboxGrid.Children)
            {
                if (c.IsChecked == true)
                    AddToFeeds(c.Content as string);
            }
            if (feeds.Count > 0)
            {
                foreach(var feed in feeds)
                {
                    Thread th = new Thread(() => LoadFeed(feed));
                    th.Start();
                }
            }
        }
        
        private void LoadFeed(string url)
        {
            SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(url));
            Dictionary<string, string> content = new Dictionary<string, string>();
            foreach (SyndicationItem item in feed.Items)
            {
                content.Add(item.Title.Text, item.Links[0].Uri.AbsoluteUri);
            }
            // TODO: Add sorting and lock combine
        }

        private void AddToFeeds(string subreddit)
        {
            feeds.Add(String.Format("http://reddit{0}/.rss", subreddit));
        }
    }
}
