using MyToDo.Extensions;
using MyToDo.Model;
using MyToDo.Singletons;
using System;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class LoginService : ILoginService
    {

        private readonly IFreeSql freeSql = DataBaseConnect.GetFreeSqlInstance();
        public LoginService()
        {

        }

        public async Task<ApiResponse<User>> LoginAsync(User user)
        {
            var password = StringExtensions.GetMD5(user.PassWord);
            var info = await freeSql.Select<User>().Where(x => x.Account.Equals(user.Account) && x.PassWord.Equals(password)).ToOneAsync();
            if (info != null)
            {
                return new ApiResponse<User>() {
                    Status = true,
                    Result=user,
                };
            }
            else
            {
                return new ApiResponse<User>() {
                    Status = false,
                    Message="账号或密码错误",
                };
            }
        }

        public ApiResponse Register(User user)
        {
            try
            {
                var password = StringExtensions.GetMD5(user.PassWord);
                user.PassWord = password;
                var info = freeSql.Insert<User>(user).ExecuteAffrows();
                if (info > 0)
                {
                    return new ApiResponse()
                    {
                        Status = true,
                        Message = "",
                    };
                }
                return new ApiResponse() {
                    Status=false,
                    Message="数据入库失败！",
                };
            }
            catch(Exception ex)
            {
                return new ApiResponse()
                {
                    Status = false,
                    Message = ex.Message,
                };
            }
           
        }
    }
}
