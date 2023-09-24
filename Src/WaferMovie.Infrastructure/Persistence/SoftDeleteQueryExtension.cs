using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;
using WaferMovie.Domain;

namespace WaferMovie.Infrastructure.Persistence;

public static class SoftDeleteQueryExtension
{
    public static void AddSoftDeleteQueryFilter(
        this IMutableEntityType entityData)
    {
        var methodToCall = typeof(SoftDeleteQueryExtension)
            .GetMethod(nameof(GetSoftDeleteFilter),
                BindingFlags.NonPublic | BindingFlags.Static)
            ?.MakeGenericMethod(entityData.ClrType);
        var filter = methodToCall?.Invoke(null, new object[] { });
        entityData.SetQueryFilter((LambdaExpression)filter!);
        entityData.AddIndex(entityData.
            FindProperty(nameof(IBaseSoftDeleteEntity.DeletedBy))!);
    }

    private static LambdaExpression GetSoftDeleteFilter<TEntity>() where TEntity : IBaseSoftDeleteEntity
    {
        Expression<Func<TEntity, bool>> filter = x => x.DeletedOn == null;
        return filter;
    }
}