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

    }
}
