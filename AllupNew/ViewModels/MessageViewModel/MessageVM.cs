using AllupNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupNew.ViewModels.MessageViewModel
{
    public class MessageVM
    {
        public List<AppUser> Users { get; set; }
        public List<Inbox> Inboxes { get; set; }
        public List<Message> Messages{ get; set; }
    }
}
