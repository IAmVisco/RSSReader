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
            var feed = SyndicationFeed.Load(XmlReader.Create(@"http://www.reddit.com/r/news/.rss"));
            foreach (SyndicationItem item in feed.Items)
            {
                links.Add(item);
                LinksList.Items.Add(item.Title.Text);
            }
        }
    }
}
