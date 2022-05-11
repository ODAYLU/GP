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
    public class Contact
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone  { get; set; }
        [Required]
        public string Object  { get; set; }
        [Required]
        public string Description  { get; set; }

        public bool IsReply { get; set; }
    }

    public interface IContact
    {
        public Task<Contact> GetOne(long id);
        public IQueryable<Contact> GetAll(string search = "");
        public Task<DbCRUD> InsertContact(Contact contact);
        public Task<DbCRUD> UpdateContact(Contact contact);
        public Task<DbCRUD> DeleteContact(long id);
    }

    public class ContactManagments : IContact
    {
        private readonly ApplicationDbContext _context;
        public ContactManagments(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<DbCRUD> DeleteContact(long id)
        {

        Contact com = await GetOne(id);
            if (com != null)
            {
                try
                {
                    _context.TContacts.Remove(com);
                     _context.SaveChanges();

                    return DbCRUD.success;
                }
                catch (Exception)
                {
                    throw;
                }

            }
            return DbCRUD.fail;

        }

        public IQueryable<Contact> GetAll(string search = "")
        {
            IQueryable<Contact> lst = _context.TContacts.AsNoTracking()
                .Where(x => string.IsNullOrEmpty(search)? true:x.Name.Contains(search)||x.Email.Contains(search)).AsQueryable();
            return lst;
        }

        public async Task<Contact> GetOne(long id) => await _context
                .TContacts
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);

        public async Task<DbCRUD> InsertContact(Contact comment)
        {
              
            //Contact data = new Contact()
            //{
            //    Name = comment.Name.Trim(),
            //    Description = comment.Description.Trim(),
            //    Email = comment.Email.Trim(),
            //    Object = comment.Object.Trim(),
            //    Phone = comment.Phone.Trim()
               
            //};
            try
            {
                var cate = await GetOne(comment.Id);
                if (cate != null)
                    return DbCRUD.isExisted;
                _context.TContacts.Add(comment);
                _context.SaveChanges();
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

        public async Task<DbCRUD> UpdateContact(Contact comment)
        {
            try
            {
                Contact com = await GetOne(comment.Id);
                if (com == null)
                    return DbCRUD.isNotExisted;
                _context.TContacts.Update(comment);
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
