using GP.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static GP.Models.Enum;

namespace GP.Models
{
    public class Currency
    {
        [Key]
        public int Id { set; get; }
        [Required]
        public string currency { get; set;}

    }
    public interface ICurrency
    {
        public IQueryable<Currency> GetAllSelected();
        public Task<Currency> GetOne(long id);
        public IQueryable<Currency> GetAll(string search = "");
        public Task<DbCRUD> InsertCurrency(Currency curre);
        public Task<DbCRUD> UpdateCurrency(Currency city);
        public Task<DbCRUD> DeleteCurrency(long id);
    }
    public class CurrencyManage : ICurrency
    {
        private readonly ApplicationDbContext _context;

        public CurrencyManage(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Currency> GetAllSelected()
        {
            return _context.TCurrency.AsNoTracking().AsQueryable();
        }
        public async Task<DbCRUD> DeleteCurrency(long id)
        {
            try
            {
                Currency currency = await GetOne(id);
                if (currency == null)
                    return DbCRUD.isNotExisted;
                _context.TCurrency.Remove(currency);
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

        public IQueryable<Currency> GetAll(string search = "") => _context
                    .TCurrency
                    .AsNoTracking().Where(x => string.IsNullOrEmpty(search) ? true :
                       (x.currency.Contains(search))
                    )
                    .AsQueryable();

        public async Task<Currency> GetOne(long id) => await _context
                .TCurrency
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);

        public async Task<DbCRUD> InsertCurrency(Currency currency)
        {
            try
            {
                Currency cu = await GetOne(currency.Id);
                if (cu != null)
                    return DbCRUD.isExisted;
                await _context.TCurrency.AddAsync(currency);
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

        public async Task<DbCRUD> UpdateCurrency(Currency currency)
        {
            try
            {
                Currency cr = await GetOne(currency.Id);
                if (cr == null)
                    return DbCRUD.isNotExisted;
                _context.TCurrency.Update(currency);
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
