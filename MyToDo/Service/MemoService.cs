using MyToDo.Model;
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
    }
}
