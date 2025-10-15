using MauiApp6;
using System.Net.Http.Json;
using System.Web;

namespace MauiApp6
{
    public partial class MainPage : ContentPage
    {
        DateTime lastChatRefresh;
        IDispatcherTimer timer;
        public MainPage()
        {
            InitializeComponent();           
            lastChatRefresh = DateTime.UnixEpoch; 
            timer = Application.Current.Dispatcher.CreateTimer();            
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                GetHistory();
            };          
            timer.Start();
        }

        private void Send(object sender, EventArgs e)
        {
            
            string userName = UsernameEntry.Text;            
            string message = ChatEntry.Text;          
            ChatEntry.Text = string.Empty;       
            using (HttpClient client = new HttpClient())
            {
               
                client.BaseAddress = new Uri("http://192.168.6.25:5000/");
                
                ChatMessage chatMessage = new ChatMessage
                {
                    Author = userName,
                    Content = message,
                    Timestamp = DateTime.Now
                };
                HttpResponseMessage response = client.PostAsJsonAsync("chat", chatMessage).Result;
                if (!response.IsSuccessStatusCode)
                {
                    DisplayAlert("Error", "Failed to send message", "OK");
                }
            }
        }
        private void GetHistory()
        {           
            string timestamp = lastChatRefresh.ToString("o");           
            timestamp = HttpUtility.UrlEncode(timestamp);          
            using (HttpClient client = new HttpClient())
            {               
                client.BaseAddress = new Uri("http://192.168.6.25:5000/");               
                HttpResponseMessage response = client.GetAsync("chat?timestamp=" + timestamp).Result;                
                List<ChatMessage> messages = response.Content.ReadFromJsonAsync<List<ChatMessage>>().Result
                                                                ?? new List<ChatMessage>();               
                foreach (ChatMessage message in messages)
                {
                    Label messageLabel = new Label();
                    messageLabel.Text = message.Author + ": " + message.Content;
                    ChatHistory.Children.Add(messageLabel);
                }
            }
            ChatScrollView.ScrollToAsync(ChatHistory, ScrollToPosition.End, true);           
            lastChatRefresh = DateTime.Now;
        }
    }
}