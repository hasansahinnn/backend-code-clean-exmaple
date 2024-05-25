using Core;
using Data.Repository.Abstract;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Service<TEntity> : IService
        where TEntity : class, IModel, new()
    {
        internal readonly IRepository<TEntity> repository;
        internal readonly IHttpContextAccessor httpContextAccessor;
        public Service(IRepository<TEntity> repository, IHttpContextAccessor httpContextAccessor) 
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
        }
    }
}
