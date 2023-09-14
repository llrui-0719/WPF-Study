using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Model;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels.Dialogs
{
    public class AddToDoViewModel : BindableBase,IDialogHostAware
    {
        public AddToDoViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private bool isReadOnly;

        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set { isReadOnly = value;RaisePropertyChanged(); }
        }


        private ToDo model;
        /// <summary>
        /// 新增或编辑的实体
        /// </summary>
        public ToDo Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));//取消返回no,操作结束
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Model.Title) || string.IsNullOrWhiteSpace(Model.Content)) return;

            if (DialogHost.IsDialogOpen(DialogHostName))
            {
                DialogParameters param = new DialogParameters();
                param.Add("Value", Model);
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
            }
        }

        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Model = parameters.GetValue<ToDo>("Value");
                IsReadOnly = true;
            }
            else
            {
                Model = new ToDo();
                IsReadOnly = false;
            }
        }
    }
}
