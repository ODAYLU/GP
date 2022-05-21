using GP.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static GP.Models.Enum;

namespace GP.Models
{
    public class likedEstates
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("User")]
        public string IdUser { get; set; }

        public AppUser User { get; set; }

        [ForeignKey("Estate")]
        public long IdEstate { get; set; }
        public Estate Estate { get; set; }
    }

    public interface IlikedEstates
    {
        public Task<likedEstates> GetOne(long id);
        public IEnumerable<likedEstates> GetAll();
        public Task<DbCRUD> InsertObj(likedEstates estate);
        public Task<DbCRUD> DeleteObj(long id);
    }

    public class likedEstatesManage : IlikedEstates 
    {
        private readonly ApplicationDbContext _context;

        public likedEstatesManage(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<likedEstates> GetAll()
        {
            IEnumerable<likedEstates> list = _context.TlikedEstates.AsNoTracking().Include(c => c.User).Include(x => x.Estate).AsEnumerable();

            return list;
        }

        public async Task<DbCRUD> DeleteObj(long id)
        {
            try
            {
                likedEstates estate = await GetOne(id);
                if (estate == null)
                    return DbCRUD.isNotExisted;
                _context.TlikedEstates.Remove(estate);
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
        public async Task<likedEstates> GetOne(long id) => await _context
               .TlikedEstates
               .AsNoTracking()
               .Include(c => c.User).Include(x => x.Estate)
               .SingleOrDefaultAsync(c => c.Id == id);

        public async Task<DbCRUD> InsertObj(likedEstates estate)
        {
            try
            {
                likedEstates EST = await GetOne(estate.Id);
                if (EST != null)
                    return DbCRUD.isExisted;
                await _context.TlikedEstates.AddAsync(estate);
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
