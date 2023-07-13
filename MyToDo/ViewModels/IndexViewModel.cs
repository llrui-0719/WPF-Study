﻿using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameter;
using Prism.Commands;
using Prism.Ioc;
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
    public class IndexViewModel:NavigationViewModel
    {
        private readonly IToDoService todoService;
        private readonly IMemoService memoService;

        public IndexViewModel(IContainerProvider provider,IDialogHostService dialog):base(provider)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            MemoDtos = new ObservableCollection<MemoDto>();
            ExcuteCommand = new DelegateCommand<string>(Excute);
            this.todoService = provider.Resolve<IToDoService>();
            this.memoService = provider.Resolve<IMemoService>();
            this.dialog = dialog;
            CreateTaskBars();
            CreateData();
        }

        private void Excute(string obj)
        {
            switch (obj)
            {
                case "新增待办":AddTodo();break;
                case "新增备忘录":AddMemo();break;
            }
        }
        /// <summary>
        /// 添加待办事项
        /// </summary>
        private async void AddTodo()
        {
            var result=await dialog.ShowDialog("AddToDoView",null);
            if (result.Result == ButtonResult.OK)
            {
                var todo= result.Parameters.GetValue<ToDoDto>("Value");
                if (todo.Id > 0)
                {

                }
                else
                {
                    var addresult=await todoService.AddAsync(todo);
                    if (addresult.Status)
                    {
                        ToDoDtos.Add(addresult.Result);
                    }
                }
            }
        }

        /// <summary>
        /// 添加备忘录
        /// </summary>
        private async void AddMemo()
        {
            var result = await dialog.ShowDialog("AddMemoView", null);
            if (result.Result == ButtonResult.OK)
            {
                var memo = result.Parameters.GetValue<MemoDto>("Value");
                if (memo.Id > 0)
                {

                }
                else
                {
                    var addresult = await memoService.AddAsync(memo);
                    if (addresult.Status)
                    {
                        MemoDtos.Add(addresult.Result);
                    }
                }
            }
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

        async void CreateTaskBars()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            var huizong = 0;
            var yiwancheng = 0;
            double wanchengbili = 0.0;
            var beiwanglu = 0;
            var todolistresult=await todoService.GetAllFilterAsync(new ToDoParameter() { 
                PageIndex=0,
                PageSize=100,
            });
            if (todolistresult.Status)
            {
                huizong = todolistresult.Result.Items.Count();
                yiwancheng = todolistresult.Result.Items.Where(x => x.Status == 1).Count();
                wanchengbili =yiwancheng / huizong;
            }
            var memolistresult = await memoService.GetAllAsync(new QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100,
            });
            if (memolistresult.Status)
            {
                beiwanglu = memolistresult.Result.Items.Count();
            }
            TaskBars.Add(new TaskBar() {Icon="ClockFast",Title="汇总",Content=huizong.ToString(),Color="#FF0CA0FF",Target="" });
            TaskBars.Add(new TaskBar() {Icon="ClockCheckOutline",Title="已完成",Content=yiwancheng.ToString(),Color="#FF1ECA3A",Target="" });
            TaskBars.Add(new TaskBar() {Icon="ChartLineVariant",Title="完成比例",Content= wanchengbili.ToString(), Color="#FF02C6DC",Target="" });
            TaskBars.Add(new TaskBar() {Icon="PlaylistStar",Title="备忘录",Content= beiwanglu.ToString(), Color="#FFFFA000",Target="" });
        }

        async void CreateData()
        {
            var todolist =await todoService.GetAllFilterAsync(new ToDoParameter(){
                PageIndex=0,
                PageSize=100,
            });
            if (todolist.Status)
            {
                foreach(var info in todolist.Result.Items)
                {
                    ToDoDtos.Add(info);
                }
            }

            var memolist = await memoService.GetAllAsync(new QueryParameter() {
                PageIndex=0,
                PageSize=100,
            });
            if (memolist.Status)
            {
                foreach(var info in memolist.Result.Items){
                    MemoDtos.Add(info);
                }
            }
        }
    }
}