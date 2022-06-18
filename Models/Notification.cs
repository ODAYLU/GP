using GP.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GP.Models
{
  
    public class Notification
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string SenderId { get; set; }
        [ForeignKey("SenderId")]
        public AppUser Sender { get; set; }
        public string ReciverId { get; set; }
        [ForeignKey("ReciverId")]
        public AppUser Reciver { get; set; }
        public bool IsReaded { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string IdAction { get; set; }
    }
     public interface INotification
    {
        public Task<Notification> GetOne(long id);
        public IQueryable<Notification> GetAll();
        public Task<bool> InsertNot(Notification comment);
        public Task<bool> UpdateNot(List<Notification> comment);
        public Task<bool> DeleteNot(long id);
    }
    class NotificationManagement : INotification
    {
        private readonly ApplicationDbContext _context;

        public NotificationManagement(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteNot(long id)
        {
         var data = _context.Notifications.Where(x => x.Id == id).FirstOrDefault();
            if (data is not null)
            {
                _context.Notifications.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }

                return false;

        }

        public IQueryable<Notification> GetAll()
        {
            var data = _context.Notifications.AsQueryable();
            return data;
        }

        public async Task<Notification> GetOne(long id)
        {
          return  await _context.Notifications.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> InsertNot(Notification notification)
        {
            if (notification is not null)
            {
                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateNot(List<Notification> comment)
        {
            if (comment is not null)
            {
                 _context.Notifications.UpdateRange(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
