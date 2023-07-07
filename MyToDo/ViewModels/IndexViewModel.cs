using MyToDo.Common.Models;
using Prism.Mvvm;
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
        public IndexViewModel()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            CreateTaskBars();
            CreateTestData();
        }

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

        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }

        void CreateTaskBars()
        {
            TaskBars.Add(new TaskBar() {Icon="ClockFast",Title="汇总",Content="9",Color="#FF0CA0FF",Target="" });
            TaskBars.Add(new TaskBar() {Icon="ClockCheckOutline",Title="已完成",Content="9",Color="#FF1ECA3A",Target="" });
            TaskBars.Add(new TaskBar() {Icon="ChartLineVariant",Title="完成比例",Content="100",Color="#FF02C6DC",Target="" });
            TaskBars.Add(new TaskBar() {Icon="PlaylistStar",Title="备忘录",Content="4",Color="#FFFFA000",Target="" });
        }

        void CreateTestData()
        {
            toDoDtos = new ObservableCollection<ToDoDto>();
            memoDtos = new ObservableCollection<MemoDto>();
            for(var i = 1; i < 10; i++)
            {
                toDoDtos.Add(new ToDoDto() { Title = "待办标题："+i, Content = "待办内容：" + i, Status = i });
                memoDtos.Add(new MemoDto() { Title = "备忘录标题："+i, Content = "备忘录内容："+i, Status = i });
            }
        }

    }
}
