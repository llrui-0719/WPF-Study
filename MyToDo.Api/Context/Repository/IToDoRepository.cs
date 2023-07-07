using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyToDo.Api.Context.Repository
{
    public interface IToDoRepository
    {
        Task<bool> Add(ToDo toDo);
        Task<bool> Update(ToDo toDo);

        Task<bool> Delete(int id);

    }

    public class ToDoRepository : IToDoRepository
    {
        private MyToDoContext doContext;
        public ToDoRepository(MyToDoContext doContext)
        {
            this.doContext = doContext;
        }
        public async Task<bool> Add(ToDo toDo)
        {
            await doContext.ToDo.AddAsync(toDo);
            return await doContext.SaveChangesAsync()>0;
        }

        Task<bool> IToDoRepository.Delete(int id)
        {
            throw new NotImplementedException();
        }

        Task<bool> IToDoRepository.Update(ToDo toDo)
        {
            throw new NotImplementedException();
        }
    }
}
