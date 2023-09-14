using MyToDo.Model.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface IBaseService<TEntity> where TEntity:class
    {
        ApiResponse<TEntity> Add(TEntity entity);
        Task<ApiResponse<TEntity>> UpdateAsync(TEntity entity);
        Task<ApiResponse<TEntity>> DeleteAsync(int id);
        Task<ApiResponse<TEntity>> GetFirstorDefaultAsync(int id);
        Task<ApiResponse<List<TEntity>>> GetAllAsync(QueryParameter query);
    }
}
