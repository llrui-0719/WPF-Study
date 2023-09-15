using MyToDo.Model.Parameter;
using MyToDo.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {

        public BaseService()
        {

        }

        private Func<TEntity, int> CreateIdGetter()
        {
            var entityParam = Expression.Parameter(typeof(TEntity), "entity");
            var propertyAccess = Expression.Property(entityParam, "Id");
            var lambda = Expression.Lambda<Func<TEntity, int>>(propertyAccess, entityParam);
            return lambda.Compile();
        }
        private Func<TEntity, DateTime> CreateDateGetter()
        {
            var entityParam = Expression.Parameter(typeof(TEntity), "entity");
            var propertyAccess = Expression.Property(entityParam, "UpdateDate");
            var lambda = Expression.Lambda<Func<TEntity, DateTime>>(propertyAccess, entityParam);
            return lambda.Compile();
        }



        public string ServiceName { get; }

        public ApiResponse<TEntity> Add(TEntity entity)
        {
            try
            {
                var id = DataBaseConnect.GetFreeSqlInstance()
                    .Insert<TEntity>(entity)
                    .ExecuteIdentity();
                if (id >= 0)
                {
                    return new ApiResponse<TEntity>() {
                        Status = true,
                        Message = id.ToString(),
                        Result = entity,
                    };
                }
                return new ApiResponse<TEntity>() {
                    Status = false,
                    Message = "新增数据失败！",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TEntity>()
                {
                    Status = true,
                    Message = ex.Message,
                };
            }
        }

    }
}
