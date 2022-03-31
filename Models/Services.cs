using GP.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GP.Models.Enum;

namespace GP.Models
{
    public class Services
    {
        [Key, DisplayName("Service code")]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Display(Name = "نوع الخدمة")]
        public string Name { get; set; }

        public string ImagePath { get; set; }

    }
    public interface IService
    {
        public IQueryable<Services> GetAllSelected();

        public Task<Services> GetOne(int id);
        public IQueryable<Services> GetAll(string search);
        public Task<DbCRUD> InsertServices(Services services);
        public Task<DbCRUD> UpdateServices(Services services);
        public Task<DbCRUD> DeleteServices(int id);
    }
    public class ServicesManage : IService
    {
        private readonly ApplicationDbContext _context;

        public ServicesManage(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Services> GetAllSelected()
        {
            return _context.TServices.AsNoTracking().AsQueryable();
        }
        public async Task<DbCRUD> DeleteServices(int id)
        {
            try
            {
                Services services= await GetOne(id);
                if (services == null)
                    return DbCRUD.isNotExisted;
                _context.TServices.Remove(services);
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

        public IQueryable<Services> GetAll(string search) => _context
                    .TServices
                    .AsNoTracking().Where(x => string.IsNullOrEmpty(search)
                   ? true : (x.Name.Contains(search))
                   );

        public async Task<Services> GetOne(int id) => await _context
                .TServices
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);

        public async Task<DbCRUD> InsertServices(Services services)
        {
            try
            {
                Services ser = await GetOne(services.Id);
                if (ser != null)
                    return DbCRUD.isExisted;
                await _context.TServices.AddAsync(services);
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

        public async Task<DbCRUD> UpdateServices(Services services)
        {
            try
            {
                Services ser = await GetOne(services.Id);
                if (ser == null)
                    return DbCRUD.isNotExisted;
                if (!string.IsNullOrEmpty(services.ImagePath))
                {
                    ser.ImagePath = services.ImagePath;
                }
                ser.Name = services.Name;
                _context.TServices.Update(ser);
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
