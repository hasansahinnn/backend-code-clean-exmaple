using Core;
using Core.ReturnModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Abstract
{
    public interface IRepository<TModel> : IDisposable
        where TModel : class, IModel, new()
    {
        public Task<IReturn<TModel>> GetAsync(Expression<Func<TModel, bool>> filter);
        public Task<IReturn<TModel>> GetByIdAsync(int Id);
        public Task<IReturn> DeleteAsync(TModel model);
        public Task<IReturn> UpdateAsync(TModel model);
        public Task<IReturn> AddAsync(TModel model);
        public Task<IReturn<TModel>> SaveAsync(TModel model);
        public Task<IReturn<IEnumerable<TModel>>> SaveAsync(IEnumerable<TModel> model);
    }
}
