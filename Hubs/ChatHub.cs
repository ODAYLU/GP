using GP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using GP.Data;
using Microsoft.AspNetCore.Identity;

namespace GP.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public ChatHub(ApplicationDbContext context, 
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task SendMessage(string message, string Id) 
        { 
            Message msg = new Message 
            { 
                Text = message,
                Time = DateTime.Now,
                ReceiverId = Id,
                UserId = Context.UserIdentifier,
                UserName = Context.User.Identity.Name
                

            };   
             await _context.Messages.AddAsync(msg);
            await _context.SaveChangesAsync();
            await Clients.User(Id).SendAsync("receiveMessage", msg);
        }


        public override async Task OnConnectedAsync()
        {
            if (!ConnectedUser.IDs.Contains(Context.UserIdentifier))
                ConnectedUser.IDs.Add(Context.UserIdentifier);
            if(!!ConnectedUser.IDsVistor.Contains(Context.ConnectionId))
                ConnectedUser.IDsVistor.Add(Context.ConnectionId);
            await Clients.All.SendAsync("connectedUsers", ConnectedUser.IDs);
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
        public static List<string> IDsVistor = new List<string>();
    }
}
