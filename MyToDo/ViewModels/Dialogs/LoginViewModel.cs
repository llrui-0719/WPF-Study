using MyToDo.Common;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels.Dialogs
{
    public class LoginViewModel : BindableBase, IDialogAware
    {

        private readonly ILoginService service;

        private readonly IEventAggregator aggregator;
        public LoginViewModel(ILoginService service, IEventAggregator aggregator)
        {
            UserDto = new RegisterDto();
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.service = service;
            this.aggregator = aggregator;
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "Login": Login();break;
                case "LoginOut":LoginOut();break;
                case "Go":SelectedIndex=1;break;//跳转注册页面
                case "Return":SelectedIndex = 0;break;//返回登陆页面
                case "Register": Register(); break;//注册页面注册
            }
        }

        private async void Register()
        {

            if (string.IsNullOrWhiteSpace(UserDto.Account) || string.IsNullOrWhiteSpace(UserDto.UserName) || string.IsNullOrWhiteSpace(UserDto.PassWord))
                return;
            if (!UserDto.PassWord.Equals(UserDto.NewPassWord))
            {
                //验证失败提示...
                aggregator.SendMessage("两次密码输入不一致！", "Login");
                return;
            }
            var result=await service.RegisterAsync(new Shared.Dtos.UserDto() {
                Account= UserDto.Account,
                UserName=UserDto.UserName,
                PassWord=UserDto.PassWord,
            });

            if(result!=null && result.Status)
            {
                //注册成功
                aggregator.SendMessage("注册成功", "Login");
                SelectedIndex = 0;
                return;
            }
            //注册失败提示。。。
            aggregator.SendMessage(result.Result.ToString(), "Login");

        }

        private void LoginOut()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        /// <summary>
        /// 登陆
        /// </summary>
        private async void Login()
        {
            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(passWord)) return;

            var loginresult =await service.LoginAsync(new Shared.Dtos.UserDto() {
                Account=Account,
                PassWord=PassWord,
            });

            if (loginresult!=null &&  loginresult.Status)
            {
                AppSession.UserName = loginresult.Result.UserName;
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                return;
            }

            //登陆失败提示...
            aggregator.SendMessage(loginresult.Message, "Login");

        }

        public string Title { get; set; } = "ToDo";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            LoginOut();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public DelegateCommand<string> ExecuteCommand { get;private set; }


        #region 属性

        private RegisterDto userDto;

        public RegisterDto UserDto
        {
            get { return userDto; }
            set { userDto = value;RaisePropertyChanged(); }
        }


        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }
        private string account;

        /// <summary>
        /// 账号
        /// </summary>
        public string Account
        {
            get { return account; }
            set { account = value;RaisePropertyChanged(); }
        }

        private string passWord;

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; RaisePropertyChanged(); }
        }

        #endregion

    }
}
