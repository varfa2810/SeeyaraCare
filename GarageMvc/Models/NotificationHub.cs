using Microsoft.AspNetCore.SignalR;

namespace GarageMvc.Models
{
    public class NotificationHub: Hub
    {
        public async Task SendNotification(string user , string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", user, message);
        }
    }
    
}
