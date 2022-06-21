using GP.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GP.Models.Enum;

namespace GP.Models
{
    public class InformationGen
    {

        public int Id { get; set; }
       
        [EmailAddress(ErrorMessage = "يرجى ادخال ايميل بشكل صحيح")]
        public string Email { get; set; }
    
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "يرجى ادخال الرقم بشكل صحيح")]
        public string Phone { get; set; }
        [Url(ErrorMessage = "يجب أن يكون رابط صحيح")]
        public string UrlFacebook { get; set; }
        [Url(ErrorMessage = "يجب أن يكون رابط صحيح")]
        public string UrlTwitter { get; set; }
        [Url(ErrorMessage = "يجب أن يكون رابط صحيح")]
        public string UrlInstegrame { get; set; }
    }

    public interface IInformationGen
    {
        public InformationGen GetOne();

        public Task<DbCRUD> UpdateInformationGen(InformationGen information);
        public Task<DbCRUD> InsertInformationGen(InformationGen information);

        public Task<InformationGen> GetOneById(int id);

    }
    public class InformationGenManage : IInformationGen
    {
        private readonly ApplicationDbContext _context;

        public InformationGenManage(ApplicationDbContext context)
        {
            _context = context;
        }

        public InformationGen GetOne()
        {
            try
            {
                return _context.TInformatiomGensT
                .AsNoTracking()
                .SingleOrDefault();
            }
            catch
            {
                return null;
            }

        }

        public async Task<InformationGen> GetOneById(int id) => await _context
                .TInformatiomGensT
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);

        public async Task<DbCRUD> InsertInformationGen(InformationGen information)
        {
           if(information is not null)
            {
                try
                {
                   await _context.TInformatiomGensT.AddAsync(information);
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    return DbCRUD.dbError;
                }
            }
            return DbCRUD.success;
        }

        public async Task<DbCRUD> UpdateInformationGen(InformationGen information)
        {
            try
            {
                InformationGen cty = await GetOneById(information.Id);
                if (cty == null)
                    return DbCRUD.isNotExisted;

                _context.TInformatiomGensT.Update(information);
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
