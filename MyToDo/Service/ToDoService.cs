using MyToDo.Common.Models;
using MyToDo.Model;
using MyToDo.Model.Parameter;
using MyToDo.Singletons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class ToDoService:BaseService<ToDo>,IToDoService
    {
        private readonly IFreeSql freeSql = DataBaseConnect.GetFreeSqlInstance();
        public ToDoService():base()
        {
        }

        public async Task<ApiResponse<List<ToDo>>> GetAllFilterAsync(ToDoParameter parameter)
        {
            try
            {
                var select = DataBaseConnect.GetFreeSqlInstance().Select<ToDo>();
                var total = await select.CountAsync();
                var list = await select.Page(parameter.PageIndex, parameter.PageSize).ToListAsync();
                return new ApiResponse<List<ToDo>>()
                {
                    Status = true,
                    Message = "",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<ToDo>>() { Status = false, Message = ex.Message };
            }
        }

        public async Task<ApiResponse<ToDo>> GetFirstorDefaultAsync(int id)
        {
            try
            {
                var info = await DataBaseConnect.GetFreeSqlInstance()
                    .Select<ToDo>()
                    .Where(t => t.Id == id)
                    .OrderByDescending(t => t.CreateDate)
                    .ToOneAsync();
                if (info != null)
                {
                    return new ApiResponse<ToDo>
                    {
                        Status = true,
                        Message = "",
                        Result = info,
                    };
                }
                else
                {
                    return new ApiResponse<ToDo>()
                    {
                        Status = false,
                        Message = $"编号：{id}，未查询到数据!"
                    };
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ToDo>()
                {
                    Status = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<ApiResponse<ToDo>> DeleteAsync(int id)
        {
            try
            {
                var info = await GetFirstorDefaultAsync(id);
                if (info.Status)
                {
                    var result = DataBaseConnect.GetFreeSqlInstance().Delete<ToDo>(id).ExecuteAffrows();
                    if (result > 0)
                    {
                        return new ApiResponse<ToDo>()
                        {
                            Status = true,
                            Message = "",
                        };
                    }
                    else
                    {
                        return new ApiResponse<ToDo>()
                        {
                            Status = false,
                            Message = $"编号{id},数据未找到",
                        };
                    }
                }
                return new ApiResponse<ToDo>()
                {
                    Status = false,
                    Message = $"编号{id},数据未找到",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ToDo>() { Status = false, Message = ex.Message };
            }
        }

    }
}
