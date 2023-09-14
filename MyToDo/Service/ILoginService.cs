using MyToDo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface ILoginService
    {
        Task<ApiResponse<User>> LoginAsync(User user);

        ApiResponse Register(User user);
    }
}
