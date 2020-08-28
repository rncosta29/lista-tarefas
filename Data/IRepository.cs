using lista_tarefas_api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lista_tarefas_api.Data
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        Task<TaskModel[]> GetAllByPeriod(string macAddress);
        Task<TaskModel> GetTaskById(string id);
    }
}
