using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllupNew.Controllers
{
    public class MessageController : Controller
    {
        public async Task<IActionResult> OpenChatBox(string userId)
        {
            return View();
        }
    }
}
