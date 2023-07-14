using MyToDo.Service;
using Prism.Commands;
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
        public LoginViewModel(ILoginService service)
        {
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.service = service;
        }

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "Login": Login();break;
                case "LoginOut":LoginOut();break;
            }
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

            if (loginresult.Status)
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }

            //登陆失败提示...

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
