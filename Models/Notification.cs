using GP.Data;
using System;
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
    }
     public interface INotification
    {
        public Task<Notification> GetOne(long id);
        public IQueryable<Notification> GetAll(string search = "");
        public Task<bool> InsertNot(Comments comment);
        public Task<bool> UpdateNot(Comments comment);
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

        public IQueryable<Notification> GetAll(string search = "")
        {
            throw new NotImplementedException();
        }

        public Task<Notification> GetOne(long id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertNot(Comments comment)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateNot(Comments comment)
        {
            throw new NotImplementedException();
        }
    }
}
