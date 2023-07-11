
using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyToDo.Api.Service
{
    /// <summary>
    /// 待办事项的实现
    /// </summary>
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public ToDoService(IUnitOfWork work,IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }
        public async Task<ApiResponse> AddAsync(ToDoDto model)
        {
            try
            {
                var todo = mapper.Map<ToDo>(model);
                await work.GetRepository<ToDo>().InsertAsync(todo);
                if(await work.SaveChangesAsync()>0)
                {
                    return new ApiResponse(true, model);
                }
                return new ApiResponse(false, "添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todo=await repository.GetFirstOrDefaultAsync(predicate:t=>t.Id==id);
                repository.Delete(todo);
                if (await work.SaveChangesAsync() > 0)
                { 
                    return new ApiResponse(true, "");
                }
                return new ApiResponse(false, "删除数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter parameter )
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todos = await repository.GetPagedListAsync(predicate: x => string.IsNullOrEmpty(parameter.Search) ? true : x.Title.Contains(parameter.Search),
                    pageIndex: parameter.PageIndex,
                    pageSize: parameter.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.CreateDate));
                return new ApiResponse(true, todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, ex.Message);
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate:t=>t.Id==id);
                return new ApiResponse(true, todo);
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(ToDoDto model)
        {
            try
            {
                var dbtodo = mapper.Map<ToDo>(model);
                var repository = work.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: t => t.Id == dbtodo.Id);
                if (todo != null)
                {
                    todo.Title = dbtodo.Title;
                    todo.Content = dbtodo.Content;
                    todo.Status = dbtodo.Status;
                    todo.UpdateDate = DateTime.Now;

                    repository.Update(todo);

                    if(await work.SaveChangesAsync() > 0)
                    {
                        return new ApiResponse(true, todo);
                    }
                }
                else
                {
                    return new ApiResponse(false, "需要更新数据不存在");
                }
                return new ApiResponse(false, "更新数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(false, ex.Message);
            }
        }
    }
}
