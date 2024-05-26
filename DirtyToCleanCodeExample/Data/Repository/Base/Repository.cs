using Core;
using Core.ReturnModel;
using Data.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace Data.Repository.Base
{
    abstract public class Repository<TModel, TContext> : IRepository<TModel> 
        where TModel : class, IModel, new()
        where TContext : DbContext, new()
    {
        internal readonly TContext context;
        public Repository(TContext context)
        {
            this.context = context;
        }

        public void Dispose() // performans etkisi ve işi garantiye almak için dispose
        {
            context.Database.CloseConnection();
            context.Dispose();
        }

        public IReturn<TNull> CheckIsNull<TNull>(TNull? result)
            => (result == null) ? new SuccessReturn<TNull>("Data is null") : new SuccessReturn<TNull>("Data is not null", result);

        public IReturn<IEnumerable<TNull>> CheckIsNull<TNull>(IEnumerable<TNull>? result)
            => (result.Count() == 0) ? new SuccessReturn<IEnumerable<TNull>>("Data is null") : new SuccessReturn<IEnumerable<TNull>>("Data is not null", result);

        public async Task<IReturn<TModel>> GetAsync(Expression<Func<TModel, bool>> filter)
        {
            var result = await context.Set<TModel>().AsNoTracking().TagWith("TModel tipli getasync işlemi").AsNoTracking().FirstOrDefaultAsync(filter);
            return CheckIsNull(result);
        }

        public async Task<IReturn<TModel>> GetByIdAsync(int Id)
        {
            var result = await context.Set<TModel>().AsNoTracking().TagWith("TModel tipli GetByIdAsync işlemi").AsNoTracking().FirstOrDefaultAsync(p => p.Id == Id);
            return CheckIsNull(result);
        }

        public async Task<IReturn> DeleteAsync(TModel model)
        {
            context.Set<TModel>().Remove(model);
            return await SaveAsync(model);
        }

        public async Task<IReturn> UpdateAsync(TModel model)
        {
            context.Set<TModel>().Update(model);
            return await SaveAsync(model);
        }

        public async Task<IReturn> AddAsync(TModel model)
        {
            await context.Set<TModel>().AddAsync(model);
            return await SaveAsync(model);
        }

        public async Task<IReturn> SaveAsync()
        {
            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
                    return new SuccessReturn();
                }
                return new ErrorReturn();
            }
            catch (Exception e)
            {
                return new ErrorReturn(e);
            }
        }

        public async Task<IReturn<TModel>> SaveAsync(TModel model)
        {
            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
                    return new SuccessReturn<TModel>(model);
                }
                return new ErrorReturn<TModel>(model);
            }
            catch (Exception e)
            {
                return new ErrorReturn<TModel>(model, e);
            }
        }

        public async Task<IReturn<IEnumerable<TModel>>> SaveAsync(IEnumerable<TModel> model)
        {
            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
                    return new SuccessReturn<IEnumerable<TModel>>(model);
                }
                return new ErrorReturn<IEnumerable<TModel>>(model);
            }
            catch (Exception e)
            {
                return new ErrorReturn<IEnumerable<TModel>>(model, e);
            }
        }
    }
}
