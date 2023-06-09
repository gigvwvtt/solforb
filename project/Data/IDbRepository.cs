﻿using System.Linq.Expressions;

namespace project.Data;

public interface IDbRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    IEnumerable<T> GetAllWithInclude(params Expression<Func<T, object>>[] includeProperties);
    Task<T?> GetById(object id);
    IEnumerable<T> GetWithFilter(params Func<T, bool>[] predicate);
    IEnumerable<T> GetWithFilterWithInclude(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties);
    Task<IEnumerable<TT>> GetDistinctTs<TT>(Expression<Func<T, TT>> select);
    bool Add(T obj);
    bool Update(T obj);
    bool Delete(T obj);
    bool Save();
}