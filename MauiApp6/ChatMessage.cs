using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp6
{
    internal class ChatMessage
    {
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}