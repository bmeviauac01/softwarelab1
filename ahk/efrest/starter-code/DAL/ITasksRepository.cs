using api.Controllers.Dto;
using api.Model;
using System.Collections.Generic;

namespace api.DAL
{
    // DO NOT CHANGE ANYTHING
    // NE VALTOZTASS MEG SEMMIT
    public interface ITasksRepository
    {
        IReadOnlyCollection<Task> List();
        Task FindById(int taskId);
        Task Insert(CreateTask value);
        Task Delete(int taskId);

        Task MarkDone(int taskId);
        Task MoveToStatus(int taskId, string newStatusName);
    }
}