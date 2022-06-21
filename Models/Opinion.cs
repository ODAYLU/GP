using GP.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static GP.Models.Enum;

namespace GP.Models
{
    public class Opinion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Body { get; set; }
        public bool is_active { get; set; }

        public int Rating { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser Users { get; set; }
    }

    public interface IOpinion
    {
        public Task<Opinion> GetOne(int id);
        public IQueryable<Opinion> GetAll(string search = "");
        public Task<DbCRUD> InsertOpinion(Opinion opinion);
        public Task<DbCRUD> UpdateOpinion(Opinion opinion);
    }

    public class OpinionManage : IOpinion
    {
        private readonly ApplicationDbContext _context;

        public OpinionManage(ApplicationDbContext context)
        {
            _context = context;
        }



        public IQueryable<Opinion> GetAll(string search = "") => _context
                   .TOpinion
                   .AsNoTracking().Where(x => string.IsNullOrEmpty(search) ? true :
                      (x.Body.Contains(search))
                   )
                   .Include(x => x.Users).AsQueryable();

        public async Task<Opinion> GetOne(int id) => await _context
                .TOpinion
                .AsNoTracking().Include(x => x.Users)
                .SingleOrDefaultAsync(c => c.Id == id);




        public async Task<DbCRUD> InsertOpinion(Opinion opinion)
        {
            try
            {
                Opinion ct = await GetOne(opinion.Id);
                if (ct != null)
                    return DbCRUD.isExisted;
                await _context.TOpinion.AddAsync(opinion);
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

        public async Task<DbCRUD> UpdateOpinion(Opinion opinion)
        {
            try
            {
                Opinion cty = await GetOne(opinion.Id);
                if (cty == null)
                    return DbCRUD.isNotExisted;

                _context.TOpinion.Update(opinion);
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
