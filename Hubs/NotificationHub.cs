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
	public class NotificationHub : Hub
	{
		//private readonly ApplicationDbContext _context;
		//private readonly SignInManager<AppUser> _signInManager;
		//private readonly UserManager<AppUser> _userManager;
		//public NotificationHub(ApplicationDbContext context,
		//   SignInManager<AppUser> signInManager,
		//   UserManager<AppUser> userManager)
		//{
		//    _context = context;
		//    _signInManager = signInManager;
		//    _userManager = userManager;
		//}


		//public async Task SendNotification(string Text, string Id,string Type)
		//{
		//    Notification msg = new Notification
		//    {
		//        Text = Text,
		//        Time = DateTime.Now,
		//        ReciverId = Id,
		//        SenderId = Context.UserIdentifier,
		//        Type = Type
		//    };
		//    await _context.Notifications.AddAsync(msg);
		//    await _context.SaveChangesAsync();
		//    await Clients.User(Id).SendAsync("receiveNotification", msg);
		//}
		internal Task SendAsync(string v, Notification msg)
		{
			throw new NotImplementedException();
		}
	}
}
