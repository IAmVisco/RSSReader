using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
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
        List<Thread> threads = new List<Thread>();
        bool anyAlive = true;
        object locker = new object();
        string body = "";

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
                foreach (var feed in feeds)
                {
                    Thread th = new Thread(() =>
                    {
                        Thread.CurrentThread.IsBackground = true;
                        LoadFeed(feed);
                    });
                    threads.Add(th);
                    th.Start();
                }
                while (anyAlive)
                {
                    anyAlive = false;
                    foreach (var th in threads)
                        if (th.IsAlive)
                        {
                            anyAlive = true;
                            break;
                        }
                }
                SendEmail();
            }
            else
            {
                MessageBox.Show("Select at least one feed!");
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
            CombineContent(new SortedDictionary<string, string>(content));
        }

        private void AddToFeeds(string subreddit)
        {
            feeds.Add(String.Format("http://reddit.com{0}/.rss", subreddit));
        }

        private void CombineContent(SortedDictionary<string, string> content)
        {
            lock (locker)
            {
                body += "-------------------------------------------------------------------------------------------------------\n";
                foreach (var pair in content)
                {
                    body += String.Format("{0}\n{1}\n", pair.Key, pair.Value);
                }
                Thread.CurrentThread.Abort();
            }
        }

        private void SendEmail()
        {
            MailMessage mail = new MailMessage("recipient@gmail.com", EmailField.Text, "RSS Feed", body);
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Timeout = 3000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("your@gmail.com", "your_16_symbol_gmail_access_token")
            };
            client.Send(mail);
        }
    }
}
