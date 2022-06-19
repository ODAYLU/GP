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
    public class State
    {
        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "الحقل مطلوب")]
        public string name { get; set; }
      
        public string ImagePath { get; set; }
    }
    public interface IState
    {
        public Task<State> GetOne(long? id);
        public IQueryable<State> GetAll(string search = "");
        public Task<DbCRUD> InsertState(State services);
        public Task<DbCRUD> UpdateState(State services);
        public Task<DbCRUD> DeleteState(long? id);
        public IQueryable<State> GetAllSelected();

    }
    public class StateManage : IState
    {
        private readonly ApplicationDbContext _context;

        public StateManage(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<State> GetAllSelected()
        {
            return _context.TState.AsNoTracking().AsQueryable();
        }
        public async Task<DbCRUD> DeleteState(long? id)
        {
            try
            {
                State state = await GetOne(id);
                if (state == null)
                    return DbCRUD.isNotExisted;
                _context.TState.Remove(state);
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

                 



        public IQueryable<State> GetAll(string search = "") => _context
                    .TState
                    .AsNoTracking().Where(x => string.IsNullOrEmpty(search) ? true :
                    (x.name.Contains(search))
                    )
                    .AsQueryable();

        public async Task<State> GetOne(long? id) => await _context
                .TState
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);

        public async Task<DbCRUD> InsertState(State state)
        {
            try
            {
                State stt = await GetOne(state.Id);
                if (stt != null)
                    return DbCRUD.isExisted;
                await _context.TState.AddAsync(state);
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

        public async Task<DbCRUD> UpdateState(State state)
        {
            try
            {
                State stt = await GetOne(state.Id);
                if (stt == null)
                    return DbCRUD.isNotExisted;
                if (!string.IsNullOrEmpty(state.ImagePath))
                {
                    stt.ImagePath = state.ImagePath;
                }
                stt.name = state.name;
                _context.TState.Update(stt);
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
