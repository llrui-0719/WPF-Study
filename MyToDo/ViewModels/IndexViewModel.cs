using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class IndexViewModel:BindableBase
    {
        public IndexViewModel(IDialogHostService dialog)
        {
            TaskBars = new ObservableCollection<TaskBar>();
            CreateTaskBars();
            ToDoDtos = new ObservableCollection<ToDoDto>();
            MemoDtos = new ObservableCollection<MemoDto>();
            ExcuteCommand = new DelegateCommand<string>(Excute);
            this.dialog = dialog;
        }

        private void Excute(string obj)
        {
            switch (obj)
            {
                case "新增待办":AddTodo();break;
                case "新增备忘录":AddMemo();break;
            }
        }
        private void AddTodo()
        {
            dialog.ShowDialog("AddToDoView",null);
        }

        private void AddMemo()
        {
            dialog.ShowDialog("AddMemoView",null);
        }


        #region 属性
        private ObservableCollection<TaskBar> taskBars;

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<ToDoDto> toDoDtos;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<MemoDto> memoDtos;
        private readonly IDialogHostService dialog;

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }
        #endregion
        public DelegateCommand<string> ExcuteCommand { get; private set; }

        void CreateTaskBars()
        {
            TaskBars.Add(new TaskBar() {Icon="ClockFast",Title="汇总",Content="9",Color="#FF0CA0FF",Target="" });
            TaskBars.Add(new TaskBar() {Icon="ClockCheckOutline",Title="已完成",Content="9",Color="#FF1ECA3A",Target="" });
            TaskBars.Add(new TaskBar() {Icon="ChartLineVariant",Title="完成比例",Content="100",Color="#FF02C6DC",Target="" });
            TaskBars.Add(new TaskBar() {Icon="PlaylistStar",Title="备忘录",Content="4",Color="#FFFFA000",Target="" });
        }

    }
}