using MyToDo.Model;
using MyToDo.Model.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface IMemoService:IBaseService<Memo>
    {
        Task<ApiResponse<Memo>> GetFirstorDefaultAsync(int id);

        Task<ApiResponse<Memo>> DeleteAsync(int id);

        Task<ApiResponse<Memo>> UpdateAsync(Memo entity);

        Task<ApiResponse<List<Memo>>> GetAllAsync(QueryParameter query);
    }
}
