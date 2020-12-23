using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;

namespace HomeShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Email : Controller
    {
        [HttpPost]
        public ActionResult<IEnumerable<bool>> SendEmail()
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("HomeShopping", "minhman231@gmail.com"));
                message.To.Add(new MailboxAddress("User", "nguyenhuynhminhman2311999@gmail.com"));
                message.Subject = "My First Email";
                message.Body = new TextPart("plain")
                {
                    Text = "ABC"
                };
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("minhman231@gmail.com", "Minhman23199");
                    client.Send(message);
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Error occured");
            }
            return Ok(true);
        }
      
    }
}
