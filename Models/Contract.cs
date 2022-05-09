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
    public class Contract
    {
        [Key]
        public long Id { get; set; }

        [Display(Name = "اسم المالك ")]
        public string SallerName { get; set; }

        [Display(Name = "رقم  الهاتف ")]
        public double? Sallerphone_num { get; set; }
        [Required(ErrorMessage = "الحقل مطلوب")]
        [DisplayName("  العقد ")]
        public string Description { get; set; }

        [Required(ErrorMessage = "الحقل مطلوب")]
        [Display(Name = "اسم المشتري ")]
        public string BuyerName { get; set; }

        [Required(ErrorMessage = "الحقل مطلوب")]
        [DisplayName("رقم هاتف المشتري ")]
        public double? Buyerphone_num { get; set; }

        [Display(Name = "الاحداثي السيني  ")]
        public string Longitude { get; set; }
        [Display(Name = "الاحداثي الصادي   ")]
        public string Latitude { get; set; }
        [Display(Name = "تاريخ كتابة العقد  ")]
        public DateTime OnDate { get; set; }

        [Display(Name = " تاريخ نهاية العقد ")]
        public DateTime up_to_date { get; set; }
        [Display(Name = "نوع العقار  ")]
        public string Type { get; set; }
        [Display(Name = "تصنيف العقار  ")]
        public string category { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser Users { get; set; }
        public long IDEstet { get; set; }
        [ForeignKey("IDEstet")]
        public Estate Estate { get; set; }

    }


    public interface IContract
    {
        public Task<Contract> GetOne(long id);
        public IEnumerable<Contract> GetAll();
        public Task<DbCRUD> InsertContract(Contract contract);
        public Task<DbCRUD> UpdateContract(Contract contract);
        
    }

    public class ContractManage : IContract
    {
        private readonly ApplicationDbContext _context;

        public ContractManage(ApplicationDbContext context)
        {
            this._context = context;
        }
        public IEnumerable<Contract> GetAll()
        {
            IEnumerable<Contract> list = _context.TContract.AsEnumerable();
            return list;
        }

        public async Task<Contract> GetOne(long id) => await _context
                   .TContract
                   .AsNoTracking()
                   .SingleOrDefaultAsync(c => c.Id == id);


        public async Task<DbCRUD> InsertContract(Contract contract)
        {
            try
            {
                Contract EST = await GetOne(contract.Id);
                if (EST != null)
                    return DbCRUD.isExisted;
                await _context.TContract.AddAsync(contract);
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

        public Task<DbCRUD> UpdateContract(Contract contract)
        {
            throw new NotImplementedException();
        }
    }
}
