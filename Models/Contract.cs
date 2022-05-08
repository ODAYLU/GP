using GP.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using static GP.Models.Enum;

namespace GP.Models
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }

       
        [Required(ErrorMessage = "الحقل مطلوب")]
        [Display(Name ="اسم المالك")]
        public string SallerName { get; set; }

        [Required(ErrorMessage = "الحقل مطلوب")]
        [DisplayName("رقم هاتف المالك")]
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


        [Required(ErrorMessage = "الرجاء تحديد الموقع على الخريطة")]
        [DisplayName("المحور الصادي ")]
        public string Longitude { get; set; }
        [Required(ErrorMessage = "الرجاء تحديد الموقع على الخريطة")]
        [DisplayName("المحور السيني ")]
        public string Latitude { get; set; }
        public DateTime OnDate { get; set; }


        public DateTime up_to_date { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser Users { get; set; }
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
            throw new NotImplementedException();
        }

        public Task<Contract> GetOne(long id)
        {
            throw new NotImplementedException();
        }

        public Task<DbCRUD> InsertContract(Contract contract)
        {
            throw new NotImplementedException();
        }

        public Task<DbCRUD> UpdateContract(Contract contract)
        {
            throw new NotImplementedException();
        }
    }
}
