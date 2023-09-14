using MyToDo.Common.Models;
using MyToDo.Model;
using MyToDo.Model.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface IToDoService:IBaseService<ToDo>
    {
        Task<ApiResponse<List<ToDo>>> GetAllFilterAsync(ToDoParameter parameter);

    }
}
