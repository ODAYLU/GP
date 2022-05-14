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
    public class Type
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string type { get; set; }
        public string ImagePath { get; set; }


    }
    public interface IType
    {
        public IQueryable<Type> GetAllSelected();
        public Task<Type> GetOne(int id);
        public IQueryable<Type> GetAll(string search ="");
        public Task<DbCRUD> InsertType(Type type);
        public Task<DbCRUD> UpdateType(Type type);
        public Task<DbCRUD> DeleteType(int id);
    }
    public class TypeManage : IType
    {
        private readonly ApplicationDbContext _context;

        public TypeManage(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Type> GetAllSelected()
        {
            return _context.Ttype.AsNoTracking().AsQueryable();
        }
        public async Task<DbCRUD> DeleteType(int id)
        {
            try
            {
                Type type = await GetOne(id);
                if (type == null)
                    return DbCRUD.isNotExisted;
                _context.Ttype.Remove(type);
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

        public IQueryable<Type> GetAll(string search) => _context
                    .Ttype
                    .AsNoTracking().Where(x =>
                    string.IsNullOrEmpty(search) ? true :
                    (x.type.Contains(search))
                    ).AsQueryable();

        public async Task<Type> GetOne(int id) => await _context
                .Ttype
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);

        public async Task<DbCRUD> InsertType(Type type)
        {
            try
            {
                Type ty = await GetOne(type.Id);
                if (ty != null)
                    return DbCRUD.isExisted;
                await _context.Ttype.AddAsync(type);
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

        public async Task<DbCRUD> UpdateType(Type type)
        {
            try
            {
                Type ty = await GetOne(type.Id);
                if (ty == null)
                    return DbCRUD.isNotExisted;
                _context.Ttype.Update(type);
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
