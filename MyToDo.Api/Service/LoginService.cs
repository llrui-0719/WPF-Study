using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyToDo.Api.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public LoginService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }
        public async Task<ApiResponse> LoginAsync(string Account, string Password)
        {
            try
            {
                var model=await work.GetRepository<User>().GetFirstOrDefaultAsync(predicate: t => 
                t.Account.Equals(Account)
                &&
                t.PassWord.Equals(Password));
                if (model != null)
                {
                    return new ApiResponse(true, model);
                }
                else
                {
                    return new ApiResponse(false, "账号或密码有误,请重试");
                }
            }
            catch(Exception ex)
            {
                return new ApiResponse(false, ex.Message);
            }
        }

        public async Task<ApiResponse> Resgiter(UserDto user)
        {
            try
            {
                var model=mapper.Map<User>(user);
                var repository =work.GetRepository<User>();
                var olduer=await repository.GetFirstOrDefaultAsync(predicate: x => x.Account.Equals(model.Account));
                if (olduer != null)
                {
                    return new ApiResponse(false, $"该账号{model.Account}用户已存在!");
                }
                else
                {
                    model.CreateDate = DateTime.Now;
                    await repository.InsertAsync(model);
                    if (await work.SaveChangesAsync() > 0)
                    {
                        return new ApiResponse(true, model);
                    }
                    else
                    {
                        return new ApiResponse(false, "注册失败");
                    }
                }
            }
            catch(Exception ex)
            {
                return new ApiResponse(false, ex.Message);
            }
        }
    }
}
