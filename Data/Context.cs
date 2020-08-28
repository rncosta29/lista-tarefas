using lista_tarefas_api.Mapeamento;
using lista_tarefas_api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lista_tarefas_api.Data
{
    public class Context : DbContext
    {
        public DbSet<TaskModel> Tasks { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new TaskMap());
        }
    }
}
