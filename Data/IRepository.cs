using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace L6.Data.Infrastructure
{
public interface IRepository<T> : IUnitOfWorkProvider where T : class
{
    void Delete(System.Linq.Expressions.Expression<Func<T, bool>> where);
    void Delete(object id);
    void Delete(T entityToDelete);
    System.Collections.Generic.IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<System.Linq.IQueryable<T>, System.Linq.IOrderedQueryable<T>> orderBy = null, string includeProperties = "");
    T Get(System.Linq.Expressions.Expression<Func<T, bool>> where);
    System.Collections.Generic.IEnumerable<T> GetAll();
    T GetById(long id);
    T GetById(object id);
    T GetById(string id);
    System.Collections.Generic.IEnumerable<T> GetMany(System.Linq.Expressions.Expression<Func<T, bool>> where);
    System.Collections.Generic.IEnumerable<T> GetWithRawSql(string query, params object[] parameters);
    void Insert(T entity);
    void Update(T entity); 


}
}
