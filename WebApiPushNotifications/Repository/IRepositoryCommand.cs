using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApiPushNotifications.Repository
{
    public partial interface IRepositoryCommand<T>
    {
        Task<List<T>> Read();
        //Task<List<T>> ReadToFilter(Expression<Func<T, IProperty>> result);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);

    }

    //public interface IRepositoryCommandHandler<T> where T : IRepositoryCommand<T>
    //{
    //    void Handle(T command);
    //}    
}
