using api.Controllers.Dto;
using api.DAL.EfDbContext;
using api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace api.DAL
{
    public class StatusesRepository : IStatusesRepository
    {
        private readonly TasksDbContext db;

        public StatusesRepository(TasksDbContext db)
        {
            this.db = db;
        }

        public bool ExistsWithName(string statusName)
        {
            throw new NotImplementedException();
        }

        public Status FindById(int statusId)
        {
            throw new NotImplementedException();
        }

        public Status Insert(CreateStatus value)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<Status> List()
        {
            throw new NotImplementedException();
        }
    }
}
