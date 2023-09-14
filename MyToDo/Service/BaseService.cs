﻿using MyToDo.Model.Parameter;
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

        private Func<TEntity, int> getIdFunc;
        private Func<TEntity, DateTime> getDateFunc;
        public BaseService()
        {
            getIdFunc = CreateIdGetter();
            getDateFunc = CreateDateGetter();
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
                var info = DataBaseConnect.GetFreeSqlInstance()
                    .Insert<TEntity>(entity)
                    .ExecuteAffrows();
                if (info > 0)
                {
                    return new ApiResponse<TEntity>() {
                        Status = true,
                        Message = "",
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

        public async Task<ApiResponse<TEntity>> DeleteAsync(int id)
        {
            try
            {
                var info =await GetFirstorDefaultAsync(id);
                if (info.Status)
                {
                    var result = DataBaseConnect.GetFreeSqlInstance().Delete<TEntity>(info.Result);

                }
                return new ApiResponse<TEntity>() {
                    Status = false,
                    Message=$"编号{id},数据未找到",
                };
            }
            catch(Exception ex)
            {
                return new ApiResponse<TEntity>() { Status = false, Message = ex.Message };
            }
        }

        public async Task<ApiResponse<List<TEntity>>> GetAllAsync(QueryParameter query)
        {
            try
            {
                var select = DataBaseConnect.GetFreeSqlInstance().Select<TEntity>();
                var total = await select.CountAsync();
                var list = await select.Page(query.PageIndex, query.PageSize).ToListAsync();
                return new ApiResponse<List<TEntity>>() {
                    Status = true,
                    Message = "",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<TEntity>>() {Status=false,Message=ex.Message };
            }
        }

        public async Task<ApiResponse<TEntity>> GetFirstorDefaultAsync(int id)
        {
            try
            {
                var info = await DataBaseConnect.GetFreeSqlInstance()
                    .Select<TEntity>()
                    .Where(t => getIdFunc(t) == id)
                    .OrderByDescending(t => getDateFunc(t))
                    .ToOneAsync();
                if (info != null)
                {
                    return new ApiResponse<TEntity> {
                        Status = true,
                        Message = "",
                        Result = info,
                    };
                }
                else
                {
                    return new ApiResponse<TEntity>()
                    {
                        Status = false,
                        Message = $"编号：{id}，未查询到数据!"
                    };
                };
            }
            catch(Exception ex)
            {
                return new ApiResponse<TEntity>() { 
                    Status=false,
                    Message=ex.Message,
                };
            }
        }

        public async Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity)
        {
            try
            {
                var info = await DataBaseConnect.GetFreeSqlInstance().Update<TEntity>(entity).ExecuteAffrowsAsync();
                if (info > 0)
                {
                    return new ApiResponse<TEntity>()
                    {
                        Status = true,
                        Message = "",
                        Result = entity,
                    };
                }
                return new ApiResponse<TEntity>()
                {
                    Status = false,
                    Message = "更新失败！",
                };
            }
            catch(Exception ex)
            {
                return new ApiResponse<TEntity>() {Status=false,Message=ex.Message };
            }
        }
    }
}
