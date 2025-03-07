using Microsoft.AspNetCore.SignalR;

namespace MikuSpaceServer.Hubs
{
    public class NotificationHub:Hub
    {
        // 客户端可以调用此方法向服务器发送消息
        public async Task SendMessage(string user, string message)
        {
            // 向所有客户端广播消息
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
