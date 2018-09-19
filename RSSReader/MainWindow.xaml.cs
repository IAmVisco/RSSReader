using System;
using System.Windows;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;

namespace RSSReader
{
    public partial class MainWindow : Window
    {
        List<SyndicationItem> links = new List<SyndicationItem>();
        List<String> feeds = new List<string>()
        {
            @"http://www.reddit.com/r/news/.rss",
            @"http://www.reddit.com/r/politics/.rss",
            @"http://www.reddit.com/r/pics/.rss"
        };
        int currentFeed = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListItemClick(object sender, SelectionChangedEventArgs e)
        {
            int n = (sender as ListBox).SelectedIndex;

            Browser.Navigate(links[n].Links[0].Uri.AbsoluteUri);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBtn_Click(null, null);
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                links.Clear();
                LinksList.Items.Clear();
                var feed = SyndicationFeed.Load(XmlReader.Create(feeds[currentFeed]));
                foreach (SyndicationItem item in feed.Items)
                {
                    links.Add(item);
                    LinksList.Items.Add(item.Title.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            FeedsWindow feedsWin = new FeedsWindow(feeds, currentFeed);
            if (feedsWin.ShowDialog() == false)
            {
                feeds = feedsWin.Feeds;
                currentFeed = feedsWin.SelectedIndex;
                UpdateBtn_Click(null, null);
            }
        }
    }
}
