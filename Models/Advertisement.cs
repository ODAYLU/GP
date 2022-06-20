using GP.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static GP.Models.Enum;

namespace GP.Models
{
    public class Advertisement
    {
        [Key,DisplayName("كود الاعلان")]
       // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long Id{ get; set; }
        [Required,DisplayName("الاسم الاول لصاحب الشركة ")]
        public string FirstName{ get; set; }
        [Required,DisplayName("الاسم الثاني لصاحب الشركة ")]
        public string LastName{ get; set; }
        [Required,DisplayName("رقم هاتف صاحب الشركة ")]
        public double PhoneNumber { get; set; }
        [Required,DisplayName("ايميل الشركة ")]
        public string Email { get; set; }
        [DisplayName(" صورة المعلن عنه ")]
        [Required(ErrorMessage = "الرجاء ادخال الصورة")]
        public string Photo { get; set; }
        [Required,DisplayName("رابط الاعلان ")]
        public string Link { get; set; }
        [Required,DisplayName("السعر ")]
        public double Price { get; set; }
        [Required,DisplayName("تاريخ بداية الاعلان  ")]
        public DateTime StartDate { get; set; }
        [Required,DisplayName("تاريخ نهاية الاعلان  ")]
        public DateTime EndDate { get; set; }
        [DisplayName("وصف الاعلان ")]
        public string Description { get; set; }
    }

    public interface IAdvertisement
    {
        public IQueryable<Advertisement> GetAll(string search="");
        public Task<Advertisement> GetOne(long id);
        public Task<DbCRUD> InsertAdvertisement(Advertisement Advertisement);
        public Task<DbCRUD> UpdateAdvertisement(Advertisement Advertisement);
        public Task<DbCRUD> DeleteAdvertisement(long id);
    }

    public class ManageAdvertisement : IAdvertisement
    {
        private readonly ApplicationDbContext _context;
        public ManageAdvertisement(ApplicationDbContext context)
        {
            this._context = context;
        }
        public async Task<DbCRUD> DeleteAdvertisement(long id)
        {
            Advertisement ads = await GetOne(id);
            if (id!=null)
            {
                try
                {
                    if (ads==null)
                    {
                        return DbCRUD.isNotExisted;
                    }
                    _context.TAdvertisement.Remove(ads);
                    _context.SaveChanges();
                    return DbCRUD.success;
                }
                catch (Exception er)
                {
                    if (er is SqlException)
                        return DbCRUD.dbError;
                    else
                        return DbCRUD.fail;
                }
            }

            return DbCRUD.fail;
        }

        public IQueryable<Advertisement> GetAll(string search = "") => _context
            .TAdvertisement
            .AsNoTracking()
            .Where(x => string.IsNullOrEmpty(search) ? true :(x.FirstName.Contains(search)))
            .AsQueryable();



        public async Task<Advertisement> GetOne(long id)=>await _context
            .TAdvertisement
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Id == id);


        public async Task<DbCRUD> InsertAdvertisement(Advertisement Advertisement)
        {
            try
            {
                Advertisement ads = await GetOne(Advertisement.Id);
                if (ads != null)
                    return DbCRUD.isExisted;
                await _context.TAdvertisement.AddAsync(Advertisement);
                await _context.SaveChangesAsync();
                return DbCRUD.success;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                    return DbCRUD.dbError;
                else
                    return DbCRUD.fail;
            }
        }

        public async Task<DbCRUD> UpdateAdvertisement(Advertisement Advertisement)
        {
            try
            {
                Advertisement ads = await GetOne(Advertisement.Id);
                if (ads == null)
                    return DbCRUD.isNotExisted;
                _context.TAdvertisement.Update(Advertisement);
                await _context.SaveChangesAsync();
                return DbCRUD.success;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                    return DbCRUD.dbError;
                else
                    return DbCRUD.fail;
            }
        }
    }
}
