using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.Hubs
{
    public class ConversationHub : Hub
    {
        static ConcurrentDictionary<string, string> users = new ConcurrentDictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            string email = httpContext.Request.Query["email"].ToString();

            if (string.IsNullOrWhiteSpace(email))
            {
                Clients.Caller.SendAsync("sayhello", "Inputu bosh qoymayin");
            }
            else
            {
                //Clients.Caller.SendAsync("sayhello", $"Xosh geldiniz " +
                //    $"{email}, sizin ConnectionId-niz {Context.ConnectionId}-dir.");


                users.AddOrUpdate(email, Context.ConnectionId, (k, v) => v);
            }

            // Sadece daxil olana gedir mesaj

            //Clients.Caller.SendAsync("sayhello", "Xoş gəlmisiniz!",
            //    "Admin çox şad oldu!"
            //    );

            // Daxil olandan bashqa hamiya gedir mesaj

            //Clients.Others.SendAsync("sayhello", "Xoş gəlmisiniz!",
            //    "Admin çox şad oldu!"
            //    );

            // Herkese geder mesaj

            //Clients.All.SendAsync("sayhello", "Xoş gəlmisiniz!",
            //    "Admin çox şad oldu!"
            //    );

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public Task Send(string toMail, string toMsg)
        {

            if (users.TryGetValue(toMail, out string connId))
            {
                var fromMail =
                users.FirstOrDefault(to => to.Value == Context.ConnectionId).Key;

                Clients.Client(connId).SendAsync("mesajgeldi", toMail, toMsg);
            }

            return Task.CompletedTask;
        }

        public Task JoinToGroup(string userEmail, string groupName)
        {

            var fromMail =
            users.FirstOrDefault(to => to.Value == Context.ConnectionId).Key;

            if (!string.IsNullOrEmpty(fromMail))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, groupName);

                Clients.Group(groupName).SendAsync("groupaQoshulma", userEmail, groupName);
            }

            return Task.CompletedTask;
        }

        public Task SendToGroup(string groupName, string chatMessage)
        {

            var fromMail =
            users.FirstOrDefault(to => to.Value == Context.ConnectionId).Key;

            if (!string.IsNullOrEmpty(fromMail))
            {
                Clients.GroupExcept(groupName, new string[] {
                Context.ConnectionId
                })
                    .SendAsync("groupaGonderenXaricMesajiGostermek", fromMail, chatMessage);
            }

            return Task.CompletedTask;
        }
    }
}
