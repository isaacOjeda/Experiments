using System.Linq.Expressions;
using System.Reflection;

namespace DynamicSort01;


public static class EntityFrameworkExtensions
{
    public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByStrValues) where TEntity : class
    {
        var queryExpr = source.Expression;
        var methodAsc = "OrderBy";
        var methodDesc = "OrderByDescending";

        var orderByValues = orderByStrValues.Trim().Split(',').Select(x => x.Trim()).ToList();

        foreach (var orderPairCommand in orderByValues)
        {
            var command = orderPairCommand.ToUpper().EndsWith("DESC") ? methodDesc : methodAsc;

            //Get propertyname and remove optional ASC or DESC
            var propertyName = orderPairCommand.Split(' ')[0].Trim();

            var type = typeof(TEntity);
            var parameter = Expression.Parameter(type, "p");

            PropertyInfo? property = SearchProperty(type, propertyName);

            if (property == null)
                continue;

            MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            queryExpr = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType }, queryExpr, Expression.Quote(orderByExpression));

            methodAsc = "ThenBy";
            methodDesc = "ThenByDescending";
        }

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