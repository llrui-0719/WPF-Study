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
                var select = freeSql.Select<ToDo>().Where(a => (string.IsNullOrEmpty(parameter.Search) ? a.Id>=0 : a.Title.Contains(parameter.Search)) && ((parameter.Status!=null)? a.Status == parameter.Status:a.Id>=0));
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

        public async Task<ApiResponse<ToDo>> UpdateAsync(ToDo entity)
        {
            try
            {
                var info = await DataBaseConnect.GetFreeSqlInstance().Update<ToDo>(entity.Id)
                   .Set(a => a.UpdateDate, entity.UpdateDate)
                   .Set(a => a.Title, entity.Title)
                   .Set(a => a.Content, entity.Content)
                   .Set(a => a.Status, entity.Status)
                   .ExecuteAffrowsAsync();
                if (info > 0)
                {
                    return new ApiResponse<ToDo>()
                    {
                        Status = true,
                        Message = "",
                        Result = entity,
                    };
                }
                return new ApiResponse<ToDo>()
                {
                    Status = false,
                    Message = "更新失败！",
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ToDo>() { Status = false, Message = ex.Message };
            }
        }

        public ApiResponse<SummaryDto> SummaryAsync()
        {
            var todods = GetAllFilterAsync(new ToDoParameter() {
                PageIndex = 0,
                PageSize = 100,
            }).Result.Result;
            var memos = new MemoService().GetAllAsync(new QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
            }).Result.Result;
            var summary = new SummaryDto();
            summary.Sum = todods.Count;
            summary.CompletedCount = todods.Where(x => x.Status == 1).Count();
            summary.CompletedRadio = ((summary.CompletedCount / summary.Sum) * 100.0).ToString();
            summary.MemoCount = memos.Count;
            summary.ToDoList = new ObservableCollection<ToDo>();
            summary.ToDoList.AddRange(todods.Where(x => x.Status == 0).ToList());
            summary.MemoList = new ObservableCollection<Memo>();
            summary.MemoList.AddRange(memos);
            return new ApiResponse<SummaryDto>() { Status = true, Message = "",Result=summary };
        }
    }
}
