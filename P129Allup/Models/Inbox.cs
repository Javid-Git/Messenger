using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupNew.Models
{
    public class Inbox
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public List<Message> Messages { get; set; }
    }
}
