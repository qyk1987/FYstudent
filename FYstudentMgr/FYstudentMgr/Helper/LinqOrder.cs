using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace FYstudentMgr.Helper
{
    public static class LinqOrder
    {
        public static IQueryable<T> DataSort<T>(IQueryable<T> source, string sortExpression, string sortDirection)
        {
            string sortingDir = string.Empty;
            if (sortDirection.ToUpper().Trim() == "ASC")
                sortingDir = "OrderBy";
            else if (sortDirection.ToUpper().Trim() == "DESC")
                sortingDir = "OrderByDescending";
            ParameterExpression param = Expression.Parameter(typeof(T), sortExpression);
            PropertyInfo pi = typeof(T).GetProperty(sortExpression);
            Type[] types = new Type[2];
            types[0] = typeof(T);
            types[1] = pi.PropertyType;
            Expression expr = Expression.Call(typeof(Queryable), sortingDir, types, source.Expression, Expression.Lambda(Expression.Property(param, sortExpression), param));
            IQueryable<T> query = source.AsQueryable().Provider.CreateQuery<T>(expr);
            return query;
        }
        public static IQueryable<T> DataPaging<T>(IQueryable<T> source, int pageNumber, int pageSize)
        {
            return source.Skip(pageNumber * pageSize).Take(pageSize);
        }
        public static IQueryable<T> Sorting<T>(IQueryable<T> source, string sortExpression, string sortDirection, int pageNumber, int pageSize)
        {
            IQueryable<T> query = DataSort<T>(source, sortExpression, sortDirection);
            return DataPaging(query, pageNumber, pageSize);
        }
    }
}