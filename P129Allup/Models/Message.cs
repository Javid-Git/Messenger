using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupNew.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int InboxId { get; set; }
        public Inbox Inbox { get; set; }
    }
}
