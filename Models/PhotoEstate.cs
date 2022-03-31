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
    public class PhotoEstate
    {
        [Key]
        public long Id { get; set; }
        public string ImagePath { get; set; }
        public long IdEstate { get; set; }
        [ForeignKey("IdEstate")]
        public Estate estate { get; set; }
    }

    public interface IPhotoEstate
    {
        public List<PhotoEstate> GetAllByEstate(long id);
        public Task<PhotoEstate> GetOne(long id);
        public List<PhotoEstate> GetAll();
        public Task<DbCRUD> InsertPhotoEstate(PhotoEstate photoEstate);
        public Task<DbCRUD> UpdatePhotoEstate(PhotoEstate photoEstate);
        public Task<DbCRUD> DeletePhotoEstate(long id);
    }
    public class PhotoEstateManage : IPhotoEstate
    {
        private readonly ApplicationDbContext _context;

        public PhotoEstateManage(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<DbCRUD> DeletePhotoEstate(long id)
        {
            try
            {
                PhotoEstate photoEstate = await GetOne(id);
                if (photoEstate == null)
                    return DbCRUD.isNotExisted;
                _context.TPhotoEstate.Remove(photoEstate);
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
        public List<PhotoEstate> GetAllByEstate(long id) => _context
                   .TPhotoEstate
                   .AsNoTracking().Where(x=>x.IdEstate==id)
                   .ToList();
        public List<PhotoEstate> GetAll() => _context
                    .TPhotoEstate
                    .AsNoTracking()
                    .ToList();

        public async Task<PhotoEstate> GetOne(long id) => await _context
                .TPhotoEstate
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);

        public async Task<DbCRUD> InsertPhotoEstate(PhotoEstate photoEstate)
        {
            try
            {
                PhotoEstate photo = await GetOne(photoEstate.Id);
                if (photo != null)
                    return DbCRUD.isExisted;
                await _context.TPhotoEstate.AddAsync(photoEstate);
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

        public async Task<DbCRUD> UpdatePhotoEstate(PhotoEstate photoEstate)
        {
            try
            {
                PhotoEstate photo = await GetOne(photoEstate.Id);
                if (photo == null)
                    return DbCRUD.isNotExisted;
                _context.TPhotoEstate.Update(photoEstate);
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
