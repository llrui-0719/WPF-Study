using MyToDo.Model;
using MyToDo.Model.Parameter;
using MyToDo.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class MemoService : BaseService<Memo>, IMemoService
    {
        private readonly IFreeSql freeSql = DataBaseConnect.GetFreeSqlInstance();
        public MemoService():base()
        {

        }

        public async Task<ApiResponse<Memo>> GetFirstorDefaultAsync(int id)
        {
            try
            {
                var info = await DataBaseConnect.GetFreeSqlInstance()
                    .Select<Memo>()
                    .Where(t => t.Id == id)
                    .OrderByDescending(t => t.CreateDate)
                    .ToOneAsync();
                if (info != null)
                {
                    return new ApiResponse<Memo>
                    {
                        Status = true,
                        Message = "",
                        Result = info,
                    };
                }
                else
                {
                    return new ApiResponse<Memo>()
                    {
                        Status = false,
                        Message = $"编号：{id}，未查询到数据!"
                    };
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Memo>()
                {
                    Status = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<ApiResponse<Memo>> DeleteAsync(int id)
        {
            try
            {
                var info = await GetFirstorDefaultAsync(id);
                if (info.Status)
                {
                    var result = DataBaseConnect.GetFreeSqlInstance().Delete<ToDo>(id).ExecuteAffrows();
                    if (result > 0)
                    {
                        return new ApiResponse<Memo>()
                        {
                            Status = true,
                            Message = "",
                        };
                    }
                    else
                    {
                        return new ApiResponse<Memo>()
                        {
                            Status = false,
                            Message = $"编号{id},数据未找到",
                        };
                    }
                }
                return new ApiResponse<Memo>()
                {
                    Status = false,
                    Message = $"编号{id},数据未找到",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Memo>() { Status = false, Message = ex.Message };
            }
        }

        public async Task<ApiResponse<Memo>> UpdateAsync(Memo entity)
        {
            try
            {
                var info = await DataBaseConnect.GetFreeSqlInstance().Update<Memo>(entity.Id)
                    .Set(a =>a.UpdateDate, entity.UpdateDate)
                    .Set(a =>a.Title, entity.Title)
                    .Set(a =>a.Content, entity.Content)
                    .ExecuteAffrowsAsync();
                if (info > 0)
                {
                    return new ApiResponse<Memo>()
                    {
                        Status = true,
                        Message = "",
                        Result = entity,
                    };
                }
                return new ApiResponse<Memo>()
                {
                    Status = false,
                    Message = "更新失败！",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Memo>() { Status = false, Message = ex.Message };
            }
        }

        public async Task<ApiResponse<List<Memo>>> GetAllAsync(QueryParameter parameter)
        {
            try
            {
                var select = freeSql.Select<Memo>().Where(a => string.IsNullOrEmpty(parameter.Search) ? a.Id >= 0 : a.Title.Contains(parameter.Search));
                var total = await select.CountAsync();
                var list = await select.Page(parameter.PageIndex, parameter.PageSize).ToListAsync();
                return new ApiResponse<List<Memo>>()
                {
                    Status = true,
                    Message = "",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Memo>>() { Status = false, Message = ex.Message };
            }
        }
    }
}
