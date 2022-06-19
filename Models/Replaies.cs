using GP.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static GP.Models.Enum;

namespace GP.Models
{
    public class Replaies
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "الحقل مطلوب")]
        public string body { get; set; }
         public long CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comments Comments { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

    }
    public interface IReplaies
    {
        public Task<Replaies> GetOne(long id);
        public IQueryable<Replaies> GetAll(string search = "");
        public Task<DbCRUD> InsertReply(Replaies replaies);
        public Task<DbCRUD> UpdateReply(Replaies replaies);
        public Task<DbCRUD> DeleteReply(long id);
        public Task<List<Replaies>>  GetCommentReplies(long id);
    }
    public class ManageReplayies : IReplaies
    {
        private readonly ApplicationDbContext _context;
        public ManageReplayies(ApplicationDbContext context)
        {
            this._context = context;
        }
 
         public async Task<DbCRUD> DeleteReply(long id)
        {

            Replaies rep= await GetOne(id);
            if (rep != null)
            {
                try
                {
                    _context.Remove(rep);
                    await _context.SaveChangesAsync();

                    return DbCRUD.success;
                }
                catch (Exception er)
                {
                    throw er;
                }

            }
            return DbCRUD.fail;
        }

        public IQueryable<Replaies> GetAll(string search = "")=> _context
                    .TReplaies
                    .AsNoTracking().Include(x => x.AppUser).Include(x => x.Comments).ThenInclude(x => x.Estate).Where(x => string.IsNullOrEmpty(search) ? true :
                       (x.body.Contains(search))
                    )
                    .AsQueryable();

        public async Task<List<Replaies>>  GetCommentReplies(long id) 
        {
             return  await  _context
               .TReplaies
               .AsNoTracking().Where(x => x.CommentId.Equals(id)).Include(x=>x.AppUser).Include(x => x.Comments).ThenInclude(x => x.Estate).ToListAsync();
        }
           
        

        public async Task<Replaies> GetOne(long id) => await _context
                .TReplaies
                .AsNoTracking()
.Include(x => x.AppUser).Include(x => x.Comments).ThenInclude(x => x.Estate).SingleOrDefaultAsync(c => c.Id == id);

        public async Task<DbCRUD> InsertReply(Replaies replaies)
        {
            try
            {
                Replaies rep = await GetOne(replaies.Id);
                if (rep != null)
                    return DbCRUD.isExisted;
                await _context.TReplaies.AddAsync(replaies);
                await _context.SaveChangesAsync();
                return DbCRUD.success;
            }
            catch (System.Exception ex)
            {
                if (ex is SqlException)
                    return DbCRUD.dbError;
                else
                    return DbCRUD.fail;
            }
        }

        public async Task<DbCRUD> UpdateReply(Replaies replaies)
        {
            try
            {
                Replaies rep= await GetOne(replaies.Id);
                if (rep == null)
                    return DbCRUD.isNotExisted;
                _context.TReplaies.Update(replaies);
                await _context.SaveChangesAsync();
                return DbCRUD.success;
            }
            catch (System.Exception ex)
            {
                if (ex is SqlException)
                    return DbCRUD.dbError;
                else
                    return DbCRUD.fail;
            }
        }
    }
}
