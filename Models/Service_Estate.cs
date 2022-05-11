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
    public class Service_Estate
    {
        [Key]
        public  int ID { get; set; }
        public int ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        public Services Services { get; set; }

        public long EstateID{ get; set; }
       
        [ForeignKey("EstateID")]
        public Estate Estate{ get; set; }

    }
    public interface IService_Estate 
    {
        public List<Service_Estate> GetAllEstate(long id);
        public string Vs(long id);
        public Task<Service_Estate> Getone(long id);
        public IQueryable<Services> GetALl(long id);
        public Task<DbCRUD> InsertService_Estate(Service_Estate service_Estate);
        public Task<DbCRUD> UpdateService_Estate(Service_Estate service_Estate);
        public Task<DbCRUD> DeleteService_Estate(long id);
    }

    public class ManageService_Estate : IService_Estate
    {
        private readonly Data.ApplicationDbContext _context;
        public ManageService_Estate(Data.ApplicationDbContext context)
        {
            this._context= context; 
        }

        public string Vs (long id)
        {

            string x = _context.TService_Estate.Where(s => s.EstateID == id).ToString();

            return x;


        }
        public async Task<DbCRUD> DeleteService_Estate(long id)
        {
           List<Service_Estate> service =  _context.TService_Estate.Where(x=>x.EstateID==id).ToList();
            try
            {
                if (service!=null)
                {
                    for (int i = 0; i < service.Count(); i++)
                    {
                        _context.TService_Estate.Remove(service[i]);
                    }
                   

                    await  _context.SaveChangesAsync();

                    return DbCRUD.success;
                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return DbCRUD.fail;
        }

        public IQueryable<Services> GetALl(long id)
        {
           List<Service_Estate> list_ser=  _context.TService_Estate.Where(s => s.EstateID == id).Include(x=>x.Services).ToList();
           List<Services> services = new List<Services>();

            for(int i = 0; i < list_ser.Count; i++)
            {
                services.Add(list_ser[i].Services);

            }

            return services.AsQueryable();
        }
        public List<Service_Estate> GetAllEstate(long id)
        {
            List<Service_Estate> list_ser = _context.TService_Estate.Where(s => s.EstateID == id).AsNoTracking().ToList();
            return list_ser;
        }


        public async Task<Service_Estate> Getone(long id) => await _context
                .TService_Estate
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.ID == id);

        public async Task<DbCRUD> InsertService_Estate(Service_Estate service_Estate)
        {
            try
            {
                Service_Estate se = await Getone(service_Estate.ID);
                if (se != null)
                    return DbCRUD.isExisted;
                await _context.TService_Estate.AddAsync(service_Estate);
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

        public async Task<DbCRUD> UpdateService_Estate(Service_Estate service_Estate)
        {
            try
            {
                Service_Estate se = await Getone(service_Estate.ID);
                if (se == null)
                    return DbCRUD.isNotExisted;
                _context.TService_Estate.Update(service_Estate);
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
