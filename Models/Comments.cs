using GP.Data;
using GP.Hubs;
using Microsoft.AspNetCore.Identity;
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
    public class Comments
    {
        [Key]
        public long Id { get; set; }
        [Required,MaxLength(200,ErrorMessage ="لحد 200 حرف ")]
        public string Body { get; set; }
        public double Rating { get; set; }

        public DateTime OnDate { get; set; }
        public long EstateId { get; set; }
        [ForeignKey("EstateId")]

        public Estate Estate { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

    }

    public interface ICommments
    {
        public Task<Comments> GetOne(long id);
        public IQueryable<Comments> GetAll(string search = "");
        public Task<DbCRUD> InsertComment(Comments comment);
        public Task<DbCRUD> UpdateComment(Comments comment);
        public Task<DbCRUD> DeleteComment(long id);
    }

    public class CommentsManagments : ICommments
    {
        private readonly ApplicationDbContext _context;
        public CommentsManagments(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<DbCRUD> DeleteComment(long id)
        {
            
            Comments com = await GetOne(id);
            if (com!=null)
            {
                try
                {
                    if (com == null)
                        return DbCRUD.isNotExisted;
                    _context.TComments.Remove(com);
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
            return DbCRUD.fail;

        }

        public IQueryable<Comments> GetAll(string search = "")=>_context
                    .TComments
                    .AsNoTracking().Include(c=>c.AppUser).Include(c=>c.Estate).Where(x => string.IsNullOrEmpty(search)? true :
                       (x.Body.Contains(search))
                    )
                    .AsQueryable();
       





        public async Task<Comments> GetOne(long id) => await _context
                .TComments
                .AsNoTracking()
                .Include(c => c.AppUser).Include(x=>x.Estate)
                .SingleOrDefaultAsync(c => c.Id == id);

        public async Task<DbCRUD> InsertComment(Comments comment)
        {
            try
            {
                Comments com= await GetOne(comment.Id);
                if (com != null)
                    return DbCRUD.isExisted;
                await _context.TComments.AddAsync(comment);
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

        public async Task<DbCRUD> UpdateComment(Comments comment)
        {
            try
            {
                Comments com= await GetOne(comment.Id);
                if (com == null)
                    return DbCRUD.isNotExisted;
                _context.TComments.Update(comment);
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
