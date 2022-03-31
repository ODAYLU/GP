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
    public class City
    {

        [Key]
        public long Id { get; set; }
        [Required]
        public string name { get; set; }
    }
    public interface ICity
    {
        public IQueryable<City> GetAllSelected();
        public Task<City> GetOne(long id);
        public IQueryable<City> GetAll(string search = "");
        public Task<DbCRUD> InsertCity(City city);
        public Task<DbCRUD> UpdateCity(City city);
        public Task<DbCRUD> DeleteCity(long id);
    }
    public class CityManage : ICity
    {
        private readonly ApplicationDbContext _context;

        public CityManage(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<City> GetAllSelected()
        {
            return _context.TCity.AsNoTracking().AsQueryable();
        }
        public async Task<DbCRUD> DeleteCity(long id)
        {
            try
            {
                City city = await GetOne(id);
                if (city == null)
                    return DbCRUD.isNotExisted;
                _context.TCity.Remove(city);
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

        public IQueryable<City> GetAll(string search = "") => _context
                    .TCity
                    .AsNoTracking().Where(x => string.IsNullOrEmpty(search)? true:
                       (x.name.Contains(search))                  
                    )
                    .AsQueryable();

        public async Task<City> GetOne(long id) => await _context
                .TCity
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);

        public async Task<DbCRUD> InsertCity(City city)
        {
            try
            {
                City ct = await GetOne(city.Id);
                if (ct != null)
                    return DbCRUD.isExisted;
                await _context.TCity.AddAsync(city);
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

        public async Task<DbCRUD> UpdateCity(City city)
        {
            try
            {
                City cty = await GetOne(city.Id);
                if (cty == null)
                    return DbCRUD.isNotExisted;
                cty.name = city.name;
                _context.TCity.Update(cty);
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
