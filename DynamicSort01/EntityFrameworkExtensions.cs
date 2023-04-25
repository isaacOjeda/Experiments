using System.Linq.Expressions;
using System.Reflection;

namespace DynamicSort01;


public static class EntityFrameworkExtensions
{
    public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByStrValues) where TEntity : class
    {
        var queryExpr = source.Expression;        
        var command = orderByStrValues.ToUpper().EndsWith("DESC") ? "OrderByDescending" : "OrderBy";
        var propertyName = orderByStrValues.Split(' ')[0].Trim();

        var type = typeof(TEntity);
        var property = SearchProperty(type, propertyName);

        if (property == null)
            return source; // ignoramos el sort

        var parameter = Expression.Parameter(type, "p");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        queryExpr = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, queryExpr, Expression.Quote(orderByExpression));

        return source.Provider.CreateQuery<TEntity>(queryExpr); ;
    }


    private static PropertyInfo? SearchProperty(Type type, string propertyName)
    {
        foreach (var item in type.GetProperties())
            if (item.Name.ToLower() == propertyName.ToLower())
                return item;
        return null;
    }

}