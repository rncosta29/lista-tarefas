using lista_tarefas_api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lista_tarefas_api.Data
{
    public class Repository : IRepository
    {
        private readonly Context _context;

        public Repository(Context context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<TaskModel[]> GetAllByPeriod(string macAddress)
        {
            IQueryable<TaskModel> query = _context.Tasks;

            query = query.AsNoTracking()
                .OrderBy(task => task.When)
                .Where(t => t.MacAddress == macAddress);

            return await query.ToArrayAsync();
        }

        public async Task<TaskModel> GetTaskById(string id)
        {
            IQueryable<TaskModel> query = _context.Tasks;

            query = query.AsNoTracking()
                .OrderBy(task => task.Id)
                .Where(task => task.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
    }
}
