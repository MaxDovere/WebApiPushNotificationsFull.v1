using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using WebApiPushNotifications.Store.SQLServer;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApiPushNotifications.Repository
{
    public partial class RepositoryGeneric<T> : IRepositoryCommand<T> where T : class
    {
        private readonly ILogger<RepositoryGeneric<T>> _logger;
        private DbContext _context;
        protected DbSet<T> _objectSet;

        public RepositoryGeneric(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this._context = context;
            this._objectSet = context.Set<T>();
        }
        public RepositoryGeneric(DbContext context, ILogger<RepositoryGeneric<T>> logger)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this._logger = logger;
            this._context = context;
            this._objectSet = context.Set<T>();
        }

        public DbContext Context 
        {
            get 
            {
                return this._context;
            }
        }

        public virtual async Task Delete(T entity)
        {

            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            this._objectSet.Remove(entity).State = EntityState.Deleted;
            await this._context.SaveChangesAsync();
        }

        public virtual async Task Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await this._objectSet.AddAsync(entity).ConfigureAwait(false);
            await this._context.SaveChangesAsync();

        }

        public virtual async Task<List<T>> Read()
        {
            return await this._objectSet.ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            this._objectSet.Update(entity).State = EntityState.Modified;
            await this._context.SaveChangesAsync();
        }        
    }
}
