using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WebStore.Hubs
{
    public class InformationHub : Hub
    {
        //метод будет использоваться скриптом SignalR (подключаем во вью SignalRTest)
        /// <summary>метод для рассылки сообщений всем, кто подключен к хабу</summary>
        /// <param name="Message">сообщение</param>
        /// <returns></returns>
        public async Task Send(string Message) => await Clients.All.SendAsync("Send", Message);
    }
}