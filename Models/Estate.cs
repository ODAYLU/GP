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
    public class Estate
    {
        [Key, DisplayName("كود العفار")]
        [Range(1000, long.MaxValue)]
        public long Id { get; set; }
        [Required(ErrorMessage = "الرجاء تحديد الموقع على الخريطة")]
        [DisplayName("المحور الصادي ")]
        public string Longitude { get; set; }
        [Required(ErrorMessage = "الرجاء تحديد الموقع على الخريطة")]
        [DisplayName("المحور السيني ")]
        public string Latitude { get; set; }
        [Required(ErrorMessage = "الحقل مطلوب")]
        [DisplayName("عنوان بسيط للعقار")]
        [StringLength(15, ErrorMessage = "يحتوي على 5 إلى 15 حرف", MinimumLength = 5)]

        public string name { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال الوصف لتعزيز عقارك")]
        [StringLength(50000, ErrorMessage = "يحتوي على الأقل 200 حرف", MinimumLength = 200)]
        [DisplayName("وصف العقار ")]
        public string description { get; set; }
        [Required(ErrorMessage = "الحقل مطلوب")]
        [DisplayName("اسم المالك ")]
        public string name_owner { get; set; }

        [Required(ErrorMessage = "الحقل مطلوب")]
        [DisplayName("مساحة العقار ")]
        [Range(1, int.MaxValue, ErrorMessage = "غير صالح")]
        public double? space { get; set; }

        public int Likes { get; set; }

        [DisplayName("سعر العقار ")]
        [Required(ErrorMessage = "الحقل مطلوب")]
        [Range(1, int.MaxValue, ErrorMessage = "غير صالح")]
        public double? price { get; set; }
        [RegularExpression(@"^([0-9]{3}[0-9]{3}[0-9]{4})$", ErrorMessage = "  رقم الهاتف غير صالح على الأقل 10 أرقام")]

        [Required(ErrorMessage = "الحقل مطلوب")]
        [DisplayName("رقم هاتف المالك")]
        public string phone_num { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال الصورة الرئيسية للعقار")]
        public string Main_photo { get; set; }
        public DateTime OnDate { get; set; }
        [NotMapped]
        public string[] list { get; set; }

        public bool is_active { get; set; }
        public bool publish { get; set; }
        public bool is_spacial { get; set; }


        public bool IsBlock { get; set; }

        [DisplayName(" نوع العقار ")]
        [Required(ErrorMessage = "الحقل مطلوب")]
        public int? categoryID { get; set; }
        [ForeignKey("categoryID")]
        public Category Category { get; set; }
        [DisplayName(" نوع العقار ")]
        [Required(ErrorMessage = "الحقل مطلوب")]
        public int? TypeID { get; set; }
        [ForeignKey("TypeID")]
        public Type Type { get; set; }
        [DisplayName(" الدولة  ")]
        [Required(ErrorMessage = "الحقل مطلوب")]
        public long? StateID { get; set; }
        [ForeignKey("StateID")]
        public State State { get; set; }
        [DisplayName(" المدينة  ")]
        [Required(ErrorMessage = "الحقل مطلوب")]
        public long? CityID { get; set; }
        [ForeignKey("CityID")]

        public City City { get; set; }
        [DisplayName(" العملة  ")]
        [Required(ErrorMessage = "الحقل مطلوب")]
        public int? CurrencyID { get; set; }
        [ForeignKey("CurrencyID")]
        public Currency Currency { get; set; }
        // اسم السمسار
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser Users { get; set; }
        public long Views { get; set; }
    }
    public interface IEstate
    {
        public Task<Estate> GetOne(long id);
        public Estate GetOnetoImage(long id);
        public IEnumerable<Estate> GetAll();
        public IQueryable<Estate> GetAllQuiers();
        public Task<DbCRUD> InsertEstate(Estate estate);
        public Task<DbCRUD> UpdateEstate(Estate estate);
        public Task<DbCRUD> DeleteEstate(long id);
    }
    public class ProductManage : IEstate
    {
        private readonly ApplicationDbContext _context;

        public ProductManage(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Estate> GetAll()
        {
            IEnumerable<Estate> list = _context.TEstates
                .AsNoTracking()
                .Include(c => c.Currency)
                .Include(x => x.Category)
                .Include(x => x.State)
                .Include(x => x.Type).Include(x => x.Users).Include(x => x.City).AsEnumerable();


            return list;
        }

        public async Task<DbCRUD> DeleteEstate(long id)
        {
            try
            {
                Estate estate = await GetOne(id);
                if (estate == null)
                    return DbCRUD.isNotExisted;
                _context.TEstates.Remove(estate);
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


        public async Task<Estate> GetOne(long id) => await _context
                .TEstates
                .AsNoTracking()
                .Include(c => c.Currency).Include(x => x.Category).Include(x => x.State).Include(x => x.Type).Include(x => x.Users).Include(x => x.City)
                .SingleOrDefaultAsync(c => c.Id == id);


        public Estate GetOnetoImage(long id) =>
            _context
            .TEstates
              .AsNoTracking()
               .Include(c => c.Currency).Include(x => x.Category).Include(x => x.State).Include(x => x.Type).Include(x => x.Users).Include(x => x.City)
               .SingleOrDefault(c => c.Id == id);

        public async Task<DbCRUD> InsertEstate(Estate estate)
        {
            try
            {
                Estate EST = await GetOne(estate.Id);
                if (EST != null)
                    return DbCRUD.isExisted;
                await _context.TEstates.AddAsync(estate);
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

        public async Task<DbCRUD> UpdateEstate(Estate estate)
        {
            try
            {
                Estate EST = await GetOne(estate.Id);
                if (EST == null)
                    return DbCRUD.isNotExisted;
                _context.TEstates.Update(estate);
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

        public IQueryable<Estate> GetAllQuiers() => _context
                .TEstates
                .AsNoTracking()
                .Include(c => c.Currency).Include(x => x.Category).Include(x => x.State).Include(x => x.Type).Include(x => x.Users).Include(x => x.City)
                .AsQueryable();

    }

}
