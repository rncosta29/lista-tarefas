using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lista_tarefas_api.Model
{
    public class TaskModel
    {
        public string Id { get; set; }
        public string MacAddress { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime When { get; set; }
        public bool Done { get; set; }
        public DateTime Created { get; set; }

        public TaskModel(string id, string macAddress, int type, string title, string description, DateTime when)
        {
            Id = id;
            MacAddress = macAddress;
            Type = type;
            Title = title;
            Description = description;
            When = when;
            Done = false;
        }

        public TaskModel() { }
    }
}
