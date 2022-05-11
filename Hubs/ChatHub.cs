using GP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using GP.Data;

namespace GP.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string message, string Id) { 
            Message msg = new Message 
            { 
                Text = message,
                Time = DateTime.Now,
                ReceiverId = Id,
                UserId = Context.UserIdentifier,
                UserName = Context.User.Identity.Name
            
            };   
             await _context.Messages.AddAsync(msg);
            await Clients.User(Id).SendAsync("receiveMessage", msg);
        }
        public override async Task OnConnectedAsync()
        {
            if(!ConnectedUser.IDs.Contains(Context.UserIdentifier))
            ConnectedUser.IDs.Add(Context.UserIdentifier);
           await  Clients.All.SendAsync("connectedUsers",ConnectedUser.IDs);
           
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (ConnectedUser.IDs.Contains(Context.UserIdentifier))
                ConnectedUser.IDs.Remove(Context.UserIdentifier);
            await Clients.All.SendAsync("connectedUsers", ConnectedUser.IDs);
            await base.OnDisconnectedAsync(exception);
        }
    }
    public static class ConnectedUser
    {
        public static List<string> IDs = new List<string>();
    }
}
