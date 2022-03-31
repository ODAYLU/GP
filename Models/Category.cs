using GP.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static GP.Models.Enum;

namespace GP.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string category { get; set; }
       
        public string ImagePath { set; get; }
        
    }
    public interface ICategory
    {
  
        public Task<Category> GetOne(int id);
        public  IQueryable<Category>  GetAll(string search);
        public IQueryable<Category> GetAllSelected();
        public Task<DbCRUD> InsertCategory(Category category);
        public Task<DbCRUD> UpdateCategory(Category Category);
        public Task<DbCRUD> DeleteCategory(int id);
    }
    public class CategoryManage : ICategory
    {
        private readonly ApplicationDbContext _context;

        public CategoryManage(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Category> GetAllSelected()
        {

            IQueryable<Category> category = _context.TCategory.AsNoTracking().AsQueryable();
            return category;

        }
        public async Task<DbCRUD> DeleteCategory(int id)
        {
            try
            {
                Category category = await GetOne(id);
                if (category == null)
                    return DbCRUD.isNotExisted;
                _context.TCategory.Remove(category);
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

        public IQueryable<Category> GetAll(string search = "") => _context
                    .TCategory

                    .AsNoTracking().Where(x =>
                    string.IsNullOrEmpty(search) ? true :
                    (x.category.Contains(search))
                    )
                    .AsQueryable();

        public async Task<Category> GetOne(int id) => await _context
                .TCategory
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);

        public async Task<DbCRUD> InsertCategory(Category category)
        {
            try
            {
                Category cate = await GetOne(category.Id);
                if (cate != null)
                    return DbCRUD.isExisted;
                await _context.TCategory.AddAsync(category);
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

        public async Task<DbCRUD> UpdateCategory(Category category)
        {
            try
            {
                Category cate = await GetOne(category.Id);
                if (cate == null)
                    return DbCRUD.isNotExisted;
                _context.TCategory.Update(category);
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
