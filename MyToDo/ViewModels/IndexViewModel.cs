using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Model;
using MyToDo.Service;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
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
        private readonly IDialogHostService dialog;
        private readonly IRegionManager regionManager;

        public IndexViewModel(IContainerProvider provider,IDialogHostService dialog):base(provider)
        {
            Title = $"{DateTime.Now.GetDateTimeFormats('D')[1].ToString()},你好{AppSession.UserName}";
            ExcuteCommand = new DelegateCommand<string>(Excute);
            EditMemoCommand = new DelegateCommand<Memo>(AddMemo);
            EditToDoCommand = new DelegateCommand<ToDo>(AddToDo);
            ToDoCompltedCommand = new DelegateCommand<ToDo>(Complted);
            NavigateCommand = new DelegateCommand<TaskBar>(Navigate);
            this.regionManager = provider.Resolve<IRegionManager>();
            this.todoService = provider.Resolve<IToDoService>();
            this.memoService = provider.Resolve<IMemoService>();
            this.dialog = dialog;
            CreateTaskBars();
        }

        private void Navigate(TaskBar obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Target)) return;

            NavigationParameters param = new NavigationParameters();
            if (obj.Title.Equals("已完成"))
            {
                param.Add("Value",2);
            }

            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.Target,param);
        }

        private async void Complted(ToDo obj)
        {
            try
            {
                UpdateLoading(true);
                var result = await todoService.UpdateAsync(obj);
                if (result.Status)
                {
                    var todo = summary.ToDoList.FirstOrDefault(x => x.Id == obj.Id);
                    if (todo != null)
                    {
                        summary.ToDoList.Remove(todo);
                        summary.CompletedCount += 1;
                        summary.CompletedRadio = (summary.CompletedCount / (double)summary.Sum).ToString("0%");
                        this.Refresh();
                    }
                    aggregator.SendMessage($"待办{obj.Title}，已完成");
                }
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        private void Excute(string obj)
        {
            switch (obj)
            {
                case "新增待办":AddToDo(null);break;
                case "新增备忘录":AddMemo(null);break;
            }
        }
        /// <summary>
        /// 添加待办事项
        /// </summary>
        private async void AddToDo(ToDo model)
        {
            DialogParameters param = new DialogParameters();
            if (model!=null)
            {
                param.Add("Value", model);
            }

            var result=await dialog.ShowDialog("AddToDoView", param);
            if (result.Result == ButtonResult.OK)
            {
                try
                {
                    UpdateLoading(true);
                    var todo = result.Parameters.GetValue<ToDo>("Value");
                    if (todo.Id > 0)//更新
                    {
                        var updateresult = await todoService.UpdateAsync(todo);
                        if (updateresult.Status)
                        {
                            var Info = summary.ToDoList.FirstOrDefault(x => x.Id == todo.Id);
                            Info.Title = todo.Title;
                            Info.Content = todo.Content;
                            aggregator.SendMessage($"待办{Info.Title},更新成功");
                        }
                    }
                    else//新增
                    {
                        var addresult = todoService.Add(todo);
                        //if (addresult.Status)
                        //{
                        //    summary.ToDoList.Add(addresult.Result);
                        //    summary.Sum += 1;
                        //    summary.CompletedRadio = (summary.CompletedCount / (double)summary.Sum).ToString("0%");
                        //    this.Refresh();
                        //}
                    }
                }
                finally
                {
                    UpdateLoading(false);
                }
            }
        }

        /// <summary>
        /// 添加备忘录
        /// </summary>
        private async void AddMemo(Memo model)
        {
            DialogParameters param = new DialogParameters();
            if (model != null)
            {
                param.Add("Value", model);
            }
            var result = await dialog.ShowDialog("AddMemoView", param);
            if (result.Result == ButtonResult.OK)
            {
                try
                {
                    UpdateLoading(true);
                    var memo = result.Parameters.GetValue<Memo>("Value");
                    if (memo.Id > 0)
                    {
                        var updateresult = await memoService.UpdateAsync(memo);
                        if (updateresult.Status)
                        {
                            var Info = summary.MemoList.FirstOrDefault(x => x.Id == memo.Id);
                            Info.Title = memo.Title;
                            Info.Content = memo.Content;
                            aggregator.SendMessage($"备忘录{Info.Title},更新成功");
                        }
                    }
                    else
                    {
                        var addresult = memoService.Add(memo);
                        if (addresult.Status)
                        {
                            summary.MemoList.Add(addresult.Result);
                            summary.MemoCount += 1;
                            this.Refresh();
                        }
                    }
                }
                finally
                {
                    UpdateLoading(false);
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

        private string title;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value;RaisePropertyChanged(); }
        }



        /// <summary>
        /// 首页统计
        /// </summary>
        private SummaryDto summary;

        public SummaryDto Summary
        {
            get { return summary; }
            set { summary = value; RaisePropertyChanged(); }
        }
        #endregion

        public DelegateCommand<ToDo> ToDoCompltedCommand { get; private set; }

        public DelegateCommand<ToDo> EditToDoCommand { get; private set; }
        public DelegateCommand<Memo> EditMemoCommand { get; private set; }
        public DelegateCommand<string> ExcuteCommand { get; private set; }

        public DelegateCommand<TaskBar> NavigateCommand { get; private set; }

        void CreateTaskBars()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            TaskBars.Add(new TaskBar() {Icon="ClockFast",Title="汇总",Color="#FF0CA0FF",Target="ToDoView" });
            TaskBars.Add(new TaskBar() {Icon="ClockCheckOutline",Title="已完成",Color="#FF1ECA3A",Target= "ToDoView" });
            TaskBars.Add(new TaskBar() {Icon="ChartLineVariant",Title="完成比例", Color="#FF02C6DC",Target="" });
            TaskBars.Add(new TaskBar() {Icon="PlaylistStar",Title="备忘录", Color="#FFFFA000",Target="MemoView" });

        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            //var summaryResult=await todoService.SummaryAsync();
            //if (summaryResult.Status)
            //{
            //    Summary = summaryResult.Result;
            //    Refresh();
            //}

            base.OnNavigatedTo(navigationContext);
        }

        void Refresh()
        {
            TaskBars[0].Content = summary.Sum.ToString();
            TaskBars[1].Content = summary.CompletedCount.ToString();
            TaskBars[2].Content = summary.CompletedRadio;
            TaskBars[3].Content = summary.MemoCount.ToString();
        }

    }
}