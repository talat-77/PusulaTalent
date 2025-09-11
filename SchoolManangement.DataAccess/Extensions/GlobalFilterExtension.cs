
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SchoolManangement.Entity;

namespace SchoolManangement.DataAccess.Extensions
{
    public static class GlobalFilterExtension
    {
        public static void ApplyGlobalFilters<T>(this ModelBuilder modelBuilder, string propertyName, T value)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var foundProperty = entityType.FindProperty(propertyName);
                if (foundProperty != null && foundProperty.ClrType == typeof(T))
                {
                    var newParameter = Expression.Parameter(entityType.ClrType);
                    var filter = Expression.Lambda(Expression.Equal(Expression.Property(newParameter, propertyName),
                        Expression.Constant(value)), newParameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }
        }
    }

}
